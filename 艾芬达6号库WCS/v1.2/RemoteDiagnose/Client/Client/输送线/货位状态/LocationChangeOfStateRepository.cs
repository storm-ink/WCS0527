using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
namespace Client.Conveyor
{
    public class LocationChangeOfStateRepository:Spiral.NhRepository<LocationChangeOfState,Int32>
    {
        public LocationChangeOfStateRepository(Spiral.NhRepositoryContext context)
            : base(context)
        {
        }


        public CreateChangeOfStatusResult Create(String deviceName, Int32 batchSize, DateTime? startTime, DateTime? endTime)
        {
            Int32 result = 0;
            Boolean finished = false;

            var lastChangeOfState = this.Query()
                        .OrderByDescending(x => x.StartTime)
                        .FirstOrDefault();
            if (lastChangeOfState != null && lastChangeOfState.EndTime == null)
            {
                this.Remove(lastChangeOfState.Id);
            }

            lastChangeOfState = this.Query()
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            var q = Context.Session.Query<Location>();
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

            q = q.Take(batchSize);
            var list = q.ToList();

            startTime = list.OrderByDescending(x => x.LongDate).Select(x => x.LongDate).FirstOrDefault();

            finished = batchSize > list.Count;

            LocationChangeOfState state = null;
            foreach (var grouping in list.GroupBy(x => new
            {
                x.Name,
                x.PosNo
            }))
            {
                foreach (var item in grouping)
                {
                    if (state == null)
                    {
                        state = new LocationChangeOfState();
                        state.Name = grouping.Key.Name;
                        state.PosNo = grouping.Key.PosNo;
                        state.IsRunning = item.IsRunning();
                        state.StateAId = item.LogId;
                        state.StartTime = item.LongDate;
                        state.StateA = item.Status;
                    }
                    else
                    {
                        if (state.StartTime < item.LongDate && item.IsRunning() != state.IsRunning)
                        {
                            //结束一个统计
                            state.EndTime = item.LongDate;
                            state.StateBId = item.LogId;
                            state.StateB = item.Status ;
                            state.TotalMilliseconds = Convert.ToInt32(state.EndTime.Value.Subtract(state.StartTime).TotalMilliseconds);

                            this.Add(state);
                            result++;

                            //开始一个新统计
                            state = new LocationChangeOfState();
                            state.Name = grouping.Key.Name;
                            state.PosNo = grouping.Key.PosNo;
                            state.IsRunning = item.IsRunning();
                            state.StateAId = item.LogId;
                            state.StartTime = item.LongDate;
                            state.StateA = item.Status;
                        }
                        else if (state.StartTime == item.LongDate && item.IsRunning() != state.IsRunning)
                        {
                            state = new LocationChangeOfState();
                            state.Name = grouping.Key.Name;
                            state.PosNo = grouping.Key.PosNo;
                            state.IsRunning = item.IsRunning();
                            state.StateAId = item.LogId;
                            state.StartTime = item.LongDate;
                            state.StateA = item.Status;
                        }
                    }
                }
            }

            return new CreateChangeOfStatusResult
            {
                RefreshCount = result,
                Finished = finished,
                LastTime = startTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffff")
            };
        }
    }
}
