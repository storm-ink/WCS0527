using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.RailGuidedVehicle
{
    public class RailGuidedVehicleDiagnosisDataSpecTranslator : ISpecificationTranslator<RailGuidedVehicleDiagnosisData, Int32>
    {
        public IQueryable<RailGuidedVehicleDiagnosisData> ApplyWhere(IQueryable<RailGuidedVehicleDiagnosisData> q, ISpecification spec)
        {
            var specification = (RailGuidedVehicleDiagnosisDataSpec)spec;

            if (specification.ErrorCode != null)
            {
                q = q.Where(x => x.ErrorCode == specification.ErrorCode.Value);
            }
            if (specification.Event != null)
            {
                q = q.Where(x => x.Event == specification.Event.Value);
            }

            if (!string.IsNullOrWhiteSpace(specification.NameToMatch))
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }
            if (specification.Status != null)
            {
                q = q.Where(x => x.State == specification.Status.Value);
            }

            if (specification.DateMin != null)
            {
                q = q.Where(x => x.LongDate >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.LongDate <= specification.DateMax);
            }
            if (specification.IsWarn != null)
            {
                if (specification.IsWarn == true)
                {
                    q = q.Where(x => x.ErrorCode != 0);
                }
                else
                {
                    q = q.Where(x => x.ErrorCode == 0);
                }
            }
            if (specification.Array == "升序")
            {
                q = q.OrderBy(x => x.LongDate).ThenBy(x=>x.LogId);
            }
            if (specification.Array == "降序")
            {
                q = q.OrderByDescending(x => x.LongDate).ThenByDescending(x=>x.LogId);
            }

            return q;
        }
    }
}