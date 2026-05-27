using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class ConveyorRepository<TAggregateRoot>:Spiral.NhRepository<TAggregateRoot,Int32>
        where TAggregateRoot : ConveyorDiagnosisData, Spiral.IAggregateRoot<Int32>
    {
        public ConveyorRepository(Spiral.NhRepositoryContext context):base(context)
        {
            
        }

        public Int32 GetTransmissionDataId()
        {
            var v = (int?)this.Query().OrderByDescending(x => x.Id).Select(x => x.LogId).FirstOrDefault();

            return v.GetValueOrDefault(0) + 1;
        }
    }
}
