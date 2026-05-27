using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Wcs.App.Plugins.堆垛机任务故障查询
{
    public class WcsConfigurationConnectionStrings
    {
        public static String WcsConnectionString
        {
            get
            {
                String currentDir = Path.GetDirectoryName(typeof(WcsConfigurationConnectionStrings).Assembly.Location);
                String wcsConfigPath = Path.Combine(currentDir, "Wcs.App.exe.config");

                if (File.Exists(wcsConfigPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(wcsConfigPath);
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("cfg", "urn:nhibernate-configuration-2.2");
                    var node = doc.SelectSingleNode("configuration/cfg:hibernate-configuration/cfg:session-factory[@name='wcs']/cfg:property[@name='connection.connection_string']", ns);
                    return node.InnerText;
                }

                String hibernateCfgPath = Path.Combine(currentDir, "hibernate.cfg.xml");
                if (File.Exists(hibernateCfgPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(hibernateCfgPath);
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("cfg", "urn:nhibernate-configuration-2.2");
                    var node = doc.SelectSingleNode("cfg:hibernate-configuration/cfg:session-factory[@name='wcs']/cfg:property[@name='connection.connection_string']", ns);
                    return node.InnerText;
                }

                return System.Configuration.ConfigurationManager.ConnectionStrings["wcs"].ConnectionString;
            }
        }
        public static String WcsBakConnectionString
        {
            get
            {
                String currentDir = Path.GetDirectoryName(typeof(WcsConfigurationConnectionStrings).Assembly.Location);
                String wcsConfigPath = Path.Combine(currentDir, "Wcs.App.exe.config");

                if (File.Exists(wcsConfigPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(wcsConfigPath);
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("cfg", "urn:nhibernate-configuration-2.2");
                    var node = doc.SelectSingleNode("configuration/backup-hibernate-configuration/cfg:session-factory[@name='wcs']/cfg:property[@name='connection.connection_string']", ns);
                    return node.InnerText;
                }

                String hibernateCfgPath = Path.Combine(currentDir, "hibernatebackup.cfg.xml");
                if (File.Exists(hibernateCfgPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(hibernateCfgPath);
                    XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                    ns.AddNamespace("cfg", "urn:nhibernate-configuration-2.2");
                    var node = doc.SelectSingleNode("backup-hibernate-configuration/cfg:session-factory[@name='wcs']/cfg:property[@name='connection.connection_string']", ns);
                    return node.InnerText;
                }

                return System.Configuration.ConfigurationManager.ConnectionStrings["wcs_bak"].ConnectionString;
            }
        }
    }
}
