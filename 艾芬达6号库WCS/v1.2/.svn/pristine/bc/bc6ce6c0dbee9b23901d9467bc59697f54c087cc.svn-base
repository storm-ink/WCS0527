using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.RailGuidedVehicle
{
    /// <summary>
    /// 状态变化，表示从状态A变化到状态B的过程
    /// </summary>
    public class ChangeOfState:Spiral.IAggregateRoot<Int32>
    {
        public ChangeOfState()
        {
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Id
        /// </summary>
        public virtual Int32 Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// 状态A关联Id
        /// </summary>
        public virtual Int32 StateAId { get; set; }
        /// <summary>
        /// 状态B关联Id
        /// </summary>
        public virtual Int32 StateBId { get; set; }
        /// <summary>
        /// 状态A
        /// </summary>
        public virtual RailGuidedVehicleStatus StateA { get; set; }
        /// <summary>
        /// 状态B
        /// </summary>
        public virtual RailGuidedVehicleStatus StateB { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }
        /// <summary>
        /// 持续总时间毫秒数
        /// </summary>
        public virtual Int32 TotalMilliseconds { get; set; }
        /// <summary>
        /// 是否处于运行状态
        /// </summary>
        public virtual Boolean IsRunning { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        public virtual Int32 Version { get; protected set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedAt { get; set; }
    }
}
