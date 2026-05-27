using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public abstract class DeviceStatusData
    {
        public Int32 Id { get; protected set; }
        public String DeviceName { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DeviceStatusData(DataRow row)
        {
            Id = Convert.ToInt32(row["Id"]);
            DeviceName = Convert.ToString(row["DeviceName"]);
            CreatedAt = Convert.ToDateTime(row["CreatedAt"]);
        }
        public abstract String ToDetails();

        public override string ToString()
        {
            return ToDetails();
        }
    }
}
