using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using NLog;
using NLog.Config;
using Wcs.Framework.Cfg;

namespace WcsConsoleApplication
{
    internal class Program
    {
        private static string[] AssemblySearchDirectories = new string[0];
        private static string ValidationConfigurationPath;
        private static bool ValidationProcessMode;
        private static int _assemblyResolverRegistered;
        private static readonly ManualResetEvent ShutdownEvent = new ManualResetEvent(false);
        private static int _shutdownRequested;

        private static int Main(string[] args)
        {
            try
            {
                HostOptions options = HostOptions.Parse(args);
                ValidationProcessMode = options.ValidateConfigurationOnly;
                if (options.ShowHelp)
                {
                    HostOptions.WriteHelp(Console.Out);
                    return 0;
                }

                Bootstrap(options);

                if (options.ValidateConfigurationOnly)
                {
                    ConfigureValidationLogging();
                    ValidateConfiguration();
                    Environment.Exit(0);
                    return 0;
                }

                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Console.CancelKeyPress += Console_CancelKeyPress;

                Console.WriteLine("Starting WCS host...");
                WcsConfiguration.StartApplication(null);
                Console.WriteLine("WCS host started.");

                if (!options.WaitForExit)
                {
                    return 0;
                }

                Console.WriteLine("Press Ctrl+C to stop the host.");
                ShutdownEvent.WaitOne();
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("WCS host failed: " + ex);
                if (ValidationProcessMode)
                {
                    Environment.Exit(1);
                }
                return 1;
            }
        }

        private static void Bootstrap(HostOptions options)
        {
            string configPath = null;
            string effectiveConfigPath = null;
            if (!string.IsNullOrWhiteSpace(options.ConfigurationPath))
            {
                configPath = Path.GetFullPath(options.ConfigurationPath);
                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException("The configuration file does not exist.", configPath);
                }
            }

            string baseDirectory = null;
            if (!string.IsNullOrWhiteSpace(options.BaseDirectory))
            {
                baseDirectory = Path.GetFullPath(options.BaseDirectory);
            }
            else if (!string.IsNullOrWhiteSpace(configPath))
            {
                baseDirectory = Path.GetDirectoryName(configPath);
            }
            else
            {
                baseDirectory = Environment.CurrentDirectory;
            }

            Directory.SetCurrentDirectory(baseDirectory);
            ConfigureAssemblyResolution(baseDirectory);
            AppDomain.CurrentDomain.SetData("WCS_VALIDATE_CONFIG_ONLY", options.ValidateConfigurationOnly);
            Console.WriteLine("Working directory: " + baseDirectory);

            if (!string.IsNullOrWhiteSpace(configPath))
            {
                if (options.ValidateConfigurationOnly)
                {
                    ValidationConfigurationPath = configPath;
                }

                effectiveConfigPath = options.ValidateConfigurationOnly
                    ? CreateValidationConfig(configPath)
                    : configPath;

                ConfigurationFileSwitcher.Use(effectiveConfigPath);
                Console.WriteLine("Using external config: " + configPath);
                if (!string.Equals(configPath, effectiveConfigPath, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Validation config: " + effectiveConfigPath);
                }
            }
            else
            {
                Console.WriteLine("Using embedded app config.");
            }
        }

        private static void ValidateConfiguration()
        {
            WcsConfiguration configuration = string.IsNullOrWhiteSpace(ValidationConfigurationPath)
                ? WcsConfiguration.Instance
                : LoadValidationConfiguration(ValidationConfigurationPath);
            int startupCount = configuration.ApplicationElement != null
                && configuration.ApplicationElement.StartupSelection != null
                && configuration.ApplicationElement.StartupSelection.StartupElements != null
                ? configuration.ApplicationElement.StartupSelection.StartupElements.Length
                : 0;

            int deviceCount = configuration.DeviceCollection != null
                && configuration.DeviceCollection.ParticularDeviceCollection != null
                ? configuration.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements ?? new Wcs.Framework.Cfg.DeviceElement[] { }).Count()
                : 0;

            int locationCount = configuration.LocationCollection != null
                && configuration.LocationCollection.ParticularLocationCollection != null
                ? configuration.LocationCollection.ParticularLocationCollection.SelectMany(x => x.LocationElements ?? new Wcs.Framework.Cfg.LocationElement[] { }).Count()
                : 0;

            int routeCount = configuration.RouteCollection != null
                && configuration.RouteCollection.RouteElements != null
                ? configuration.RouteCollection.RouteElements.Count()
                : 0;

            Console.WriteLine("Configuration loaded successfully.");
            Console.WriteLine("Startups: {0}", startupCount);
            Console.WriteLine("Devices: {0}", deviceCount);
            Console.WriteLine("Locations: {0}", locationCount);
            Console.WriteLine("Routes: {0}", routeCount);
        }

        private static void ConfigureValidationLogging()
        {
            LogManager.Configuration = new LoggingConfiguration();
        }

        private static void ConfigureAssemblyResolution(string baseDirectory)
        {
            AssemblySearchDirectories = GetAssemblySearchDirectories(baseDirectory);
            AppDomain.CurrentDomain.SetData("WCS_CONFIG_BASE_DIRECTORY", baseDirectory);
            TryPreloadAssembly("ZHQXC.dll");
            if (Interlocked.Exchange(ref _assemblyResolverRegistered, 1) == 1)
            {
                return;
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static string CreateValidationConfig(string configPath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(configPath);

            XmlNode configSections = document.SelectSingleNode("/configuration/configSections");
            if (configSections != null)
            {
                XmlNode nlogSection = configSections.SelectSingleNode("section[@name='nlog']");
                if (nlogSection != null)
                {
                    configSections.RemoveChild(nlogSection);
                }
            }

            XmlNode nlogNode = document.SelectSingleNode("/configuration/nlog");
            if (nlogNode != null && nlogNode.ParentNode != null)
            {
                nlogNode.ParentNode.RemoveChild(nlogNode);
            }

            string validationConfigPath = Path.Combine(
                Path.GetTempPath(),
                "WcsConsoleApplication.validation." + Guid.NewGuid().ToString("N") + ".config");

            document.Save(validationConfigPath);
            return validationConfigPath;
        }

        private static WcsConfiguration LoadValidationConfiguration(string configPath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(configPath);
            ExpandXmlFileAttributes(document, Path.GetDirectoryName(configPath));

            XmlNode wcsConfigurationNode = document.SelectSingleNode("/configuration/wcs-configuration");
            if (wcsConfigurationNode == null)
            {
                throw new ConfigurationErrorsException("Missing wcs-configuration section in " + configPath);
            }

            return new WcsConfiguration(wcsConfigurationNode);
        }

        private static void ExpandXmlFileAttributes(XmlDocument document, string configDirectory)
        {
            XmlNodeList redirectedNodes = document.SelectNodes("//*[@xmlFile]");
            if (redirectedNodes == null)
            {
                return;
            }

            foreach (XmlNode redirectedNode in redirectedNodes)
            {
                if (redirectedNode.Attributes == null)
                {
                    continue;
                }

                XmlAttribute xmlFileAttribute = redirectedNode.Attributes["xmlFile"];
                if (xmlFileAttribute == null || string.IsNullOrWhiteSpace(xmlFileAttribute.Value) || Path.IsPathRooted(xmlFileAttribute.Value))
                {
                    continue;
                }

                xmlFileAttribute.Value = Path.GetFullPath(Path.Combine(configDirectory, xmlFileAttribute.Value));
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (AssemblySearchDirectories == null || AssemblySearchDirectories.Length == 0)
            {
                return null;
            }

            string assemblyName = new AssemblyName(args.Name).Name;
            foreach (string searchDirectory in AssemblySearchDirectories)
            {
                string[] candidateFiles = new[]
                {
                    Path.Combine(searchDirectory, assemblyName + ".dll"),
                    Path.Combine(searchDirectory, assemblyName + ".exe")
                };

                foreach (string candidateFile in candidateFiles)
                {
                    if (File.Exists(candidateFile))
                    {
                        return Assembly.LoadFrom(candidateFile);
                    }
                }
            }

            return null;
        }

        private static void TryPreloadAssembly(string fileName)
        {
            if (AssemblySearchDirectories == null || AssemblySearchDirectories.Length == 0)
            {
                return;
            }

            foreach (string searchDirectory in AssemblySearchDirectories)
            {
                string assemblyPath = Path.Combine(searchDirectory, fileName);
                if (!File.Exists(assemblyPath))
                {
                    continue;
                }

                try
                {
                    Assembly.LoadFrom(assemblyPath);
                    return;
                }
                catch
                {
                }
            }
        }

        private static string[] GetAssemblySearchDirectories(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                return new string[0];
            }

            string[] candidateDirectories = new[]
            {
                baseDirectory,
                Path.Combine(baseDirectory, "bin"),
                Path.Combine(baseDirectory, "bin", "Debug"),
                Path.Combine(baseDirectory, "bin", "Release")
            };

            return candidateDirectories
                .Where(Directory.Exists)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            if (Interlocked.Exchange(ref _shutdownRequested, 1) == 1)
            {
                return;
            }

            Console.WriteLine("Shutdown requested. Exiting host...");
            ShutdownEvent.Set();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Console.Error.WriteLine("Unhandled exception: " + (ex == null ? e.ExceptionObject.ToString() : ex.ToString()));
        }
    }
}
