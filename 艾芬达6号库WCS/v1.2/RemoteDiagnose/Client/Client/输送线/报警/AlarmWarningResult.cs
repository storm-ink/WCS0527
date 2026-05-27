using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class CreateAlarmWarningResult
    {
        public Boolean Finished { get; set; }
        public Int32 CreatedRecords { get; set; }
        public DateTime? LastTime { get; set; }
    }
}
