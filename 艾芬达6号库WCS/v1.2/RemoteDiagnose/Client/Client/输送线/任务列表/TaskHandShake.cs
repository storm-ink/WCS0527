using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public enum TaskHandShake
    {
        /// <summary>
        /// 初始化时数据
        /// </summary>
        初始化 = 0,
        /// <summary>
        /// 新任务
        /// </summary>
        新任务 = 1,
        /// <summary>
        /// 删除请求
        /// </summary>
        请求删除 = 2,
        /// <summary>
        /// PLC已请求
        /// </summary>
        设备已读 = 3,
        /// <summary>
        /// WCS 二次确认
        /// </summary>
        握手完成 = 4,
        /// <summary>
        /// 请求清空
        /// </summary>
        请求清空 = 5,
    }
}
