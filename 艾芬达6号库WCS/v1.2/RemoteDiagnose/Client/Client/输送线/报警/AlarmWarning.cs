using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AlarmWarning : Spiral.IAggregateRoot<Int32>
    {
        public virtual Int32 Id { get; set; }
        /// <summary>
        /// 堆垛机名称
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// 货位号
        /// </summary>
        public virtual Int32 PosNo { get; set; }
        /// <summary>
        /// 报警名称
        /// </summary>
        public virtual String WarningName { get; set; }
        /// <summary>
        /// 状态A Id
        /// </summary>
        public virtual Int32 StateAId { get; set; }
        /// <summary>
        /// 状态B Id
        /// </summary>
        public virtual Int32? StateBId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }
        /// <summary>
        /// 持续总时长（毫秒数）
        /// </summary>
        public virtual Int32 TotalMilliseconds { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        public virtual Int32 Version { get; protected set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }
    }
}
