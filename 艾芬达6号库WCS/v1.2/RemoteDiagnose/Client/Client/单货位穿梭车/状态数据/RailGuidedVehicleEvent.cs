using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.RailGuidedVehicle
{
    /// <summary>
    /// 穿梭车事件
    /// </summary>
    [DataContract]
    public enum RailGuidedVehicleEvent
    {
        /// <summary>无货无任务</summary>
        [Description("无货无任务")]
        [EnumMember]
        无货无任务 = 0,

        /// <summary>接到任务未运行</summary>
        [Description("接到任务未运行")]
        [EnumMember]
        接到任务未运行 = 1,

        /// <summary>行走运行</summary>
        [Description("行走运行")]
        [EnumMember]
        行走运行 = 2,

        /// <summary>到达起始点</summary>
        [Description("到达起始点")]
        [EnumMember]
        到达起始点 = 3,


        /// <summary>到达目的地</summary>
        [Description("到达目的地")]
        [EnumMember]
        到达目的地 = 4,

        /// <summary>执行让道任务</summary>
        [Description("执行让道任务")]
        [EnumMember]
        执行让道任务 = 5,

        /// <summary>
        /// 自动任务完成
        /// </summary>
        [Description("自动任务完成")]
        [EnumMember]
        自动任务完成 = 6,

        /// <summary>
        /// 手动报完成
        /// </summary>
        [Description("手动报完成")]
        [EnumMember]
        手动报完成 = 7,

        /// <summary>
        /// 交接货超时
        /// </summary>
        [Description("交接货超时")]
        [EnumMember]
        交接货超时=8,
    }
}
