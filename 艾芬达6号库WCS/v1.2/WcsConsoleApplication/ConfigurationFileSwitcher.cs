using System;
using System.Configuration;
using System.Reflection;

namespace WcsConsoleApplication
{
    internal static class ConfigurationFileSwitcher
    {
        public static void Use(string configFilePath)
        {
            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                throw new ArgumentException("Configuration file path is required.", "configFilePath");
            }

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFilePath);
            ResetConfigurationManager();
        }

        private static void ResetConfigurationManager()
        {
            FieldInfo initStateField = typeof(ConfigurationManager).GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static);
            if (initStateField != null)
            {
                initStateField.SetValue(null, 0);
            }

            FieldInfo initErrorField = typeof(ConfigurationManager).GetField("s_initError", BindingFlags.NonPublic | BindingFlags.Static);
            if (initErrorField != null)
            {
                initErrorField.SetValue(null, null);
            }

            FieldInfo configSystemField = typeof(ConfigurationManager).GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static);
            if (configSystemField != null)
            {
                configSystemField.SetValue(null, null);
            }

            Type clientConfigPathsType = typeof(ConfigurationManager).Assembly.GetType("System.Configuration.ClientConfigPaths", false);
            if (clientConfigPathsType == null)
            {
                return;
            }

            FieldInfo currentField = clientConfigPathsType.GetField("s_current", BindingFlags.NonPublic | BindingFlags.Static);
            if (currentField != null)
            {
                currentField.SetValue(null, null);
            }
        }
    }
}
