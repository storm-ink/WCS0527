using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using Client.SingleForkCrane;

namespace Client.WebUI.Areas.SingleForkCrane.Controllers
{
    [Authorize]
    public class DiagnosisDataController : Controller
    {

        [WmsOperation("单货位堆垛机.状态元数据.查看")]
        public ActionResult Index(bool? isReturned)
        {

            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            if (isReturned.HasValue && isReturned.Value)
            {
                returnInfo.SetActive();
            }
            else
            {
                returnInfo.Delete();
            }

            ViewBag.ReturnInfoKey = this.GetType().ToString();

            return View();
        }

        [WmsOperation("单货位堆垛机.状态元数据.查看")]
        [HttpPost]
        public ActionResult Index_List(StatusDiagnosisDataSpec spec, FormCollection form)
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
                        var repository = factory.Create<StatusDiagnosisDataRepository>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<StatusDiagnosisData>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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

        [WmsOperation("单货位堆垛机.状态元数据.回放")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult GetPlaybackData(String name, DateTime startTime, DateTime? endTime)
        {
            if (endTime == null)
            {
                endTime = startTime.AddMinutes(10);
            }
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();
            var q = context.Session.Query<StatusDiagnosisData>()
                .Where(x => x.LongDate >= startTime
                    && x.LongDate < endTime);
            if (!string.IsNullOrWhiteSpace(name))
            {
                q = q.Where(x => x.Name == name);
            }
            return Json(q.OrderBy(x => x.LongDate));
        }

        public ContentResult GetMonitoryData()
        {
            throw new NotImplementedException();
        }


        [WmsOperation("单货位堆垛机.概要信息")]
        [OpenSessionInViewFilter]
        public ActionResult Summary()
        {
            return View();
        }
    }
}
