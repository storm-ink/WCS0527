using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class LocationTask : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public LocationTask()
            : base()
        {
        }
        [DisplayName("货位号")]
        public virtual Int32 PosNo { get; set; }
        [DisplayName("任务号")]
        public virtual Int32 TaskNo { get; set; }
        [DisplayName("托盘号")]
        public virtual Int32 TUID { get; set; }
        [DisplayName("X轴可以接收")]
        public virtual Boolean Str_Rcv_X { get; set; }
        [DisplayName("X轴接收完成")]
        public virtual Boolean Fnh_Rcv_X { get; set; }
        [DisplayName("申请传递")]
        public virtual Boolean Rqs_Snt { get; set; }
        [DisplayName("准备好")]
        public virtual Boolean Rcv_Rdy { get; set; }
        [DisplayName("Y轴可以接收")]
        public virtual Boolean Str_Rcv_Y { get; set; }
        [DisplayName("Y轴接收完成")]
        public virtual Boolean Fnh_Rcv_Y { get; set; }
    }
}
