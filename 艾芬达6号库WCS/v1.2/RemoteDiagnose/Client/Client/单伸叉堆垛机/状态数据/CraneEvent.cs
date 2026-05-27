using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.SingleForkCrane
{
    /// <summary>
    /// 堆垛机事件
    /// </summary>
    [DataContract]
    public enum CraneEvent
    {
        /// <summary>已初始化</summary>
        [Description("初始化")]
        [EnumMember]
        初始化 = 0,

        /// <summary>正在运行</summary>
        [Description("开始运行")]
        [EnumMember]
        开始运行 = 1,

        /// <summary>正在取货</summary>
        [Description("开始取货")]
        [EnumMember]
        开始取货 = 2,

        /// <summary>取货完成</summary>
        [Description("取货完成")]
        [EnumMember]
        取货完成 = 3,

        /// <summary>正在放货</summary>
        [Description("开始放货")]
        [EnumMember]
        开始放货 = 4,

        /// <summary>放货完成</summary>
        [Description("放货完成")]
        [EnumMember]
        放货完成 = 5,

        /// <summary>完成</summary>
        [Description("完成")]
        [EnumMember]
        完成 = 6,

        /// <summary>急停</summary>
        [Description("急停")]
        [EnumMember]
        急停 = 7,

        /// <summary>出错完成</summary>
        [Description("出错完成")]
        [EnumMember]
        出错完成 = 8,

        /// <summary>回原点</summary>
        [Description("回原点")]
        [EnumMember]
        回原点 = 9
    }
}
