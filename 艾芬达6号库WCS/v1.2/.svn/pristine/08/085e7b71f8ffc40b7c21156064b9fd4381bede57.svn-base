using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class AlarmData:DeviceStatusData
    {
        public String PosNo { get; private set; }
        public String Manual { get; private set; }
        public String Isolator { get; private set; }
        public String Breaker { get; private set; }
        public String Photocell { get; private set; }
        public String RunOvertime { get; private set; }
        public String OccupyOvertime { get; private set; }
        public String TaskNoGoods { get; private set; }
        public String MotorUseStatus { get; private set; }
        public String X_MotorVAF { get; private set; }
        public String Y_MotorVAF { get; private set; }
        public String X_MotorContactor { get; private set; }
        public String X_MotorBraker { get; private set; }
        public String Y_MotorContactor { get; private set; }
        public String Y_MotorBraker { get; private set; }
        public String Lift_MotorContactor { get; private set; }
        public String Lift_MotorBraker { get; private set; }
        public String Spare { get; private set; }

        public AlarmData(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "AlarmDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 AlarmData 类型");
            }

            this.PosNo = Convert.ToString(row["AlarmDataLog_PosNo"]);
            this.Manual = Convert.ToString(row["AlarmDataLog_Manual"]);
            this.Isolator = Convert.ToString(row["AlarmDataLog_Isolator"]);
            this.Breaker = Convert.ToString(row["AlarmDataLog_Breaker"]);
            this.Photocell = Convert.ToString(row["AlarmDataLog_Photocell"]);
            this.RunOvertime = Convert.ToString(row["AlarmDataLog_RunOvertime"]);
            this.OccupyOvertime = Convert.ToString(row["AlarmDataLog_OccupyOvertime"]);
            this.TaskNoGoods = Convert.ToString(row["AlarmDataLog_TaskNoGoods"]);
            this.MotorUseStatus = Convert.ToString(row["AlarmDataLog_MotorUseStatus"]);
            this.X_MotorVAF = Convert.ToString(row["AlarmDataLog_X_MotorVAF"]);
            this.Y_MotorVAF = Convert.ToString(row["AlarmDataLog_Y_MotorVAF"]);
            this.X_MotorContactor = Convert.ToString(row["AlarmDataLog_X_MotorContactor"]);
            this.X_MotorBraker = Convert.ToString(row["AlarmDataLog_X_MotorBraker"]);
            this.Y_MotorContactor = Convert.ToString(row["AlarmDataLog_Y_MotorContactor"]);
            this.Y_MotorBraker = Convert.ToString(row["AlarmDataLog_Y_MotorBraker"]);
            this.Lift_MotorContactor = Convert.ToString(row["AlarmDataLog_Lift_MotorContactor"]);
            this.Lift_MotorBraker = Convert.ToString(row["AlarmDataLog_Lift_MotorBraker"]);
            this.Spare = Convert.ToString(row["AlarmDataLog_Spare"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "货位号", this.PosNo);
            sb.AppendFormat("{0}：{1}\n", "是否手动", this.Manual);
            sb.AppendFormat("{0}：{1}\n", "离线（隔离开关断开）", this.Isolator);
            sb.AppendFormat("{0}：{1}\n", "电路保护器断开（断路器断开）", this.Breaker);
            sb.AppendFormat("{0}：{1}\n", "光电异常", this.Photocell);
            sb.AppendFormat("{0}：{1}\n", "运行超时", this.RunOvertime);
            sb.AppendFormat("{0}：{1}\n", "有任务无货", this.OccupyOvertime);
            sb.AppendFormat("{0}：{1}\n", "X轴电机变频器故障", this.X_MotorVAF);
            sb.AppendFormat("{0}：{1}\n", "Y轴电机变频器故障", this.Y_MotorVAF);
            sb.AppendFormat("{0}：{1}\n", "X轴电机正反转接触器故障", this.X_MotorContactor);
            sb.AppendFormat("{0}：{1}\n", "X轴电机抱闸接触器故障", this.X_MotorBraker);

            sb.AppendFormat("{0}：{1}\n", "Y轴电机正反转接触器故障", this.Y_MotorContactor);
            sb.AppendFormat("{0}：{1}\n", "Y轴电机抱闸接触器故障", this.Y_MotorBraker);
            sb.AppendFormat("{0}：{1}\n", "顶升电机正反转接触器故障", this.Lift_MotorContactor);
            sb.AppendFormat("{0}：{1}\n", "顶升电机抱闸接触器故障", this.Lift_MotorBraker);

            return sb.ToString();
        }
    }
}
