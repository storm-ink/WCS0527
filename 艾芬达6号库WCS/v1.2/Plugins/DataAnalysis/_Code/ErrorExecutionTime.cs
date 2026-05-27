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
    public class ErrorExecutionTime
    {
        public ErrorTime FromState { get; set; }
        public ErrorTime ToState { get; set; }
        public string DeviceName
        {
            get
            {
                return this.FromState.DeviceName;
            }
        }
        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code
        {
            get
            {
                return this.FromState.Code;
            }
        }

        public string DisplayName
        {
            get
            {
                return this.FromState.Name;
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


        public DateTime From
        {
            get
            {
                return this.FromState.StartTime;
            }
        }

        public DateTime To
        {
            get
            {
                return this.ToState.StartTime;
            }
        }

        public override string ToString()
        {
            if (FromState.StartTime.Date == ToState.StartTime.Date)
            {
                return string.Format("{0} 从 {1:HH:mm:ss} 执行到 {2:HH:mm:ss}，共保持了 {3} 秒", FromState.Code, FromState.StartTime, ToState.StartTime, TotalSeconds);
            }
            else
            {
                return string.Format("{0} 从 {2} 执行到 {3}，共保持了 {3} 秒", FromState.Code, FromState.StartTime, ToState.StartTime, TotalSeconds);
            }
        }
    }
}
