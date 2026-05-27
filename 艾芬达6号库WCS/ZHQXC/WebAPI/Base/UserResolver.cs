using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

namespace ZHQXC
{
    public class UserResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            List<Assembly> assemblies = base.GetAssemblies().ToList();
            var _config = ConfigurationManager.GetSection("webApiAssemblies") as WebApiAssembliesLoadElement;
            if (_config == null || _config.dlls == null)
            {
                return assemblies;
            }

            HashSet<string> loadedAssemblyPaths = new HashSet<string>(
                assemblies
                    .Where(x => !string.IsNullOrWhiteSpace(x.Location))
                    .Select(x => x.Location),
                StringComparer.OrdinalIgnoreCase);

            foreach (var item in _config.dlls)
            {
                Assembly controllersAssembly = ResolveAssembly(item);
                if (controllersAssembly == null)
                {
                    throw new FileNotFoundException(
                        string.Format("无法定位 WebAPI 控制器程序集 {0}。", item));
                }

                if (loadedAssemblyPaths.Add(controllersAssembly.Location))
                {
                    assemblies.Add(controllersAssembly);
                }
            }

            return assemblies;
        }

        static Assembly ResolveAssembly(string configuredAssemblyName)
        {
            if (string.IsNullOrWhiteSpace(configuredAssemblyName))
            {
                return null;
            }

            string normalizedAssemblyName = Path.GetFileNameWithoutExtension(configuredAssemblyName);
            Assembly loadedAssembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(x => string.Equals(x.GetName().Name, normalizedAssemblyName, StringComparison.OrdinalIgnoreCase));
            if (loadedAssembly != null)
            {
                return loadedAssembly;
            }

            foreach (string searchDirectory in GetSearchDirectories())
            {
                string assemblyPath = Path.Combine(searchDirectory, configuredAssemblyName);
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
            }

            return null;
        }

        static IEnumerable<string> GetSearchDirectories()
        {
            string configuredBaseDirectory = AppDomain.CurrentDomain.GetData("WCS_CONFIG_BASE_DIRECTORY") as string;
            string currentBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string[] candidateDirectories = new[]
            {
                currentBaseDirectory,
                configuredBaseDirectory,
                string.IsNullOrWhiteSpace(configuredBaseDirectory) ? null : Path.Combine(configuredBaseDirectory, "bin"),
                string.IsNullOrWhiteSpace(configuredBaseDirectory) ? null : Path.Combine(configuredBaseDirectory, "bin", "Debug"),
                string.IsNullOrWhiteSpace(configuredBaseDirectory) ? null : Path.Combine(configuredBaseDirectory, "bin", "Release")
            };

            return candidateDirectories
                .Where(x => !string.IsNullOrWhiteSpace(x) && Directory.Exists(x))
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }
}
