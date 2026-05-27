using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.SingleForkCrane
{
    public class StatusDiagnosisDataSpecTranslator : ISpecificationTranslator<StatusDiagnosisData, Int32>
    {
        public IQueryable<StatusDiagnosisData> ApplyWhere(IQueryable<StatusDiagnosisData> q, ISpecification spec)
        {
            var specification = (StatusDiagnosisDataSpec)spec;

            if (specification.Column != null)
            {
                q = q.Where(x => x.Column == specification.Column.Value);
            }
            if (specification.ErrorCode != null && specification.ErrorCode!=0)
            {
                q = q.Where(x => x.ErrorCode == specification.ErrorCode.Value);
            }

            if (!string.IsNullOrWhiteSpace(specification.ErrorNameToMatch))
            {
                q = q.Where(x => x.Alarm.Name.Contains(specification.ErrorNameToMatch));
            }
            if (specification.Event != null)
            {
                q = q.Where(x => x.Event == specification.Event.Value);
            }
            if (specification.Level != null)
            {
                q = q.Where(x => x.Level == specification.Level.Value);
            }
            if (!string.IsNullOrWhiteSpace(specification.NameToMatch))
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }
            if (specification.Status != null)
            {
                q = q.Where(x => x.State == specification.Status.Value);
            }
            if (specification.TaskIdToMatch != null)
            {
                q = q.Where(x => x.TaskId.Contains(specification.TaskIdToMatch));
            }

            if (specification.DateMin != null)
            {
                q = q.Where(x => x.LongDate >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.LongDate <= specification.DateMax);
            }

            if(specification.HasErrorCode != null)
            {
                if (specification.HasErrorCode == false)
                {
                    q = q.Where(x => x.ErrorCode==0);
                }
                else
                {
                    q = q.Where(x => x.ErrorCode != 0);
                }
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
                q = q.OrderBy(x => x.Id);
            }
            if (specification.Array == "降序")
            {
                q = q.OrderByDescending(x => x.Id);
            }

            return q;
        }
    }
}