using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.DataAnalysis
{
    public interface IChartDataAdapter
    {
        /// <summary>
        /// 查找状态时间
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="from">查询的时间范围下限</param>
        /// <param name="to">查询的时间范围上限</param>
        /// <returns></returns>
        IEnumerable<StateTime> FindStateTimes(string deviceName, DateTime? from, DateTime? to);

        /// <summary>
        /// 查找状态持续时间
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="from">查询的时间范围下限</param>
        /// <param name="to">查询的时间范围上限</param>
        /// <returns></returns>
        IEnumerable<StateExecutionTime> FindStateExecutionTimes(string deviceName, DateTime? from, DateTime? to);

        /// <summary>
        /// 查找错误时间
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="from">查询的时间范围下限</param>
        /// <param name="to">查询的时间范围上限</param>
        /// <returns></returns>
        IEnumerable<ErrorTime> FindErrorTimes(string deviceName, DateTime? from, DateTime? to);

        /// <summary>
        /// 查找错误持续时间
        /// </summary>
        /// <param name="deviceName">设备名称</param>
        /// <param name="from">查询的时间范围下限</param>
        /// <param name="to">查询的时间范围上限</param>
        /// <returns></returns>
        IEnumerable<ErrorExecutionTime> FindErrorExecutionTimes(string deviceName, DateTime? from, DateTime? to);

        IEnumerable<TaskTime> FindTaskTimes(string deviceName, DateTime? from, DateTime? to);

        IEnumerable<TaskExecutionTime> FindTaskExecutionTimes(string deviceName, DateTime? from, DateTime? to);

    }
}
