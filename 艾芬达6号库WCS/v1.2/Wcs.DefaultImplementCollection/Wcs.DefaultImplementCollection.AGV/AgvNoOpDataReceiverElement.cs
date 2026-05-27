using System.Xml;
using Wcs.Framework;
using Wcs.Framework.Cfg;

namespace Wcs.DefaultImplementCollection.AGV
{
    public class AgvNoOpDataReceiverElement : DataReceiverElement
    {
        public AgvNoOpDataReceiverElement(XmlNode node, WcsConfiguration wcsConfiguration)
            : base(node, wcsConfiguration)
        {
        }

        public override IDataReceiver CreateDataReceiver(string deviceName)
        {
            return new AgvNoOpDataReceiver(Name);
        }
    }
}
