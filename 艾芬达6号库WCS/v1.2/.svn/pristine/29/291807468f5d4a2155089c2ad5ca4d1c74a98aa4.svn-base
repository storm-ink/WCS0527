using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Conveyor
{
    public class AppearanceInspectionSpecTranslator:Spiral.ISpecificationTranslator<AppearanceInspection,Int32>
    {
        public IQueryable<AppearanceInspection> ApplyWhere(IQueryable<AppearanceInspection> q, Spiral.ISpecification spec)
        {
            var specification = (AppearanceInspectionSpec)spec;
            if (specification.NameToMatch != null)
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }

            if (specification.No != null)
            {
                q = q.Where(x => x.No == specification.No);
            }
            if (specification.Back_Over != null)
            {
                q = q.Where(x => x.Back_Over == specification.Back_Over);
            }
            if (specification.Front_Over != null)
            {
                q = q.Where(x => x.Front_Over == specification.Front_Over);
            }
            if (specification.Left_Over != null)
            {
                q = q.Where(x => x.Left_Over == specification.Left_Over);
            }
            if (specification.Result != null)
            {
                q = q.Where(x => x.Result == specification.Result);
            }
            if (specification.Right_Over != null)
            {
                q = q.Where(x => x.Right_Over == specification.Right_Over);
            }
            if (specification.Too_High != null)
            {
                q = q.Where(x => x.Too_High == specification.Too_High);
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
