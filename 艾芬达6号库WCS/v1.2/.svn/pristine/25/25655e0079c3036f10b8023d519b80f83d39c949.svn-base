using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.RailGuidedVehicle
{
    public class ChangeOfStateSpecTranslator : ISpecificationTranslator<ChangeOfState, Int32>
    {
        public IQueryable<ChangeOfState> ApplyWhere(IQueryable<ChangeOfState> q, ISpecification spec)
        {
            var specification = (ChangeOfStatesSpec)spec;

            if (specification.StateA != null)
            {
                q = q.Where(x => x.StateA == specification.StateA.Value);
            }
            if (specification.StateB != null)
            {
                q = q.Where(x => x.StateB == specification.StateB.Value);
            }
            if (specification.IsRunning != null)
            {
                q = q.Where(x => x.IsRunning == specification.IsRunning.Value);
            }

            if (!string.IsNullOrWhiteSpace(specification.NameToMatch))
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }

            if (specification.DateMin != null)
            {
                q = q.Where(x => x.StartTime >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.StartTime <= specification.DateMax);
            }
            if (specification.Array == "升序")
            {
                q = q.OrderBy(x => x.StartTime);
            }
            if (specification.Array == "降序")
            {
                q = q.OrderByDescending(x => x.StartTime);
            }

            return q;
        }
    }
}