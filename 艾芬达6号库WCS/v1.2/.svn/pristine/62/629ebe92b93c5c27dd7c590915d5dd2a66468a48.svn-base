using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Client.Conveyor
{
    public class AlarmWarningRepository : Spiral.NhRepository<AlarmWarning,Int32>
    {
        //\[DisplayName\("(\w+)"\)\]\r+\n+public virtual \w+ (.*?)\s*\{.*?\}
        //_warnings.Add\("$1"\,\(alarm, assertion\) =>\r\n\{\r\n return alarm.$2 == assertion;\r\n\}\);
        static Dictionary<String, Func<Alarm, Boolean, Boolean>> _allWarningTypes = new Dictionary<string, Func<Alarm, Boolean, Boolean>>();
        static AlarmWarningRepository()
        {
            _allWarningTypes.Add("是否手动", (alarm, assertion) =>
            {
                return alarm.Manual == assertion;
            });

            _allWarningTypes.Add("离线（隔离开关断开）", (alarm, assertion) =>
            {
                return alarm.Isolator == assertion;
            });
            _allWarningTypes.Add("电路保护器断开（断路器断开）", (alarm, assertion) =>
            {
                return alarm.Breaker == assertion;
            });
            _allWarningTypes.Add("光电异常", (alarm, assertion) =>
            {
                return alarm.Photocell == assertion;
            });
            _allWarningTypes.Add("运行超时", (alarm, assertion) =>
            {
                return alarm.RunOvertime == assertion;
            });
            _allWarningTypes.Add("占位超时", (alarm, assertion) =>
            {
                return alarm.OccupyOvertime == assertion;
            });
            _allWarningTypes.Add("有任务无货", (alarm, assertion) =>
            {
                return alarm.TaskNoGoods == assertion;
            });
            _allWarningTypes.Add("X轴电机变频器故障", (alarm, assertion) =>
            {
                return alarm.X_MotorVAF == assertion;
            });
            _allWarningTypes.Add("Y轴电机变频器故障", (alarm, assertion) =>
            {
                return alarm.Y_MotorVAF == assertion;
            });
            _allWarningTypes.Add("X轴电机正反转接触器故障", (alarm, assertion) =>
            {
                return alarm.X_MotorContactor == assertion;
            });
            _allWarningTypes.Add("X轴电机抱闸接触器故障", (alarm, assertion) =>
            {
                return alarm.X_MotorBraker == assertion;
            });
            _allWarningTypes.Add("Y轴电机正反转接触器故障", (alarm, assertion) =>
            {
                return alarm.Y_MotorContactor == assertion;
            });
            _allWarningTypes.Add("Y轴电机抱闸接触器故障", (alarm, assertion) =>
            {
                return alarm.Y_MotorBraker == assertion;
            });
            _allWarningTypes.Add("顶升电机正反转接触器故障", (alarm, assertion) =>
            {
                return alarm.Lift_MotorContactor == assertion;
            });
            _allWarningTypes.Add("顶升电机抱闸接触器故障", (alarm, assertion) =>
            {
                return alarm.Lift_MotorBraker == assertion;
            });
        }
        public AlarmWarningRepository(Spiral.NhRepositoryContext context)
            : base(context)
        {
           
        }

        public CreateAlarmWarningResult Create(String deviceName, Int32 batchSize, DateTime? startTime,DateTime? endTime)
        {
            startTime = 获取本批次起始时间(startTime);
            if (startTime == null)
            {
                return new CreateAlarmWarningResult
                {
                     CreatedRecords=0,
                     Finished=true
                };
            }

            if (endTime != null && startTime > endTime)
            {
                return new CreateAlarmWarningResult
                {
                    CreatedRecords = 0,
                    Finished = true
                }; 
            }

            var q = this.Context.Session.Query<Alarm>();

            q = q.Where(x => x.LongDate > startTime.Value);

            if (!string.IsNullOrWhiteSpace(deviceName))
            {
                q = q.Where(x => x.Name == deviceName);
            }
            if (endTime != null)
            {
                q = q.Where(x => x.LongDate < endTime);
            }

            q = q.Take(batchSize);

            var list=q.ToList();


            if (list.Count == 0)
            {
                startTime = this.Context.Session.Query<Alarm>().Where(x => x.LongDate > startTime).Select(x => x.LongDate).FirstOrDefault();
            }
            else
            {
                startTime = list.OrderByDescending(x => x.LongDate).Select(x => x.LongDate).First();
            }

            var records=q.ToList()
                .GroupBy(x => new
            {
                x.Name,
                x.PosNo
            })
            .OrderBy(x => x.Key.Name)
            .ThenBy(x => x.Key.PosNo);

            int result=0;
            foreach (var grouping in records)
            {
                AlarmWarning state = null;
                foreach (var warningType in _allWarningTypes)
                {
                    foreach (var item in grouping.OrderBy(x=>x.LongDate))
                    {
                        if (state == null && warningType.Value(item,true))
                        {
                            state = new AlarmWarning();
                            state.Name = grouping.Key.Name;
                            state.PosNo = grouping.Key.PosNo;
                            state.WarningName = warningType.Key;
                            state.StateAId = item.LogId;
                            state.StartTime = item.LongDate;
                        }
                        else if (state != null)
                        {
                            if (state.StartTime < item.LongDate && warningType.Value(item, false))
                            {
                                //结束一个统计
                                state.EndTime = item.LongDate;
                                state.StateBId = item.LogId;
                                state.TotalMilliseconds = Convert.ToInt32(state.EndTime.Value.Subtract(state.StartTime).TotalMilliseconds);

                                this.Add(state);
                                result++;

                                //开始一个新统计
                                if (warningType.Value(item, true))
                                {
                                    state = new AlarmWarning();
                                    state.Name = grouping.Key.Name;
                                    state.PosNo = grouping.Key.PosNo;
                                    state.WarningName = warningType.Key;
                                    state.StateAId = item.LogId;
                                    state.StartTime = item.LongDate;
                                }
                                else
                                {
                                    state = null;
                                }
                            }
                        }
                    }
                }
            }

            return new CreateAlarmWarningResult
            {
                 CreatedRecords=result,
                 Finished = startTime == null || startTime==default(DateTime),
                 LastTime = startTime
            };
        }

        DateTime? 获取本批次起始时间(DateTime? startTime)
        {
            var lastChangeOfState = this.Query()
                      .OrderByDescending(x => x.StartTime)
                      .FirstOrDefault();
            if (lastChangeOfState != null && lastChangeOfState.EndTime == null)
            {
                this.Remove(lastChangeOfState.Id);
            }

            var 记录的最后时间 = this.Query()
                .OrderByDescending(x => x.EndTime)
                .Select(x=>x.EndTime)
                .FirstOrDefault();
            if (记录的最后时间 == null)
            {
                if (startTime == null)
                {
                    return this.Context.Session.Query<Alarm>()
                        .OrderBy(x => x.LongDate)
                        .Select(x => x.LongDate)
                        .FirstOrDefault();
                }
                else
                {
                    return startTime;
                }
            }
            else
            {
                if (startTime != null && startTime > 记录的最后时间)
                {
                    return startTime;
                }
                else
                {
                    return 记录的最后时间;
                }
            }
        }
    }
}
