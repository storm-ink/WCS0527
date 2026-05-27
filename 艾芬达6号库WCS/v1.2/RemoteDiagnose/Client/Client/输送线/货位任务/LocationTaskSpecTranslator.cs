using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.Conveyor
{
    public class LocationTaskSpecTranslator : ISpecificationTranslator<LocationTask, Int32>
    {
        public IQueryable<LocationTask> ApplyWhere(IQueryable<LocationTask> q, ISpecification spec)
        {
            var specification = (LocationTaskSpec)spec;
            if (specification.PosNo != null)
            {
                q = q.Where(x => x.PosNo == specification.PosNo.Value);
            }
            if (specification.TUID != null)
            {
                q = q.Where(x => x.TUID == specification.TUID.Value);
            }
            if (specification.TaskNo != null)
            {
                q = q.Where(x => x.TaskNo == specification.TaskNo.Value);
            }

            if (!string.IsNullOrWhiteSpace(specification.NameToMatch))
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }

            if (specification.DateMin != null)
            {
                q = q.Where(x => x.LongDate >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.LongDate <= specification.DateMax);
            }
            if (specification.Array == "升序")
            {
                q = q.OrderBy(x => x.LongDate);
            }
            if (specification.Array == "降序")
            {
                q = q.OrderByDescending(x => x.LongDate);
            }

            return q;
        }
    }
}