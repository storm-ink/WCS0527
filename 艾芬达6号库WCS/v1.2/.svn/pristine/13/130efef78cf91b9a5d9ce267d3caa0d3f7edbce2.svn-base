using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public abstract class ConveyorDiagnosisData
    {
        public virtual int Id { get; set; }
        public virtual String Name { get; set; }
        public virtual int Version { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        public virtual int LogId { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual DateTime LongDate { get; set; }
        public ConveyorDiagnosisData()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
