using System;
using System.IO;
using System.Linq;
using System.Threading;
using Wcs.Framework.Cfg;

namespace WcsConsoleApplication
{
    internal class Program
    {
        private static readonly ManualResetEvent ShutdownEvent = new ManualResetEvent(false);
        private static int _shutdownRequested;

        private static int Main(string[] args)
        {
            try
            {
                HostOptions options = HostOptions.Parse(args);
                if (options.ShowHelp)
                {
                    HostOptions.WriteHelp(Console.Out);
                    return 0;
                }

                Bootstrap(options);

                if (options.ValidateConfigurationOnly)
                {
                    ValidateConfiguration();
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
                return 1;
            }
        }

        private static void Bootstrap(HostOptions options)
        {
            string configPath = null;
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
            Console.WriteLine("Working directory: " + baseDirectory);

            if (!string.IsNullOrWhiteSpace(configPath))
            {
                ConfigurationFileSwitcher.Use(configPath);
                Console.WriteLine("Using external config: " + configPath);
            }
            else
            {
                Console.WriteLine("Using embedded app config.");
            }
        }

        private static void ValidateConfiguration()
        {
            WcsConfiguration configuration = WcsConfiguration.Instance;
            int startupCount = configuration.ApplicationElement.StartupSelection.StartupElements.Length;
            int deviceCount = configuration.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements).Count();
            int locationCount = configuration.LocationCollection.ParticularLocationCollection.SelectMany(x => x.LocationElements).Count();
            int routeCount = configuration.RouteCollection.RouteElements.Count();

            Console.WriteLine("Configuration loaded successfully.");
            Console.WriteLine("Startups: {0}", startupCount);
            Console.WriteLine("Devices: {0}", deviceCount);
            Console.WriteLine("Locations: {0}", locationCount);
            Console.WriteLine("Routes: {0}", routeCount);
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
