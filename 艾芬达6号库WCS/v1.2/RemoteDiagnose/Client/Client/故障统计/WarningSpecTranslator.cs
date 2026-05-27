using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client
{
    public class WarningSpecTranslator : ISpecificationTranslator<Warning, Int32>
    {
        public IQueryable<Warning> ApplyWhere(IQueryable<Warning> q, ISpecification spec)
        {
            var specification = (WarningSpec)spec;

            q = q.Where(x => x.EndTime != null);

            if (specification.DateMin != null)
            {
                q = q.Where(x => x.StartTime >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.StartTime <= specification.DateMax);
            }
            if (specification.Level != null)
            {
                q = q.Where(x => x.Level == specification.Level);
            }
            if (specification.ErrorCode != null)
            {
                q = q.Where(x => x.ErrorCode == specification.ErrorCode.Value);
            }
            if (specification.DeviceType != null)
            {
                q = q.Where(x => x.DeviceType == specification.DeviceType);
            }
            if (specification.NameToMatch != null)
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }
            if (!string.IsNullOrWhiteSpace(specification.ErrorNameToMatch))
            {
                q = q.Where(x => x.AlarmName.Contains(specification.ErrorNameToMatch));
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