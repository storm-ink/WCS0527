using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs.Framework;

namespace Wcs.App.Plugins.HomePage
{
    public class DeviceAlarm
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public bool IsOK { get; set; }
        public string IsConnected { get; set; }
        public string IsLocked { get; set; }
        public string Locker { get; set; }
        public string IsAlarm { get; set; }
        public string AlarmDescription { get; set; }
        public DateTime UpDateAt { get; set; }

        public override string ToString()
        {
            return $"{DeviceName}-{DeviceType}(是否正常:{IsOK},是否连接:{IsConnected},是否锁定:{IsLocked},锁定IP:{Locker},是否故障：{IsAlarm},故障描述:{AlarmDescription})";
        }
    }
}
