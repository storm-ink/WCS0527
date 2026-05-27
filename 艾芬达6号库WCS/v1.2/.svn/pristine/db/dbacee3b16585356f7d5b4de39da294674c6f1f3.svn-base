using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using Client.RailGuidedVehicle;

namespace Client.WebUI.Areas.RailGuidedVehicle.Controllers
{
    [Authorize]
    public class DiagnosisDataController : Controller
    {

        [WmsOperation("单货位穿梭车.状态元数据.查看")]
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

        [WmsOperation("单货位穿梭车.状态元数据.查看")]
        [HttpPost]
        public ActionResult Index_List(RailGuidedVehicleDiagnosisDataSpec spec, FormCollection form)
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
                        var repository = factory.Create<RailGuidedVehicleDiagnosisDataRepository>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<RailGuidedVehicleDiagnosisData>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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


        /// <summary>
        /// 设备状态回放
        /// </summary>
        /// <returns></returns>
        [WmsOperation("单货位穿梭车.状态元数据.回放")]
        [OpenSessionInViewFilter]
        public ActionResult Playback()
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();
            var deviceNames = context.Session.Query<RailGuidedVehicleDiagnosisData>()
                .GroupBy(x => x.Name)
                .Select(x => x.Key)
                .OrderBy(x => x)
                .ToList();

            ViewBag.DeviceNames = deviceNames;


            ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
            returnInfo.Delete();

            ViewBag.ReturnInfoKey = this.GetType().ToString();
            return View();
        }

        [WmsOperation("单货位穿梭车.状态元数据.回放")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult Playback(String name, DateTime startTime, DateTime endTime)
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();
            var data = context.Session.Query<RailGuidedVehicleDiagnosisData>()
                .Where(x => x.Name == name
                    && x.LongDate >= startTime
                    && x.LongDate < endTime)
                .OrderBy(x => x.LongDate);
            return Json(data);
        }


        [WmsOperation("单货位穿梭车.概要信息")]
        [OpenSessionInViewFilter]
        public ActionResult Summary()
        {
            return View();
        }
    }
}
