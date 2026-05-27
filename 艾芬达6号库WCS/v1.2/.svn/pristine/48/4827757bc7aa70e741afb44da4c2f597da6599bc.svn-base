using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class TaskData:DeviceStatusData
    {
        public String HandShake { get; private set; }
        public String AssignmentID { get; private set; }
        public String TU_ID { get; private set; }
        public String TU_Type { get; private set; }
        public String IO_Data { get; private set; }
        public String RotingNo { get; private set; }
        public String StartMotorNo { get; private set; }
        public String DestinationNo { get; private set; }
        public String TaskStatus { get; private set; }
        public String ReadTask { get; private set; }
        public String Spare { get; private set; }

        public TaskData(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "TaskDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 TaskData 类型");
            }

            this.HandShake = Convert.ToString(row["TaskDataLog_HandShake"]);
            this.AssignmentID = Convert.ToString(row["TaskDataLog_AssignmentID"]);
            this.TU_ID = Convert.ToString(row["TaskDataLog_TU_ID"]);
            this.TU_Type = Convert.ToString(row["TaskDataLog_TU_Type"]);
            this.IO_Data = Convert.ToString(row["TaskDataLog_IO_Data"]);
            this.RotingNo = Convert.ToString(row["TaskDataLog_RotingNo"]);
            this.StartMotorNo = Convert.ToString(row["TaskDataLog_StartMotorNo"]);
            this.DestinationNo = Convert.ToString(row["TaskDataLog_DestinationNo"]);
            this.TaskStatus = Convert.ToString(row["TaskDataLog_TaskStatus"]);
            this.ReadTask = Convert.ToString(row["TaskDataLog_ReadTask"]);
            this.Spare = Convert.ToString(row["TaskDataLog_Spare"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "握手", this.HandShake);
            sb.AppendFormat("{0}：{1}\n", "任务号", this.AssignmentID);
            sb.AppendFormat("{0}：{1}\n", "托盘号", this.TU_ID);
            sb.AppendFormat("{0}：{1}\n", "托盘类型", this.TU_Type);
            sb.AppendFormat("{0}：{1}\n", "业务数据", this.IO_Data);
            sb.AppendFormat("{0}：{1}\n", "路径号", this.RotingNo);
            sb.AppendFormat("{0}：{1}\n", "开始位置", this.StartMotorNo);
            sb.AppendFormat("{0}：{1}\n", "结束位置", this.DestinationNo);
            sb.AppendFormat("{0}：{1}\n", "任务状态", this.TaskStatus);
            sb.AppendFormat("{0}：{1}\n", "读取号", this.ReadTask);
            sb.AppendFormat("{0}：{1}\n", "备用", this.Spare);

            return sb.ToString();
        }
    }
}
