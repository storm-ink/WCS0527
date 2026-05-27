using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.Conveyor
{
    public class OccupySpecTranslator : ISpecificationTranslator<Occupy, Int32>
    {
        public IQueryable<Occupy> ApplyWhere(IQueryable<Occupy> q, ISpecification spec)
        {
            var specification = (OccupySpec)spec;
            if (specification.PosNo != null)
            {
                q = q.Where(x => x.PosNo == specification.PosNo.Value);
            }

            if (!string.IsNullOrWhiteSpace(specification.NameToMatch))
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }

            if (specification.HaveSignal != null)
            {
                if (specification.HaveSignal == true)
                {
                    q = q.Where(x => x.AftPosPotocell
                        || x.AftProPotocell
                        || x.AftSloPotocell
                        || x.DownPotocell
                        || x.FroPosPotocell
                        || x.FroProPotocell
                        || x.FroSloPotocell
                        || x.UpPotocell
                        );
                }
                else
                {
                    q = q.Where(x => !x.AftPosPotocell
                       && !x.AftProPotocell
                       && !x.AftSloPotocell
                       && !x.DownPotocell
                       && !x.FroPosPotocell
                       && !x.FroProPotocell
                       && !x.FroSloPotocell
                       && !x.UpPotocell
                       );
                }
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