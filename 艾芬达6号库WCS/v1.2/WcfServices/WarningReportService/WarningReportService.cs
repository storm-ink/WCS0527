using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Wcs.Framework;

namespace Wcs.Services.Wcf
{
    public class WarningReportService:IWarningReportService
    {
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Assert, OperationName = "报警统计\\查询")]
        public Framework.WarningRecord[] Find(string deviceType, string deviceName, string code, string name, string userName, bool? isFault, bool? repaired, DateTime? fromDate, DateTime? toDate,
            Int32 pageSize,
            Int32 currentPageNo,
            out long totals)
        {
            if (currentPageNo < 1)
            {
                currentPageNo = 1;
            }

            Framework.WarningRecord[] result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q=unitOfWork.session.Query<WarningRecord>();
                if (!String.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType.Contains(deviceType));
                }

                if (!String.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Device.Contains(deviceName));
                }

                if (!String.IsNullOrWhiteSpace(code))
                {
                    q = q.Where(x => x.Code.Contains(code));
                }

                if (!String.IsNullOrWhiteSpace(name))
                {
                    q = q.Where(x => x.Name.Contains(name));
                }

                if (!String.IsNullOrWhiteSpace(userName))
                {
                    q = q.Where(x => x.UserName.Contains(userName));
                }

                if (isFault != null)
                {
                    q = q.Where(x => x.IsFault == isFault);
                }

                if (repaired != null)
                {
                    q = q.Where(x => x.Repaired == repaired);
                }


                if (fromDate != null)
                {
                    q = q.Where(x => x.BeginingAt >= fromDate);
                }


                if (toDate != null)
                {
                    q = q.Where(x => x.BeginingAt < toDate);
                }

                totals = q.Count();

                result = q
                    .Skip((currentPageNo-1) * pageSize)
                    .Take(pageSize).ToArray();

                unitOfWork.Commit();
            }

            return result;
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Assert, OperationName = "报警统计\\更新")]
        public void Update(Framework.WarningRecord record)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var obj = unitOfWork.session.Get<WarningRecord>(record.Id);
                if (obj == null)
                {
                    throw new Exception(String.Format("未找到 id 为 {0} 的报警记录.",record.Id));
                }

                obj.BeginingAt = record.BeginingAt;
                obj.Category = record.Category;
                obj.Code = record.Code;
                obj.Device = record.Device;
                obj.DeviceType = record.DeviceType;
                obj.EndingAt = record.EndingAt;
                obj.FinishedAt = record.FinishedAt;
                obj.IsFault = record.IsFault;
                obj.Name = record.Name;
                obj.Reason = record.Reason;
                obj.Repaired = record.Repaired;
                obj.Result = record.Result;
                obj.UserName = record.UserName;
                obj.Description = record.Description;

                unitOfWork.session.Update(obj);

                unitOfWork.Commit();
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Assert, OperationName = "报警统计\\删除")]
        public void Delete(int id)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var obj = unitOfWork.session.Get<WarningRecord>(id);
                if (obj == null)
                {
                    throw new Exception(String.Format("未找到 id 为 {0} 的报警记录.", id));
                }
                
                unitOfWork.session.Delete(obj);

                unitOfWork.Commit();
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Assert, OperationName = "报警统计\\获取明细")]
        public Framework.WarningRecord Get(int id)
        {
            WarningRecord result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                result = unitOfWork.session.Get<WarningRecord>(id);
                
                unitOfWork.Commit();
            }

            return result;
        }

        public bool Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Assert, OperationName = "报警统计\\添加")]
        public void Add(WarningRecord record)
        {
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                unitOfWork.session.Save(record);

                unitOfWork.Commit();
            }
        }

        public Dictionary<String,Int32> Report_CountByWarningType(DateTime? fromDate, DateTime? toDate,String deviceName,Boolean includeWarning,Boolean groupByName)
        {
            Dictionary<String, Int32> result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();
                if (fromDate != null)
                {
                    q = q.Where(x => x.BeginingAt >= fromDate);
                }

                if (toDate != null)
                {
                    q = q.Where(x => x.BeginingAt <= toDate);
                }

                if (!String.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Device.Contains(deviceName));
                }

                if (includeWarning==false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                if (groupByName)
                {
                    result = q.GroupBy(x => x.Name??x.Code)
                        .Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Count()
                        })
                        .ToDictionary(x => x.Name, x => x.Value);
                }
                else
                {
                    result = q.GroupBy(x => x.Code)
                        .Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Count()
                        })
                        .ToDictionary(x => x.Name, x => x.Value);
                }
                unitOfWork.Commit();
            }

            return result;
        }

        public Dictionary<String, Int32> Report_CountByDeviceName(DateTime? fromDate, DateTime? toDate, String warningName, Boolean includeWarning)
        {
            Dictionary<String, Int32> result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();
                if (fromDate != null)
                {
                    q = q.Where(x => x.BeginingAt >= fromDate);
                }

                if (toDate != null)
                {
                    q = q.Where(x => x.BeginingAt <= toDate);
                }

                if (!String.IsNullOrWhiteSpace(warningName))
                {
                    q = q.Where(x => x.Name.Contains(warningName));
                }

                if (includeWarning == false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                result = q.GroupBy(x => x.DeviceType+x.Device)
                    .Select(x => new
                    {
                        Name = x.Key,
                        Value = x.Count()
                    })
                    .ToDictionary(x => x.Name, x => x.Value);

                unitOfWork.Commit();
            }

            return result;
        }

        public Dictionary<string, List<int[]>> Report_TrendByMonth(Int32 year,Int32 month, string[] deviceNames, string[] warningNames, bool includeWarning)
        {
            DateTime fromDate, toDate;
            fromDate = new DateTime(year, month, 1);
            toDate = fromDate.AddMonths(1).AddDays(-1);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
            Dictionary<string, List<int[]>> result=new Dictionary<string,List<int[]>>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();

                if (deviceNames != null)
                {
                    warningNames = warningNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                }


                if (deviceNames != null)
                {
                    deviceNames = deviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                }

                q = q.Where(x => x.BeginingAt >= fromDate);
                q = q.Where(x => x.BeginingAt <= toDate);

                if (warningNames!=null && warningNames.Length > 0)
                {
                    q = q.Where(x =>warningNames.Contains(x.Name));
                }

                if (deviceNames != null && deviceNames.Length > 0)
                {
                    q = q.Where(x => deviceNames.Contains(x.Device));
                }

                if (includeWarning == false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                var tempList = q.GroupBy(x => new
                {
                    x.Device,
                    x.BeginingAt.Day
                })
                .Select(x=>new
                {
                    x.Key,
                    Value = x.Count()
                })
                .ToList()
                .Select(x => new
                {
                    Name = x.Key.Device,
                    Value = new Int32[]{x.Key.Day,x.Value},
                })
                .GroupBy(x=>x.Name)
                .ToDictionary(x => x.Key, x => x.Select(y=>y.Value).ToList());

                foreach (var item in tempList)
                {
                    List<Int32[]> values = new List<int[]>();
                    for (int day = 1; day <= Convert.ToInt32(toDate.Subtract(fromDate.Date).TotalDays); day++)
                    {
                        Int32[] value = item.Value.FirstOrDefault(x => x[0] == day);
                        if (value == null)
                        {
                            value = new Int32[] { day, 0 };
                        }

                        values.Add(value);
                    }

                    result.Add(item.Key, values);
                }


                unitOfWork.Commit();
            }

            return result;
        }
        
        public Dictionary<string, List<int[]>> Report_TrendByYear(int year, string[] deviceNames, string[] warningNames, bool includeWarning)
        {
            DateTime fromDate, toDate;
            fromDate = new DateTime(year, 1, 1);
            toDate = fromDate.AddYears(1).AddDays(-1);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
            Dictionary<string, List<int[]>> result = new Dictionary<string, List<int[]>>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();

                if (deviceNames != null)
                {
                    warningNames = warningNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                }


                if (deviceNames != null)
                {
                    deviceNames = deviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                }

                q = q.Where(x => x.BeginingAt >= fromDate);
                q = q.Where(x => x.BeginingAt <= toDate);

                if (warningNames != null && warningNames.Length > 0)
                {
                    q = q.Where(x => warningNames.Contains(x.Name));
                }

                if (deviceNames != null && deviceNames.Length > 0)
                {
                    q = q.Where(x => deviceNames.Contains(x.Device));
                }

                if (includeWarning == false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                var tempList = q.GroupBy(x => new
                {
                    x.Device,
                    x.BeginingAt.Month
                })
                .Select(x => new
                {
                    x.Key,
                    Value = x.Count()
                })
                .ToList()
                .Select(x => new
                {
                    Name = x.Key.Device,
                    Value = new Int32[] { x.Key.Month, x.Value },
                })
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).ToList());

                foreach (var item in tempList)
                {
                    List<Int32[]> values = new List<int[]>();
                    for (int month = 1; month <= 12; month++)
                    {
                        Int32[] value = item.Value.FirstOrDefault(x => x[0] == month);
                        if (value == null)
                        {
                            value = new Int32[] { month, 0 };
                        }

                        values.Add(value);
                    }

                    result.Add(item.Key, values);
                }


                unitOfWork.Commit();
            }

            return result;
        }


        public Dictionary<Boolean, int> Report_CountByRepaird(DateTime? fromDate, DateTime? toDate, string[] deviceNames, string[] warningNames, bool includeWarning)
        {
            if (deviceNames != null)
            {
                warningNames = warningNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }


            if (deviceNames != null)
            {
                deviceNames = deviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }

            Dictionary<Boolean, Int32> result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();
                if (fromDate != null)
                {
                    q = q.Where(x => x.BeginingAt >= fromDate);
                }

                if (toDate != null)
                {
                    q = q.Where(x => x.BeginingAt <= toDate);
                }

                if (warningNames != null && warningNames.Length > 0)
                {
                    q = q.Where(x => warningNames.Contains(x.Name));
                }

                if (deviceNames != null && deviceNames.Length > 0)
                {
                    q = q.Where(x => deviceNames.Contains(x.Device));
                }

                if (includeWarning == false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                result = q.GroupBy(x => x.Repaired)
                        .Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Count()
                        })
                        .ToDictionary(x => x.Name, x => x.Value);
                unitOfWork.Commit();
            }

            return result;
        }

        public Dictionary<bool, int> Report_CountByFault(DateTime? fromDate, DateTime? toDate, string[] deviceNames, string[] warningNames)
        {
            if (deviceNames != null)
            {
                warningNames = warningNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }


            if (deviceNames != null)
            {
                deviceNames = deviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            }

            Dictionary<Boolean, Int32> result;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();
                if (fromDate != null)
                {
                    q = q.Where(x => x.BeginingAt >= fromDate);
                }

                if (toDate != null)
                {
                    q = q.Where(x => x.BeginingAt <= toDate);
                }

                if (warningNames != null && warningNames.Length > 0)
                {
                    q = q.Where(x => warningNames.Contains(x.Name));
                }

                if (deviceNames != null && deviceNames.Length > 0)
                {
                    q = q.Where(x => deviceNames.Contains(x.Device));
                }

                result = q.GroupBy(x => x.IsFault)
                        .Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Count()
                        })
                        .ToDictionary(x => x.Name, x => x.Value);
                unitOfWork.Commit();
            }

            return result;
        }


        public Dictionary<string, List<double[]>> Report_FailureRate(int year, string[] deviceNames, bool includeWarning)
        {
            var projectStartUsingTime = GetProjectStartUsingTime();

            DateTime fromDate, toDate;
            fromDate = new DateTime(year, 1, 1);
            toDate = fromDate.AddYears(1).AddDays(-1);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
            Dictionary<string, List<double[]>> result = new Dictionary<string, List<double[]>>();
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<WarningRecord>();

                if (deviceNames != null)
                {
                    deviceNames = deviceNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                }

                q = q.Where(x => x.BeginingAt >= fromDate);
                q = q.Where(x => x.BeginingAt <= toDate);
                
                if (deviceNames != null && deviceNames.Length > 0)
                {
                    q = q.Where(x => deviceNames.Contains(x.Device));
                }

                if (includeWarning == false)
                {
                    q = q.Where(x => x.IsFault == true);
                }

                var devices = q
                    .GroupBy(x => new
                {
                    x.Device,
                    x.DeviceType
                })
                .Select(x => new
                {
                    x.Key.Device,
                    x.Key.DeviceType
                })
                .ToArray();

                foreach (var deviceKey in devices)
                {
                    var workStartTime = GetWorkStartTime(deviceKey.DeviceType, deviceKey.Device);
                    var offDutyTime = GetOffDutyTime(deviceKey.DeviceType, deviceKey.Device);

                    var tempList = q
                        .Where(x=>x.Device==deviceKey.Device && x.DeviceType==deviceKey.DeviceType)
                        .Where(x=>
                            (x.BeginingAt.Hour+x.BeginingAt.Minute/60)>=workStartTime.TotalHours
                            && (x.BeginingAt.Hour + x.BeginingAt.Minute / 60) <= offDutyTime.TotalHours

                        )
                        .GroupBy(x => new
                    {
                        x.BeginingAt.Month
                    })
                    .Select(x => new
                    {
                        x.Key,
                        Value = x.Sum(y => y.TotalMilliseconds)
                    })
                    .ToList()
                    .Select(x => new
                    {
                        Name = deviceKey.Device,
                        DeviceType = deviceKey.DeviceType,
                        Value = new Double[] { x.Key.Month, Convert.ToDouble(x.Value) / 3600000d, GetMonthWorkTimes(deviceKey.DeviceType, deviceKey.Device, year, x.Key.Month) },
                    })
                    .GroupBy(x => x.Name);

                    foreach (var item in tempList)
                    {
                        List<double[]> values = new List<double[]>();
                        for (double month = 1; month <= 12; month++)
                        {
                            double[] value = item.Where(x => x.Value[0] == month).Select(x => x.Value).FirstOrDefault();
                            if (value == null)
                            {
                                if (projectStartUsingTime < new DateTime(year, Convert.ToInt32(month), 1))
                                {
                                    value = new double[] { month, 0, GetMonthWorkTimes(item.FirstOrDefault().DeviceType, item.Key, year, Convert.ToInt32(month)) };
                                }
                                else
                                {
                                    value = new double[] { month, 0, 0 };
                                }
                            }

                            values.Add(value);
                        }

                        result.Add(deviceKey.DeviceType + deviceKey.Device, values);
                    }
                }



                unitOfWork.Commit();
            }

            return result;
        }

        /// <summary>
        /// 获取指定设备每月的工作总时长
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Double GetMonthWorkTimes(String deviceType,String deviceName, Int32 year, Int32 month)
        {
            var 上班时间 = GetWorkStartTime(deviceType, deviceName);
            var 下班时间 = GetOffDutyTime(deviceType, deviceName);

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var totalDays = endDate.Subtract(startDate).TotalDays;
            var hoursOfDay = 下班时间.Subtract(上班时间).TotalHours;

            return totalDays * hoursOfDay;
        }

        /// <summary>
        /// 获取指定设备的每年工作总时长
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Double GetYearWorkTimes(String deviceType, String deviceName, Int32 year)
        {
            var 上班时间 = GetWorkStartTime(deviceType, deviceName);
            var 下班时间 = GetOffDutyTime(deviceType, deviceName);

            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            var yearDays = endDate.Subtract(startDate).TotalDays;
            var hoursOfDay = 下班时间.Subtract(上班时间).TotalHours;

            return yearDays * hoursOfDay;
        }



        /// <summary>
        /// 获取项目启用时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetProjectStartUsingTime()
        {
            var value = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .GetSetting<String>("报警统计/项目启用时间");

            DateTime r;
            if (!DateTime.TryParse(value, out r))
            {
                r = DateTime.Now.Date;
            }

            return r;
        }
        /// <summary>
        /// 设置项目启用时间
        /// </summary>
        /// <param name="startUsingTime"></param>
        public static void SetProjectStartUsingTime(DateTime startUsingTime)
        {
            Wcs.Framework.Cfg.WcsConfiguration
            .Instance
            .SettingCollection
            .SetSetting("报警统计/项目启用时间", startUsingTime.ToString());
        }
        /// <summary>
        /// 获取默认上班时间
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetDefaultWorkStartTime()
        {
            var value = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .GetSetting<String>("报警统计/默认上班时间", "08:30:00");

            TimeSpan r;
            if (!TimeSpan.TryParse(value, out r))
            {
                r = new TimeSpan(08, 30, 0);
            }

            return r;
        }
        /// <summary>
        /// 设置默认上班时间
        /// </summary>
        /// <param name="v"></param>
        public static void SetDefaultWorkStartTime(TimeSpan v)
        {
            Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .SetSetting("报警统计/默认下班时间", v.ToString());
        }
        /// <summary>
        /// 获取默认下班时间
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetDefaultOffDutyTime()
        {
            var value = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .GetSetting<String>("报警统计/默认下班时间", "17:30:00");

            TimeSpan r;
            if (!TimeSpan.TryParse(value, out r))
            {
                r = new TimeSpan(17, 30, 0);
            }

            return r;
        }
        /// <summary>
        /// 设置默认下班时间
        /// </summary>
        /// <param name="v"></param>
        public static void SetDefaultOffDutyTime(TimeSpan v)
        {
            Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .SetSetting("报警统计/默认下班时间", v.ToString());
        }

        /// <summary>
        /// 获取上班时间
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static TimeSpan GetWorkStartTime(String deviceType, String deviceName)
        {
            return InternalGetTime(deviceType, deviceName, "上班时间", GetDefaultWorkStartTime().ToString());
        }
        /// <summary>
        /// 设置上班时间
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <param name="v"></param>
        public static void SetWorkStartTime(String deviceType, String deviceName, TimeSpan v)
        {

            InternalSetTime(deviceType, deviceName, "上班时间", v);
        }
        /// <summary>
        /// 获取下班时间
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static TimeSpan GetOffDutyTime(String deviceType, String deviceName)
        {
            return InternalGetTime(deviceType, deviceName, "下班时间", GetDefaultOffDutyTime().ToString());
        }
        /// <summary>
        /// 设置下班时间
        /// </summary>
        /// <param name="deviceType"></param>
        /// <param name="deviceName"></param>
        /// <param name="v"></param>
        public static void SetOffDutyTime(String deviceType, String deviceName, TimeSpan v)
        {
            InternalSetTime(deviceType, deviceName, "下班时间", v);
        }

        static TimeSpan InternalGetTime(String deviceType, String deviceName, String name,String defaultValue)
        {
            var value = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .GetSetting<String>(string.Format("报警统计/设备日工作时间/{0}/{1}/{2}", deviceType, deviceName, name), defaultValue);

            TimeSpan time;
            if (TimeSpan.TryParse(value, out time))
            {
                return time;
            }
            else
            {
                return TimeSpan.Parse(value);
            }
        }

        static void InternalSetTime(String deviceType, String deviceName, String name, TimeSpan value)
        {
            Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .SettingCollection
                .SetSetting(string.Format("报警统计/设备日工作时间/{0}/{1}/{2}", deviceType, deviceName, name), value.ToString());
        }
    }
}
