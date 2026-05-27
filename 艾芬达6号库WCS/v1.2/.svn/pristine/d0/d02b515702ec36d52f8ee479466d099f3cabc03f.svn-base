using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Cfg
{
    public class RemoteDiagnoseConfigurationSelectionHandler:System.Configuration.IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return new RemoteDiagnoseSettings(section);
        }
    }
}
