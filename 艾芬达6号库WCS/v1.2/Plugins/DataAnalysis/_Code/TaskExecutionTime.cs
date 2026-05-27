using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis
{
    /// <summary>
    /// 任务持续时间
    /// </summary>
    public class TaskExecutionTime
    {
        public TaskTime FromState { get; set; }
        public TaskTime ToState { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        public string TaskId
        {
            get
            {
                return this.FromState.TaskId;
            }
        }
        /// <summary>
        /// 原始状态的持续秒数
        /// </summary>
        public double TotalSeconds
        {
            get
            {
                return ToState.StartTime.Subtract(FromState.StartTime).TotalSeconds;
            }
        }

        public override string ToString()
        {
            if (FromState.StartTime.Date == ToState.StartTime.Date)
            {
                return string.Format("{0} 从 {1:HH:mm:ss} 执行到 {2:HH:mm:ss}，共保持了 {3} 秒", FromState.TaskId, FromState.StartTime, ToState.StartTime, TotalSeconds);
            }
            else
            {
                return string.Format("{0} 从 {2} 执行到 {3}，共保持了 {3} 秒", FromState.TaskId, FromState.StartTime, ToState.StartTime, TotalSeconds);
            }
        }
    }
}
