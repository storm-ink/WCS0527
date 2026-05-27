using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.Conveyor;
using Spiral.Web;
using Spiral;
using NHibernate.Linq;
namespace Client.WebUI.Areas.Conveyor.Controllers
{
    public class AlarmController : Controller
    {
        //
        // GET: /Conveyor/Alarm/

        [WmsOperation("输送线.报表.报警")]
        public ActionResult Index()
        {
            return View();
        }

           [WmsOperation("输送线.报表.报警")]
        [HttpPost]
        public ActionResult Index_List(AlarmSpec spec, FormCollection form)
        {
            PageInfo pageInfo = Request.GetPageInfo();

            try
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        int totalItemCount;
                        var factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<ConveyorRepository<Alarm>>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<Alarm>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                        unitOfWork.Commit();
                    }
                }

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                return PartialView();


            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.ToString());
            }
        }

        [WmsOperation("输送线.报表.查看报警")]
        public ActionResult AlarmWarning()
        {
            return View();
        }

        [WmsOperation("输送线.报表.查看报警")]
        [HttpPost]
        public ActionResult AlarmWarning_List(AlarmWarningSpec spec, FormCollection form)
        {
            PageInfo pageInfo = Request.GetPageInfo();

            try
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        int totalItemCount;
                        var factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<AlarmWarningRepository>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<AlarmWarning>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                        unitOfWork.Commit();
                    }
                }

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                return PartialView();


            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.ToString());
            }
        }

        [WmsOperation("输送线.报表.手动刷新报警")]
        [OpenSessionInViewFilter]
        public ActionResult RefreshWarning()
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();

            var count = context.Session.Query<AlarmWarning>().Count();
            var lastChangeOfState = context.Session.Query<AlarmWarning>()
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefault();

            ViewBag.Count = count;
            ViewBag.LastChangeOfState = lastChangeOfState;

            return View();
        }

        [WmsOperation("输送线.报表.手动刷新报警")]
        [HttpPost]
        public JsonResult RefreshWarning(String name, Int32 batchSize, DateTime? startTime,DateTime? endTime)
        {
            Client.Conveyor.CreateAlarmWarningResult reuslt;
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                {
                    var factory = new NhRepositoryFactory(context);
                    var repository = factory.Create<AlarmWarningRepository>();

                    reuslt = repository.Create(name, batchSize, startTime,endTime);

                    unitOfWork.Commit();
                }
            }


            return Json(new
            {
                RefreshCount = reuslt.CreatedRecords,
                Finished = reuslt.Finished,
                LastTime = reuslt.LastTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffff")
            });
        }
    }
}
