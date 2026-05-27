using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.SingleForkCrane
{
    /// <summary>
    /// 堆垛机状态
    /// </summary>
    [DataContract]
    public enum CraneStatus
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        [EnumMember]
        初始化 = 0,

        /// <summary>
        /// 回原点
        /// </summary>
        [Description("回原点")]
        [EnumMember]
        回原点 = 1,

        /// <summary>
        /// 无货待命
        /// </summary>
        [Description("无货待命")]
        [EnumMember]
        无货待命=2,

        /// <summary>
        /// 有货待命
        /// </summary>
        [Description("有货待命")]
        [EnumMember]
        有货待命=3,

        /// <summary>
        /// 无货运行
        /// </summary>
        [Description("无货运行")]
        [EnumMember]
        无货运行=4,

        /// <summary>
        /// 有货运行
        /// </summary>
        [Description("有货运行")]
        [EnumMember]
        有货运行=5,

        /// <summary>
        /// 取货
        /// </summary>
        [Description("取货")]
        [EnumMember]
        取货 = 6,

        /// <summary>
        /// 放货
        /// </summary>
        [Description("放货")]
        [EnumMember]
        放货 = 7,

        /// <summary>
        /// 报警停机
        /// </summary>
        [Description("报警停机")]
        [EnumMember]
        报警停机 = 8,

        /// <summary>
        /// 报警复位
        /// </summary>
        [Description("报警复位")]
        [EnumMember]
        报警复位 = 9,

        /// <summary>
        /// ???
        /// </summary>
        [Description("???")]
#warning 这个状态需要确认
        [EnumMember]
        备用=10,

        /// <summary>
        /// 未连接
        /// </summary>
        [Description("未连接")]
        [EnumMember]
        未连接 = 11,

        /// <summary>
        /// 手动操作
        /// </summary>
        [Description("手动操作")]
        [EnumMember]
        手动操作 = 12
    }
}
