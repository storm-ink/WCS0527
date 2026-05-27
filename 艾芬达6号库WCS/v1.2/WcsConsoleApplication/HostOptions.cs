using System;
using System.IO;

namespace WcsConsoleApplication
{
    internal sealed class HostOptions
    {
        public string BaseDirectory { get; private set; }
        public string ConfigurationPath { get; private set; }
        public bool ShowHelp { get; private set; }
        public bool ValidateConfigurationOnly { get; private set; }
        public bool WaitForExit { get; private set; }

        private HostOptions()
        {
            WaitForExit = true;
        }

        public static HostOptions Parse(string[] args)
        {
            HostOptions options = new HostOptions();
            if (args == null || args.Length == 0)
            {
                return options;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }

                if (Matches(arg, "-h") || Matches(arg, "--help") || Matches(arg, "/?"))
                {
                    options.ShowHelp = true;
                    continue;
                }

                if (Matches(arg, "--validate-config"))
                {
                    options.ValidateConfigurationOnly = true;
                    continue;
                }

                if (Matches(arg, "--no-wait"))
                {
                    options.WaitForExit = false;
                    continue;
                }

                if (TryReadValue(args, ref i, arg, "--config", out arg))
                {
                    options.ConfigurationPath = arg;
                    continue;
                }

                if (TryReadValue(args, ref i, arg, "--base-directory", out arg))
                {
                    options.BaseDirectory = arg;
                    continue;
                }

                throw new ArgumentException("Unknown argument: " + args[i]);
            }

            return options;
        }

        public static void WriteHelp(TextWriter writer)
        {
            writer.WriteLine("WcsConsoleApplication host");
            writer.WriteLine();
            writer.WriteLine("Options:");
            writer.WriteLine("  --config <path>           Use an external .config file.");
            writer.WriteLine("  --base-directory <path>   Set the working directory before bootstrapping.");
            writer.WriteLine("  --validate-config         Load and validate configuration without starting schedulers.");
            writer.WriteLine("  --no-wait                 Exit immediately after startup.");
            writer.WriteLine("  --help                    Show this help.");
            writer.WriteLine();
            writer.WriteLine("Examples:");
            writer.WriteLine("  WcsConsoleApplication.exe --help");
            writer.WriteLine("  WcsConsoleApplication.exe --config \".\\Wcs.App.exe.config\" --validate-config");
            writer.WriteLine("  WcsConsoleApplication.exe --config \".\\Wcs.App.exe.config\" --base-directory \".\"");
        }

        private static bool Matches(string arg, string expected)
        {
            return string.Equals(arg, expected, StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryReadValue(string[] args, ref int index, string currentArg, string optionName, out string value)
        {
            value = null;
            if (currentArg.StartsWith(optionName + "=", StringComparison.OrdinalIgnoreCase))
            {
                value = currentArg.Substring(optionName.Length + 1).Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Missing value for " + optionName);
                }

                return true;
            }

            if (!Matches(currentArg, optionName))
            {
                return false;
            }

            if (index + 1 >= args.Length || string.IsNullOrWhiteSpace(args[index + 1]))
            {
                throw new ArgumentException("Missing value for " + optionName);
            }

            index++;
            value = args[index].Trim();
            return true;
        }
    }
}
