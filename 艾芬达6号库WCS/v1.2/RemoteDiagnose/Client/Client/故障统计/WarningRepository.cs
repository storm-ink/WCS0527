using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
namespace Client
{
    public class WarningRepository: Spiral.NhRepository<Warning, Int32>
    {
        public WarningRepository(NhRepositoryContext context)
            : base(context)
        {
        }

        public String[] GetDeviceTypes()
        {
            return this.Query()
                .GroupBy(x => x.DeviceType)
                .Select(x => x.Key)
                .ToArray();
        }

        public String[] GetDeviceName(String deviceType)
        {
            if (String.IsNullOrWhiteSpace(deviceType))
            {
                return this.Query()
                    .GroupBy(x => x.Name)
                    .Select(x => x.Key)
                    .ToArray();
            }
            else
            {
                return this.Query()
                    .Where(x => x.DeviceType == deviceType)
                    .GroupBy(x => x.Name)
                    .Select(x => x.Key)
                    .ToArray();
            }
        }

        public Int32 SyncWarning<T>()
            where T : AbstractDiagnosisData
        {
            int result = 0;
            var lastRecond = this.Context.Session.Query<Warning>()
                .Where(x => x.SourceType == typeof(T).Name)
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefault();

            DateTime lastTime;
            if (lastRecond == null)
            {
                lastTime = new DateTime(1900, 1, 1);
            }
            else
            {
                lastTime = lastRecond.BatchDate;
            }

            var datas = this.Context.Session.Query<T>().Where(x => x.LongDate > lastTime)
                .Take(10000)
                .ToList();

            Warning warning = null;
            DateTime? batchDate=datas.Max(x=>(DateTime?)x.LongDate);

            if (batchDate != null && lastRecond!=null)
            {
                lastRecond.BatchDate = batchDate.Value;

                this.Update(lastRecond);
            }

            foreach (var grouping in datas.GroupBy(x => x.Name))
            {
                foreach (var item in grouping.OrderBy(x => x.LongDate).ThenBy(x => x.LogId))
                {
                    if (warning == null && item.IsWarnInfo())
                    {
                        warning = item.NewWarning();
                        warning.BatchDate=batchDate.Value;
                    }
                    else if (warning != null)
                    {
                        if (warning.StartTime < item.LongDate && warning.ErrorCode != item.GetErrorCode())
                        {
                            //结束一个统计
                            warning.EndTime = item.LongDate;
                            warning.StateBId = item.LogId;
                            warning.TotalMilliseconds = Convert.ToInt32(warning.EndTime.Subtract(warning.StartTime).TotalMilliseconds);

                            this.Add(warning);
                            result++;
                            //开始一个新统计
                            if (item.IsWarnInfo())
                            {
                                warning = item.NewWarning();
                                warning.BatchDate = batchDate.Value;
                            }
                            else
                            {
                                warning = null;
                            }
                        }
                        else if (warning.StartTime == item.LongDate && warning.ErrorCode == item.GetErrorCode() && item.IsWarnInfo())
                        {
                            warning = item.NewWarning();
                            warning.BatchDate = batchDate.Value;
                        }
                    }
                }
            }

            return result;
        }
    }
}
