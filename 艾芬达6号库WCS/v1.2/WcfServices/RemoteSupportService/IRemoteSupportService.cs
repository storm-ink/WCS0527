using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupportService
{
    public interface IRemoteSupportService
    {
        /// <summary>
        /// 获取指定时间段的日志数据
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="limit">返回的记录数最大值（为空时返回查询的时候段内所有日志）</param>
        /// <returns></returns>
        public CallResult GetLogs(DateTime startTime, DateTime endTime,Int32? limit);

    }
}
