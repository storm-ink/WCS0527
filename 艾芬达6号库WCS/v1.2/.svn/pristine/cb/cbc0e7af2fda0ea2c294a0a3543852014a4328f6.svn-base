using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Logs
{
    public class WcsLogDiagnosisDataRepository:Spiral.NhRepository<WcsLogDiagnosisData,Int32>
    {
        public WcsLogDiagnosisDataRepository(NhRepositoryContext context)
            : base(context)
        {

        }

        public Int32 GetTransmissionDataId()
        {
            var v = (int?)this.Query().OrderByDescending(x => x.Id).Select(x => x.LogId).FirstOrDefault();

            return v.GetValueOrDefault(0)+1;
        }
    }
}
