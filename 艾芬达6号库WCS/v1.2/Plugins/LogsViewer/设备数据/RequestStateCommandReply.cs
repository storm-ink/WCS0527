using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.LogsViewer.设备数据
{
    public class RequestStateCommandReply : DeviceStatusData
    {
        /// <summary>
        /// 堆垛机状态
        /// </summary>
        public String State { get; private set; }
        /// <summary>
        /// 当前所在列
        /// </summary>
        public Int32 Column { get; private set; }
        /// <summary>
        /// 当前所在层
        /// </summary>
        public Int32 Level { get; private set; }
        /// <summary>
        /// 货叉水平位置状态
        /// </summary>
        public String ForkHorizontalPosition { get; private set; }
        /// <summary>
        /// 货叉上下位置状态
        /// </summary>
        public String ForkVerticalPosition { get; private set; }
        /// <summary>
        /// 指示堆垛机当前是否在站点位置
        /// </summary>
        public Boolean AtStation { get; private set; }
        /// <summary>
        /// 错误码（默认为 0）
        /// </summary>
        public Int32 ErrorCode { get; private set; }
        /// <summary>
        /// 堆垛机事件
        /// </summary>
        public String Event { get; private set; }
        /// <summary>
        /// 当前任务号
        /// </summary>
        public String TaskId { get; private set; }

        public RequestStateCommandReply(DataRow row)
            : base(row)
        {
            if (Convert.ToString(row["discriminator"]) != "RequestStateCommandReplyDataLog")
            {
                throw new InvalidOperationException("此行无法转换为 RequestStateCommandReply 类型");
            }

            this.State = Convert.ToString(row["RequestStateCommandReplyDataLog_State"]);
            this.Column = Convert.ToInt32(row["RequestStateCommandReplyDataLog_Column"]);
            this.Level = Convert.ToInt32(row["RequestStateCommandReplyDataLog_Level"]);
            this.ForkHorizontalPosition = Convert.ToString(row["RequestStateCommandReplyDataLog_ForkHorizontalPosition"]);
            this.ForkVerticalPosition = Convert.ToString(row["RequestStateCommandReplyDataLog_ForkVerticalPosition"]);
            this.AtStation = Convert.ToBoolean(row["RequestStateCommandReplyDataLog_AtStation"]);
            this.ErrorCode = Convert.ToInt32(row["RequestStateCommandReplyDataLog_ErrorCode"]);
            this.Event = Convert.ToString(row["RequestStateCommandReplyDataLog_Event"]);
            this.TaskId = Convert.ToString(row["RequestStateCommandReplyDataLog_TaskId"]);
        }

        public override string ToDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}：{1}\n", "状态", this.State);
            sb.AppendFormat("{0}：{1}\n", "列", this.Column);
            sb.AppendFormat("{0}：{1}\n", "层", this.Level);
            sb.AppendFormat("{0}：{1}\n", "货叉水平位置", this.ForkHorizontalPosition);
            sb.AppendFormat("{0}：{1}\n", "货叉垂直位置", this.ForkVerticalPosition);
            sb.AppendFormat("{0}：{1}\n", "是否在站点", this.AtStation);
            sb.AppendFormat("{0}：{1}\n", "错误码", this.ErrorCode);
            sb.AppendFormat("{0}：{1}\n", "事件", this.Event);
            sb.AppendFormat("{0}：{1}\n", "任务号", this.TaskId);

            return sb.ToString();
        }
    }
}
