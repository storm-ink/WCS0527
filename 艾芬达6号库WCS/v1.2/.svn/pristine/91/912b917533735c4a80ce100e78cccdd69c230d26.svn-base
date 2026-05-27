using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis
{
    /// <summary>
    /// 状态持续时间
    /// </summary>
    public class StateExecutionTime
    {
        public StateTime FromState { get; set; }
        public StateTime ToState { get; set; }
        /// <summary>
        /// 设备名称（指状态所属设备，有可能是货位号）
        /// </summary>
        public string DeviceName
        {
            get
            {
                return this.FromState.DeviceName;
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

        public string State
        {
            get
            {
                return this.FromState.State;
            }
        }

        public string StateDisplayName
        {
            get
            {
                return this.FromState.StateDisplayName;
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
                return string.Format("{0} 从 {1:HH:mm:ss} 持续到 {2:HH:mm:ss} 变更为 {3}，共保持了 {4} 秒", FromState.State, FromState.StartTime, ToState.StartTime, ToState.State,TotalSeconds);
            }
            else
            {
                return string.Format("{0} 从 {1} 持续到 {2} 变更为 {3}，共保持了 {4} 秒", FromState.State, FromState.StartTime, ToState.StartTime, ToState.State, TotalSeconds);
            }
        }
    }
}
