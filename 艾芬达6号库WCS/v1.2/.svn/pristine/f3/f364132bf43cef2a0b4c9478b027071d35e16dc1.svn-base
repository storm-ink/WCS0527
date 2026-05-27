using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class LocationTaskData:DeviceStatusData
    {
        public String PosNo { get; private set; }
        public String TaskNo { get; private set; }
        public String TUID { get; private set; }
        public String Str_Rcv_X { get; private set; }
        public String Fnh_Rcv_X { get; private set; }
        public String Rqs_Snt { get; private set; }
        public String Rcv_Rdy { get; private set; }
        public String Str_Rcv_Y { get; private set; }
        public String Fnh_Rcv_Y { get; private set; }

        public LocationTaskData(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "LocationTaskDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 LocationTaskData 类型");
            }

            this.PosNo = Convert.ToString(row["LocationTaskDataLog_PosNo"]);
            this.TaskNo = Convert.ToString(row["LocationTaskDataLog_TaskNo"]);
            this.TUID = Convert.ToString(row["LocationTaskDataLog_TUID"]);
            this.Str_Rcv_X = Convert.ToString(row["LocationTaskDataLog_Str_Rcv_X"]);
            this.Fnh_Rcv_X = Convert.ToString(row["LocationTaskDataLog_Fnh_Rcv_X"]);
            this.Rqs_Snt = Convert.ToString(row["LocationTaskDataLog_Rqs_Snt"]);
            this.Rcv_Rdy = Convert.ToString(row["LocationTaskDataLog_Rcv_Rdy"]);
            this.Str_Rcv_Y = Convert.ToString(row["LocationTaskDataLog_Str_Rcv_Y"]);
            this.Fnh_Rcv_Y = Convert.ToString(row["LocationTaskDataLog_Fnh_Rcv_Y"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "货位号", this.PosNo);
            sb.AppendFormat("{0}：{1}\n", "任务号", this.TaskNo);
            sb.AppendFormat("{0}：{1}\n", "托盘号", this.TUID);
            sb.AppendFormat("{0}：{1}\n", "Str_Rcv_X", this.Str_Rcv_X);
            sb.AppendFormat("{0}：{1}\n", "Fnh_Rcv_X", this.Fnh_Rcv_X);
            sb.AppendFormat("{0}：{1}\n", "Rqs_Snt", this.Rqs_Snt);
            sb.AppendFormat("{0}：{1}\n", "Rcv_Rdy", this.Rcv_Rdy);
            sb.AppendFormat("{0}：{1}\n", "Str_Rcv_Y", this.Str_Rcv_Y);
            sb.AppendFormat("{0}：{1}\n", "Fnh_Rcv_Y", this.Fnh_Rcv_Y);

            return sb.ToString();
        }
    }
}
