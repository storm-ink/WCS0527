using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class LocationData:DeviceStatusData
    {
        public String PosNo { get; private set; }
        public String Status { get; private set; }

        public LocationData(DataRow row):base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "LocationDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 LocationData 类型");
            }

            this.PosNo = Convert.ToString(row["AlarmDataLog_PosNo"]);
            this.Status = Convert.ToString(row["LocationDataLog_Status"]);
        }

        public override string ToDetails()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "货位号", this.PosNo);
            sb.AppendFormat("{0}：{1}\n", "状态", this.Status);

            return sb.ToString();
        }
    }
}
