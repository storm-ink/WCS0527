using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Client.Cfg
{
    public class RemoteDiagnoseSettings
    {
        public Type[] DeviceDiagnosisDataTypes { get; private set; }

        public RemoteDiagnoseSettings(XmlNode node)
        {
            ReadDeviceDiagnosisDataTypes(node);
        }

        void ReadDeviceDiagnosisDataTypes(XmlNode node)
        {
            List<Type> types = new List<Type>();
            foreach (XmlNode item in node.SelectNodes("deviceDiagnosisDataTypes/add"))
            {
                var typeName = item.Attributes["type"].Value;
                var type = Type.GetType(typeName);

                if (type == null)
                {
                    throw new System.Configuration.ConfigurationException("未找到 type 属性指定的类型 " + typeName, node);
                }

                types.Add(type);
            }
            DeviceDiagnosisDataTypes = types.ToArray();
        }

        public static RemoteDiagnoseSettings Instance
        {
            get
            {
                return (RemoteDiagnoseSettings)System.Configuration.ConfigurationManager.GetSection("remoteDiagnose");
            }
        }
    }
}
