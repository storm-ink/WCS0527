using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using Client;
using Client.SingleForkCrane;

namespace Client.WebUI.Areas.SingleForkCrane.Controllers
{
    [Authorize]
    public class ChangeOfStatesController : Controller
    {

        [WmsOperation("单货位堆垛机.状态分析数据.查看")]
        public ActionResult Index()
        {
            return View();
        }

        [WmsOperation("单货位堆垛机.状态分析数据.查看")]
        [HttpPost]
        public ActionResult Index_List(ChangeOfStatesSpec spec, FormCollection form)
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
                        var repository = factory.Create<ChangeOfStateRepository>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<ChangeOfState>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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
        [WmsOperation("单货位堆垛机.状态分析数据.手动刷新")]
        [OpenSessionInViewFilter]
        public ActionResult Refresh()
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();

            var count = context.Session.Query<ChangeOfState>().Count();
            var lastChangeOfState = context.Session.Query<ChangeOfState>()
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            ViewBag.Count = count;
            ViewBag.LastChangeOfState = lastChangeOfState;

            return View();
        }

        [WmsOperation("单货位堆垛机.状态分析数据.手动刷新")]
        [HttpPost]
        public JsonResult Refresh(String name, Int32 batchSize, DateTime? startTime, DateTime? endTime)
        {
            CreateChangeOfStatusResult reuslt;
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                {
                    var factory = new NhRepositoryFactory(context);
                    var repository = factory.Create<ChangeOfStateRepository>();

                    reuslt = repository.Create(name, batchSize, startTime, endTime);

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
