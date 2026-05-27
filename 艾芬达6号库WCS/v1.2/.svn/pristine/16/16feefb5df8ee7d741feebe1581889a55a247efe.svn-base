using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Client.RailGuidedVehicle
{

    /// <summary>
    /// 任务模式
    /// </summary>
    [DataContract]
    public enum RailGuidedVehicleTaskMode
    {
        /// <summary>无任务类型</summary>
        [Description("无任务类型")]
        [EnumMember]
        无任务类型 = 0,

        /// <summary>表示HB任务
        /// 全自动任务
        /// </summary>
        [Description("全自动任务")]
        [Obsolete("穿梭车已改为上位机拆分任务，该任务类型已取消。")]
        [EnumMember]
        全自动任务 = 1,

        /// <summary>取货任务</summary>
        [Description("取货任务")]
        [EnumMember]
        取货任务 = 2,

        /// <summary>放货任务</summary>
        [Description("放货任务")]
        [EnumMember]
        放货任务 = 3,


        /// <summary>有货行走</summary>
        [Description("有货行走")]
        [EnumMember]
        有货行走 = 4,

        /// <summary>无货行走</summary>
        [Description("无货行走")]
        [EnumMember]
        无货行走 = 5,

    }
}
