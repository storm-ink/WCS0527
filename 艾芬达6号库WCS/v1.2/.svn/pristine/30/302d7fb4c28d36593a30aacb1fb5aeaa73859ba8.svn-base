using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.SingleForkCrane
{
    public class StatusDiagnosisDataRepository : Spiral.NhRepository<StatusDiagnosisData, Int32>
    {
        public StatusDiagnosisDataRepository(NhRepositoryContext context)
            : base(context)
        {
        }


        public Int32 GetTransmissionDataId()
        {
            var v = (int?)this.Query().OrderByDescending(x => x.Id).Select(x => x.LogId).FirstOrDefault();

            return v.GetValueOrDefault(0) + 1;
        }
    }
}
