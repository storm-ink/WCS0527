using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis
{
    /// <summary>
    /// 状态时间
    /// </summary>
    public class StateTime
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public String DeviceName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String State { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public String StateDisplayName { get; set; }
        /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
