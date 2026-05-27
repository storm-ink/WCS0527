using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
namespace Client.Logs
{
    public class WcsLogSpecTranslator:ISpecificationTranslator<WcsLogDiagnosisData,Int32>
    {
        public IQueryable<WcsLogDiagnosisData> ApplyWhere(IQueryable<WcsLogDiagnosisData> q, ISpecification spec)
        {
            var specification = (WcsLogSpec)spec;

            if (specification.IdMin != null)
            {
                q = q.Where(x => x.LogId >= specification.IdMin);
            }
            if (specification.IdMax != null)
            {
                q = q.Where(x => x.LogId <= specification.IdMax);
            }
            if (specification.Level != null && specification.Level.Length > 0)
            {
                q = q.Where(x => specification.Level.Contains(x.Level));
            }
            if (!string.IsNullOrEmpty(specification.LoggerToMatch))
            {
                q = q.Where(x => x.Logger.Contains(specification.LoggerToMatch));
            }
            if (!string.IsNullOrEmpty(specification.SenderToMatch))
            {
                q = q.Where(x => x.Sender == specification.SenderToMatch);
            }
            if (!string.IsNullOrEmpty(specification.UserNameToMatch))
            {
                q = q.Where(x => x.UserName.Contains(specification.UserNameToMatch));
            }
            if (specification.DateMin != null)
            {
                q = q.Where(x => x.LongDate >= specification.DateMin);
            }
            if (specification.DateMax != null)
            {
                q = q.Where(x => x.LongDate <= specification.DateMax);
            }
            if (specification.ThreadId != null)
            {
                q = q.Where(x => x.ThreadId == specification.ThreadId.Value);
            }
            if (!string.IsNullOrEmpty(specification.WordToMatch))
            {
                q = q.Where(x => x.Message.Contains(specification.WordToMatch) || x.Exception.Contains(specification.WordToMatch));
            }
            if (specification.ExcludedLoggers != null && specification.ExcludedLoggers.Length > 0)
            {
                foreach (var item in specification.ExcludedLoggers)
                {
                    q = q.Where(x => x.Logger != item);
                }
            }

            if (specification.TaskCode != null)
            {
                q = q.Where(x => x.TaskCode == specification.TaskCode);
            }
            if (specification.EquipmentTaskId != null)
            {
                q = q.Where(x => x.EquipmentTaskId == specification.EquipmentTaskId);
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