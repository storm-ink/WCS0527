using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using Client.Conveyor;

namespace Client.Conveyor
{
    public class AlarmSpecTranslator : ISpecificationTranslator<Alarm, Int32>
    {
        public IQueryable<Alarm> ApplyWhere(IQueryable<Alarm> q, ISpecification spec)
        {
            var specification = (AlarmSpec)spec;
            if (specification.NameToMatch != null)
            {
                q = q.Where(x => x.Name.Contains(specification.NameToMatch));
            }
            if (specification.Breaker != null)
            {
                q = q.Where(x => x.Breaker == specification.Breaker.Value);
            }
            if (specification.Isolator != null)
            {
                q = q.Where(x => x.Isolator == specification.Isolator.Value);
            }
            if (specification.Lift_MotorBraker != null)
            {
                q = q.Where(x => x.Lift_MotorBraker == specification.Lift_MotorBraker.Value);
            }
            if (specification.Lift_MotorContactor != null)
            {
                q = q.Where(x => x.Lift_MotorContactor == specification.Lift_MotorContactor.Value);
            }
            if (specification.Manual != null)
            {
                q = q.Where(x => x.Manual == specification.Manual.Value);
            }
            if (specification.OccupyOvertime != null)
            {
                q = q.Where(x => x.OccupyOvertime == specification.OccupyOvertime.Value);
            }
            if (specification.Photocell != null)
            {
                q = q.Where(x => x.Photocell == specification.Photocell.Value);
            }
            if (specification.PosNo != null)
            {
                q = q.Where(x => x.PosNo == specification.PosNo.Value);
            }
            if (specification.RunOvertime != null)
            {
                q = q.Where(x => x.RunOvertime == specification.RunOvertime.Value);
            }
            if (specification.TaskNoGoods != null)
            {
                q = q.Where(x => x.TaskNoGoods == specification.TaskNoGoods.Value);
            }
            if (specification.X_MotorBraker != null)
            {
                q = q.Where(x => x.X_MotorBraker == specification.X_MotorBraker.Value);
            }
            if (specification.X_MotorContactor != null)
            {
                q = q.Where(x => x.X_MotorContactor == specification.X_MotorContactor.Value);
            }
            if (specification.X_MotorVAF != null)
            {
                q = q.Where(x => x.X_MotorVAF == specification.X_MotorVAF.Value);
            }
            if (specification.Y_MotorBraker != null)
            {
                q = q.Where(x => x.Y_MotorBraker == specification.Y_MotorBraker.Value);
            }
            if (specification.Y_MotorContactor != null)
            {
                q = q.Where(x => x.Y_MotorContactor == specification.Y_MotorContactor.Value);
            }
            if (specification.Y_MotorVAF != null)
            {
                q = q.Where(x => x.Y_MotorVAF == specification.Y_MotorVAF.Value);
            }

            if (specification.IsWarn != null)
            {
                if (specification.IsWarn == true)
                {
                    q = q.Where(x => x.Breaker
                        || x.Isolator
                        || x.Lift_MotorBraker
                        || x.Lift_MotorContactor
                        || x.Manual
                        || x.OccupyOvertime
                        || x.Photocell
                        || x.RunOvertime
                        || x.TaskNoGoods
                        || x.X_MotorBraker
                        || x.X_MotorContactor
                        || x.X_MotorVAF
                        || x.Y_MotorBraker
                        || x.Y_MotorContactor
                        || x.Y_MotorVAF);
                }
                else
                {
                    q = q.Where(x => !x.Breaker
                           && !x.Isolator
                           && !x.Lift_MotorBraker
                           && !x.Lift_MotorContactor
                           && !x.Manual
                           && !x.OccupyOvertime
                           && !x.Photocell
                           && !x.RunOvertime
                           && !x.TaskNoGoods
                           && !x.X_MotorBraker
                           && !x.X_MotorContactor
                           && !x.X_MotorVAF
                           && !x.Y_MotorBraker
                           && !x.Y_MotorContactor
                           && !x.Y_MotorVAF);
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