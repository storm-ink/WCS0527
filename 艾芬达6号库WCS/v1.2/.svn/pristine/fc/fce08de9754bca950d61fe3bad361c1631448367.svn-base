using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
namespace Client.RailGuidedVehicle
{
    public class RailGuidedVehicleDiagnosisDataRepository : Spiral.NhRepository<RailGuidedVehicleDiagnosisData, Int32>
    {
        public RailGuidedVehicleDiagnosisDataRepository(NhRepositoryContext context)
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
