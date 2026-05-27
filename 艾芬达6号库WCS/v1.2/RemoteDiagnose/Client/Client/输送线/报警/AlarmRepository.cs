using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AlarmRepository:ConveyorRepository<Alarm>
    {
        public AlarmRepository(Spiral.NhRepositoryContext context)
            : base(context)
        {
            
        }

    }
}
