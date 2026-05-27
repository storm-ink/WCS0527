using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class Occupy : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public Occupy():base()
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
        /// 光电的使用状态，即此组使用了哪些光电
        /// </summary>
        [System.ComponentModel.DisplayName("光电的使用状态")]
        public virtual Int32 PhocllUseStatus { get; set; }
        /// <summary>
        /// 指示前保护光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("前保护光电")]
        public virtual Boolean FroProPotocell { get; set; }
        /// <summary>
        /// 指示前到位光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("前到位")]
        public virtual Boolean FroPosPotocell { get; set; }
        /// <summary>
        /// 指示前减速光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("前减速")]
        public virtual Boolean FroSloPotocell { get; set; }
        /// <summary>
        /// 指示后保护光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("后保护")]
        public virtual Boolean AftProPotocell { get; set; }
        /// <summary>
        /// 指示后到位光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("后到位")]
        public virtual Boolean AftPosPotocell { get; set; }
        /// <summary>
        /// 指示后减速光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("后减速")]
        public virtual Boolean AftSloPotocell { get; set; }
        /// <summary>
        /// 指示后高位光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("后高位")]
        public virtual Boolean UpPotocell { get; set; }
        /// <summary>
        /// 指示后低位光电是否被遮挡
        /// </summary>
        [System.ComponentModel.DisplayName("后低位")]
        public virtual Boolean DownPotocell { get; set; }
    }
}
