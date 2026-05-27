using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis
{
    /// <summary>
    /// 任务时间
    /// </summary>
    public class TaskTime
    {
        /// <summary>
        /// 任务号
        /// </summary>
        public String TaskId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public String State { get; set; }
        /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
