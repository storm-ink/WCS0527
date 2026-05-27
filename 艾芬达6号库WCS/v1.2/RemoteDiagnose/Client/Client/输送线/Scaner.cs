using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class Scaner : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public Scaner():base()
        {
           
        }

        public virtual Int32 No { get; set; }
        public virtual ScaneResult Result { get; set; }
        public virtual Int32 Readed { get; set; }
    }
}
