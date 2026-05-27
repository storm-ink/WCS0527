using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using Client.Conveyor;

namespace Client.Conveyor
{
    public class AlarmWarningSpecTranslator : ISpecificationTranslator<AlarmWarning, Int32>
    {
        public IQueryable<AlarmWarning> ApplyWhere(IQueryable<AlarmWarning> q, ISpecification spec)
        {
            var specification = (AlarmWarningSpec)spec;
            if (specification.NameToMatch != null)
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }
            
            if (specification.WarningNameToMath != null)
            {
                q = q.Where(x => x.WarningName.Contains(specification.WarningNameToMath));
            }

            if (specification.PosNo != null)
            {
                q = q.Where(x => x.PosNo == specification.PosNo);
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