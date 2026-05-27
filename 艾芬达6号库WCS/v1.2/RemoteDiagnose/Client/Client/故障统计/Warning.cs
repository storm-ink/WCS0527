using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Client
{
    public class Warning : Spiral.IAggregateRoot<Int32>
    {
        [IgnoreExport]
        public virtual Int32 Id { get; set; }
        /// <summary>
        /// 堆垛机名称
        /// </summary>
        [Display(Name="设备名称")]
        public virtual String Name { get; set; }
        /// <summary>
        /// 状态A Id
        /// </summary>
        [Display(Name = "状态A")]
        [IgnoreExport]
        public virtual Int32 StateAId { get; set; }
        /// <summary>
        /// 状态B Id
        /// </summary>
        [Display(Name = "状态B")]
        [IgnoreExport]
        public virtual Int32? StateBId { get; set; }
        /// <summary>
        /// 当前任务号
        /// </summary>
        [Display(Name = "任务号")]
        public virtual String TaskId { get; set; }
        /// <summary>
        /// 错误码（默认为 0）
        /// </summary>
        [Display(Name = "错误码")]
        public virtual Int32 ErrorCode { get; set; }
        /// <summary>
        /// 故障名称
        /// </summary>
        [Display(Name = "故障名称")]
        public virtual String AlarmName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        public virtual DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        public virtual DateTime EndTime { get; set; }
        /// <summary>
        /// 持续总时长（毫秒数）
        /// </summary>
        [Display(Name = "持续时间")]
        public virtual Int32 TotalMilliseconds { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        [Display(Name = "创建时间")]
        public virtual DateTime CreatedAt { get; set; }

        [IgnoreExport]
        public virtual Int32 Version { get; protected set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [IgnoreExport]
        public virtual DateTime UpdatedAt { get; set; }
        /// <summary>
        /// 数据源类型
        /// </summary>
        [Display(Name = "数据源")]
        [IgnoreExport]
        public virtual String SourceType { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        [Display(Name = "设备类型")]
        public virtual String DeviceType { get; set; }
        /// <summary>
        /// 报警等级
        /// </summary>
        [Display(Name = "等级")]
        public virtual WarningLevel Level { get; set; }

        /// <summary>
        /// 批数据结束日期（用于在第二次更新的时候定位数据行位置）
        /// </summary>
        [Display(Name = "批时间")]
        [IgnoreExport]
        public virtual DateTime BatchDate { get; set; }
    }
}
