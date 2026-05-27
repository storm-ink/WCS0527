using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.Conveyor
{
    public class TaskSpecTranslator : ISpecificationTranslator<Task, Int32>
    {
        public IQueryable<Task> ApplyWhere(IQueryable<Task> q, ISpecification spec)
        {
            var specification = (TaskSpec)spec;
            if (specification.AssignmentID != null)
            {
                q = q.Where(x => x.AssignmentID == specification.AssignmentID.Value);
            } 
            if (specification.HandShake != null)
            {
                q = q.Where(x => x.HandShake == specification.HandShake.Value);
            }

            if (specification.Status != null)
            {
                q = q.Where(x => x.Status == specification.Status.Value);
            }

            if (specification.TU_ID != null)
            {
                q = q.Where(x => x.TU_ID == specification.TU_ID.Value);
            }
            if (specification.TU_Type != null)
            {
                q = q.Where(x => x.TU_Type == specification.TU_Type.Value);
            }
            if (specification.IO_Data != null)
            {
                q = q.Where(x => x.IO_Data == specification.IO_Data.Value);
            }
            if (specification.RotingNo != null)
            {
                q = q.Where(x => x.RotingNo == specification.RotingNo.Value);
            }
            if (specification.StartMotorNo != null)
            {
                q = q.Where(x => x.StartMotorNo == specification.StartMotorNo.Value);
            }
            if (specification.DestinationNo != null)
            {
                q = q.Where(x => x.DestinationNo == specification.DestinationNo.Value);
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