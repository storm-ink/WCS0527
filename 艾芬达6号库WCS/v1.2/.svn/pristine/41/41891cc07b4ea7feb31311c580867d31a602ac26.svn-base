using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using System.Drawing;
using Client;
namespace Client.WebUI.Areas.Warnings.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        [WmsOperation("报表.按故障类型统计")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult WarningReports()
        {

            var context = HttpContext.GetNhRepositoryContext();
            var repositoryFactory = new NhRepositoryFactory(context);
            ViewBag.DeviceTypes = repositoryFactory.Create<WarningRepository>().GetDeviceTypes();

            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }

        [WmsOperation("报表.按故障类型统计")]
        [HttpPost]
        public JsonResult WarningReports(String deviceName, DateTime? startTime, DateTime? endTime,String deviceType,WarningLevel? level)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var q = context.Session.Query<Warning>();

                if (!string.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Name.Contains(deviceName));
                }
                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType == deviceType);
                }

                if (startTime != null)
                {
                    q = q.Where(x => x.StartTime >= startTime);
                }

                if (endTime != null)
                {
                    q = q.Where(x => x.EndTime < endTime);
                }
                if (level != null)
                {
                    q = q.Where(x => x.Level == level);
                }

                var q1 = q.GroupBy(x => x.ErrorCode).ToList();

                var totals = q1.Sum(x => x.Count());
                List<object> list = new List<object>();
                foreach (var item in q1)
                {
                    var alarmName = item.First().AlarmName;
                    list.Add(new
                    {
                        name = String.IsNullOrWhiteSpace(alarmName) ? item.Key.ToString() : alarmName,
                        code = item.Key,
                        value = item.Count(),// / Convert.ToDecimal(totals) * 100m,
                        color = Client.App.Helper.GetRandomColor(),
                        totals = item.Count(),
                    });
                }

                return Json(list);
            }
        }

        [WmsOperation("报表.报表明细")]
        [HttpPost]
        public JsonResult GetReportDetails(String deviceName, DateTime? startTime, DateTime? endTime, Int32? code, String deviceType, WarningLevel? level)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var q = context.Session.Query<Warning>();

                if (!string.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Name.Contains(deviceName));
                }

                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType == deviceType);
                }

                if (startTime != null)
                {
                    q = q.Where(x => x.StartTime >= startTime);
                }

                if (endTime != null)
                {
                    q = q.Where(x => x.EndTime < endTime);
                }

                if (code != null)
                {
                    q = q.Where(x => x.ErrorCode == code.Value);
                }

                if (level != null)
                {
                    q = q.Where(x => x.Level == level);
                }

                var totals = q.Count();
                List<object> list = new List<object>();
                foreach (var item in q.ToList().GroupBy(x => x.Name).OrderBy(x => x.Key))
                {
                    list.Add(new
                    {
                        name = item.Key,
                        //value = item.Count() / Convert.ToDecimal(totals) * 100m,
                        value = item.Count(),
                        color = Client.App.Helper.GetRandomColor(),
                        totals = item.Count(),
                    });
                }

                return Json(list);
            }
        }


        [WmsOperation("报表.按设备统计")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult WarningReports_CountByDevice()
        {

            var context = HttpContext.GetNhRepositoryContext();
            var repositoryFactory = new NhRepositoryFactory(context);
            ViewBag.DeviceTypes = repositoryFactory.Create<WarningRepository>().GetDeviceTypes();

            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }

        [WmsOperation("报表.按设备统计")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult WarningReports_CountByDevice(String deviceName, DateTime? startTime, DateTime? endTime, String deviceType, WarningLevel? level)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var q = context.Session.Query<Warning>();

                if (!string.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Name.Contains(deviceName));
                }

                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType == deviceType);
                }

                if (startTime != null)
                {
                    q = q.Where(x => x.StartTime >= startTime);
                }

                if (endTime != null)
                {
                    q = q.Where(x => x.EndTime < endTime);
                }

                if (level != null)
                {
                    q = q.Where(x => x.Level == level);
                }

                var q1 = q.GroupBy(x => x.Name).ToList();

                var totals = q1.Sum(x => x.Count());
                List<object> list = new List<object>();
                foreach (var item in q1)
                {
                    list.Add(new
                    {
                        name = item.Key,
                        value = item.Count(),
                        color = Client.App.Helper.GetRandomColor(),
                        totals = totals,
                    });
                }

                return Json(list);
            }
        }

        [WmsOperation("报表.按故障类型统计月走势图")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult WarningReports_CountByMonthAndWarning()
        {
            var context = HttpContext.GetNhRepositoryContext();
            var repositoryFactory = new NhRepositoryFactory(context);
            ViewBag.DeviceTypes = repositoryFactory.Create<WarningRepository>().GetDeviceTypes();


            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }

        [WmsOperation("报表.按故障类型统计月走势图")]
        [HttpPost]
        public JsonResult WarningReports_CountByMonthAndWarning(String deviceName, String warnName, Int32 year, Int32 month, String deviceType, WarningLevel? level)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var q = context.Session.Query<Warning>();

                if (!string.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Name.Contains(deviceName));
                }
                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType == deviceType);
                }

                q = q.Where(x => x.StartTime.Year == year);

                q = q.Where(x => x.StartTime.Month == month);

                if (!String.IsNullOrWhiteSpace(warnName))
                {
                    q = q.Where(x => x.AlarmName.Contains(warnName));
                }

                if (level != null)
                {
                    q = q.Where(x => x.Level == level);
                }

                var q1 = q.GroupBy(x => x.ErrorCode).ToList();

                var totals = q1.Sum(x => x.Count());

                var date1 = new DateTime(year, month, 1);
                var dayCount = Convert.ToInt32(date1.AddMonths(1).Subtract(date1).TotalDays);
                List<int> labels = new List<int>();
                List<object> list = new List<object>();
                for (int i = 1; i <= dayCount; i++)
                {
                    labels.Add(i);
                }

                foreach (var item in q1)
                {
                    List<Int32> value = new List<int>();
                    for (int i = 1; i <= dayCount; i++)
                    {
                        value.Add(item.Count(x => x.StartTime.Day == i));
                    }
                    var alarmName = item.First().AlarmName;
                    var obj = new
                    {
                        name = String.IsNullOrWhiteSpace(alarmName) ? item.Key.ToString() : alarmName,
                        value = value,
                        color = Client.App.Helper.GetRandomColor(),
                    };

                    list.Add(obj);
                }

                return Json(new
                {
                    labels = labels,
                    data = list
                });
            }
        }


        [WmsOperation("报表.按设备统计月走势图")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult WarningReports_CountByMonthAndDeviceName()
        {

            var context = HttpContext.GetNhRepositoryContext();
            var repositoryFactory = new NhRepositoryFactory(context);
            ViewBag.DeviceTypes = repositoryFactory.Create<WarningRepository>().GetDeviceTypes();

            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }

        [WmsOperation("报表.按设备统计月走势图")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult WarningReports_CountByMonthAndDeviceName(String deviceName, String warnName, Int32 year, Int32 month, String deviceType, WarningLevel? level)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var q = context.Session.Query<Warning>();

                if (!string.IsNullOrWhiteSpace(deviceName))
                {
                    q = q.Where(x => x.Name.Contains(deviceName));
                }
                if (!string.IsNullOrWhiteSpace(deviceType))
                {
                    q = q.Where(x => x.DeviceType == deviceType);
                }

                q = q.Where(x => x.StartTime.Year == year);

                q = q.Where(x => x.StartTime.Month == month);

                if (!String.IsNullOrWhiteSpace(warnName))
                {
                    q = q.Where(x => x.AlarmName.Contains(warnName));
                }
                if (level != null)
                {
                    q = q.Where(x => x.Level == level);
                }

                var q1 = q.GroupBy(x => x.Name).ToList();

                var totals = q1.Sum(x => x.Count());

                var date1 = new DateTime(year, month, 1);
                var dayCount = Convert.ToInt32(date1.AddMonths(1).Subtract(date1).TotalDays);
                List<int> labels = new List<int>();
                List<object> list = new List<object>();
                for (int i = 1; i <= dayCount; i++)
                {
                    labels.Add(i);
                }

                foreach (var item in q1)
                {
                    List<Int32> value = new List<int>();
                    for (int i = 1; i <= dayCount; i++)
                    {
                        value.Add(item.Count(x => x.StartTime.Day == i));
                    }

                    var obj = new
                    {
                        name = item.Key,
                        value = value,
                        color = Client.App.Helper.GetRandomColor(),
                    };

                    list.Add(obj);
                }

                return Json(new
                {
                    labels = labels,
                    data = list
                });
            }
        }

        [WmsOperation("报表.故障率")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult FailureRate()
        {
            
        
            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }
    }
}
