using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AlarmSpec:Spiral.ISpecification
    {
        public AlarmSpec()
        {
        }
        public virtual String NameToMatch { get; set; }
        [DisplayName("货位号")]
        public virtual Int32? PosNo { get; set; }
        [DisplayName("是否手动")]
        public virtual Boolean? Manual { get; set; }
        [DisplayName("离线（隔离开关断开）")]
        public virtual Boolean? Isolator { get; set; }
        [DisplayName("电路保护器断开（断路器断开）")]
        public virtual Boolean? Breaker { get; set; }
        [DisplayName("光电异常")]
        public virtual Boolean? Photocell { get; set; }
        [DisplayName("运行超时")]
        public virtual Boolean? RunOvertime { get; set; }
        [DisplayName("占位超时")]
        public virtual Boolean? OccupyOvertime { get; set; }
        [DisplayName("有任务无货")]
        public virtual Boolean? TaskNoGoods { get; set; }
        [DisplayName("X轴电机变频器故障")]
        public virtual Boolean? X_MotorVAF { get; set; }
        [DisplayName("Y轴电机变频器故障")]
        public virtual Boolean? Y_MotorVAF { get; set; }
        [DisplayName("X轴电机正反转接触器故障")]
        public virtual Boolean? X_MotorContactor { get; set; }
        [DisplayName("X轴电机抱闸接触器故障")]
        public virtual Boolean? X_MotorBraker { get; set; }
        [DisplayName("Y轴电机正反转接触器故障")]
        public virtual Boolean? Y_MotorContactor { get; set; }
        [DisplayName("Y轴电机抱闸接触器故障")]
        public virtual Boolean? Y_MotorBraker { get; set; }
        [DisplayName("顶升电机正反转接触器故障")]
        public virtual Boolean? Lift_MotorContactor { get; set; }
        [DisplayName("顶升电机抱闸接触器故障")]
        public virtual Boolean? Lift_MotorBraker { get; set; }

        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public String Array { get; set; }
        public Boolean? IsWarn { get; set; }

        public string GetTranslatorTypeName()
        {
            return "Client.Conveyor.AlarmSpecTranslator, Client";
        }

        public void Trim()
        {
            this.NameToMatch = Spiral.Utils.StringUtil.WhiteSpaceToNull(this.NameToMatch);
        }
    }
}
