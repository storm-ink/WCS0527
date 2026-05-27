using Matedata;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatedataServer.App
{
    /// <summary>
    /// 元数据服务管理器.
    /// </summary>
    public class MatedataServicesManager
    {
        static List<object> _hosts = new List<object>();
        static Logger _logger = LogManager.GetCurrentClassLogger();
        static MatedataServicesManager()
        {
            var files = System.IO.Directory.GetFiles(Application.StartupPath, "*.dll")
                .Concat(System.IO.Directory.GetFiles(Application.StartupPath, "*.exe"))
                .Distinct();
            foreach (var item in files)
            {
                try
                {
                    var assembly = System.Reflection.Assembly.LoadFile(item);
                    var serviceTypes = assembly.GetTypes()
                        .Where(x => x.IsAbstract==false
                            && x.BaseType.Name != typeof(DiagnoseMatedataServiceProxy<,>).Name
                            && (x.IsSubclassOf(typeof(DiagnoseMatedataService))
                              || x.IsSubclassOf(typeof(DeviceService)))
                            );
                    foreach (var type in serviceTypes)
                    {
                        try
                        {
                            Type hostType = typeof(WcfHosting<>).MakeGenericType(new Type[] { type });

                            var hosting = hostType.Assembly.CreateInstance(hostType.FullName, false, System.Reflection.BindingFlags.CreateInstance, null, null, null, null);
                            hosting.GetType().GetMethod("Launch").Invoke(hosting, null);
                            _hosts.Add(hosting);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            }
        }

        public static List<object> Hosts
        {
            get
            {
                return _hosts;
            }
        }
    }
}
