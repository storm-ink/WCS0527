using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
namespace Client.SingleForkCrane
{
    public class ChangeOfStateRepository : Spiral.NhRepository<ChangeOfState, Int32>
    {
        public ChangeOfStateRepository(NhRepositoryContext context)
            : base(context)
        {
        }

        public CreateChangeOfStatusResult Create(String deviceName, Int32 batchSize, DateTime? startTime, DateTime? endTime)
        {
            Int32 result = 0;
            Boolean finished = false;

            var lastChangeOfState =this.Query()
                        .OrderByDescending(x => x.StartTime)
                        .FirstOrDefault();
            if (lastChangeOfState != null && lastChangeOfState.EndTime == null)
            {
                this.Remove(lastChangeOfState.Id);
            }

            lastChangeOfState =this.Query()
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            var q = Context.Session.Query<StatusDiagnosisData>();
            if (lastChangeOfState != null)
            {
                if (startTime == null || lastChangeOfState.EndTime > startTime.Value)
                {
                    q = q.Where(x => x.LongDate > lastChangeOfState.EndTime);
                }
                else if (startTime != null)
                {
                    q = q.Where(x => x.LongDate > startTime.Value);
                }
            }
            else
            {
                if (startTime != null)
                {
                    q = q.Where(x => x.LongDate > startTime.Value);
                }
            }
            if (!string.IsNullOrWhiteSpace(deviceName))
            {
                q = q.Where(x => x.Name == deviceName);
            }
            if (endTime != null)
            {
                q = q.Where(x => x.LongDate < endTime);
            }

            q = q.OrderBy(x=>x.LongDate).Take(batchSize);
            var list = q.ToList();

            startTime = list.OrderByDescending(x => x.LongDate).Select(x => x.LongDate).FirstOrDefault();

            finished = batchSize > list.Count;

            ChangeOfState state = null;
            foreach (var grouping in list.GroupBy(x => x.Name))
            {
                foreach (var item in grouping.OrderBy(x=>x.LongDate))
                {
                    if (state == null)
                    {
                        state = new ChangeOfState();
                        state.Name = grouping.Key;
                        state.IsRunning = item.IsRunning();
                        state.StateAId = item.LogId;
                        state.StartTime = item.LongDate;
                        state.StateA = item.State;
                    }
                    else
                    {
                        if (state.StartTime < item.LongDate)
                        {
                            if (item.State != state.StateA)
                            {
                                //结束一个统计
                                state.EndTime = item.LongDate;
                                state.StateBId = item.LogId;
                                state.StateB = item.State;
                                state.TotalMilliseconds = Convert.ToInt32(state.EndTime.Value.Subtract(state.StartTime).TotalMilliseconds);

                                this.Add(state);
                                result++;

                                //开始一个新统计
                                state = new ChangeOfState();
                                state.Name = grouping.Key;
                                state.IsRunning = item.IsRunning();
                                state.StateAId = item.LogId;
                                state.StartTime = item.LongDate;
                                state.StateA = item.State;
                            }
                        }
                        else if (state.StartTime == item.LongDate)
                        {
                            if (item.State != state.StateA)
                            {
                                state = new ChangeOfState();
                                state.Name = grouping.Key;
                                state.IsRunning = item.IsRunning();
                                state.StateAId = item.LogId;
                                state.StartTime = item.LongDate;
                                state.StateA = item.State;
                            }
                        }
                    }
                }
            }

            return new CreateChangeOfStatusResult
            {
                CreatedRecords = result,
                 Finished=finished,
                LastTime = startTime
            };
        }
    }
}
