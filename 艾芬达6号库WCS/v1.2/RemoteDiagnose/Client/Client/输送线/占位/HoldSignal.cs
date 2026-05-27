using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class HoldSignal : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public HoldSignal():base()
        {
            
        }
        /// <summary>
        /// 获取或设置占位信号对应的货位号（信号源）
        /// </summary>
        /// <value>
        /// 货位对应的设备编码形式.即输送线位置对象 DeviceCode 值.
        /// </value>
        [DisplayName("货位号")]
        public virtual Int32 PosNo { get; set; }

        /// <summary>
        ///  获取或设置占位信号的握手变量值
        /// </summary>
        /// <value>
        /// 占位信号握手变量
        /// </value>
        [DisplayName("握手")]
        public virtual HoldSignalHandShake HandShake { get; set; }
        /// <summary>
        /// 获取或设置占位信号对应的任务号
        /// </summary>
        [DisplayName("任务号")]
        public virtual Int32 AssignmentID { get; set; }
        /// <summary>
        /// 获取或设置占位信号对应的托盘号
        /// </summary>
        [DisplayName("托盘号")]
        public virtual Int32 TU_ID { get; set; }
        /// <summary>
        /// 获取或设置占位信号对应的托盘类型
        /// </summary>
        [DisplayName("托盘类型")]
        public virtual Int32 TU_Type { get; set; }
        /// <summary>
        /// 获取或设置占位信号对应的业务数据
        /// </summary>
        [DisplayName("业务数据")]
        public virtual Int32 IO_Data { get; set; }
    }
}
