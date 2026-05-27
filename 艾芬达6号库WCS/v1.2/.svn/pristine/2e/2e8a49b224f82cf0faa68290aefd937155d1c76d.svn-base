using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    /// <summary>
    /// 外型检测
    /// </summary>
    public class AppearanceInspection : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public AppearanceInspection():base()
        {
        }

        public virtual Int32 No { get; set; }
        public virtual AppearanceInspectionResult Result{get;set;}
        public virtual Boolean Left_Over{get;set;}
        public virtual Boolean Right_Over{get;set;}
        public virtual Boolean Front_Over{get;set;}
        public virtual Boolean Back_Over{get;set;}
        public virtual Boolean Too_High{get;set;}
    }
}
