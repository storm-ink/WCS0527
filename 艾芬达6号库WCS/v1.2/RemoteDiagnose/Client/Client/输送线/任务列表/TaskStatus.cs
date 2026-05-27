using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public enum TaskStatus
    {
        /// <summary>
        /// 初始化时数据
        /// </summary>
        初始化 = 0,
        /// <summary>
        /// 完成
        /// </summary>
        已完成 = 1,
        /// <summary>
        /// 执行中
        /// </summary>
        执行中 = 2,
        /// <summary>
        /// 等待执行
        /// </summary>
        等待执行 = 3,
        /// <summary>
        /// 任务不能执行
        /// </summary>
        发生错误 = 4,
        /// <summary>
        /// 其它 未完成（原因或状态定义）；
        /// </summary>
        未完成 = 5
    }
}
