using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class HoldSignalData:DeviceStatusData
    {
        public String PosNo { get; private set; }
        public String HandShake { get; private set; }
        public String AssignmentID { get; private set; }
        public String TU_ID { get; private set; }
        public String TU_Type { get; private set; }
        public String IO_Data { get; private set; }

        public HoldSignalData(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "HoldSignalDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 HoldSignalData 类型");
            }

            this.PosNo = Convert.ToString(row["HoldSignalDataLog_PosNo"]);
            this.HandShake = Convert.ToString(row["HoldSignalDataLog_HandShake"]);
            this.AssignmentID = Convert.ToString(row["HoldSignalDataLog_AssignmentID"]);
            this.TU_ID = Convert.ToString(row["HoldSignalDataLog_TU_ID"]);
            this.TU_Type = Convert.ToString(row["HoldSignalDataLog_TU_Type"]);
            this.IO_Data = Convert.ToString(row["HoldSignalDataLog_IO_Data"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "货位号", this.PosNo);
            sb.AppendFormat("{0}：{1}\n", "握手", this.HandShake);
            sb.AppendFormat("{0}：{1}\n", "任务号", this.AssignmentID);
            sb.AppendFormat("{0}：{1}\n", "托盘号", this.TU_ID);
            sb.AppendFormat("{0}：{1}\n", "托盘类型", this.TU_Type);
            sb.AppendFormat("{0}：{1}\n", "业务数据", this.IO_Data);

            return sb.ToString();
        }
    }
}
