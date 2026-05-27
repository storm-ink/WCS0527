using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class Location : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public Location()
            : base()
        {
        }
        /// <summary>
        /// 货位号.
        /// </summary>
        /// <value>
        /// 位置在设备中的编码形式.
        /// </value>
        [Description("货位号")]
        public virtual Int32 PosNo { get; set; }
        /// <summary>
        /// 状态.
        /// </summary>
        [Description("状态")]
        public virtual LocationStatus Status { get; set; }

        public virtual bool IsRunning()
        {
            return this.Status == LocationStatus.运行;
        }
    }
}
