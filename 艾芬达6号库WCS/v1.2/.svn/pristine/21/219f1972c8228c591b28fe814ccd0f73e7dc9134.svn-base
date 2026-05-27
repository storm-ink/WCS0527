using Client.Conveyor;
using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
namespace Client.WebUI.Areas.Conveyor.Controllers
{
    public class LocationController : Controller
    {
        //
        // GET: /Conveyor/Location/
        [Spiral.Web.WmsOperation("输送线.货位状态")]
        public ActionResult Index()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult Index_List(LocationSpec spec, FormCollection form)
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
                        var repository = factory.Create<ConveyorRepository<Location>>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<Location>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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


        public ActionResult LocationChangeOfState()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LocationChangeOfState_List(Client.Conveyor.LocationChangeOfStateSpec spec, FormCollection form)
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
                        var repository = factory.Create<LocationChangeOfStateRepository>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<LocationChangeOfState>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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


        [OpenSessionInViewFilter]
        public ActionResult RefreshChangeOfState()
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();

            var count = context.Session.Query<LocationChangeOfState>().Count();
            var lastChangeOfState = context.Session.Query<LocationChangeOfState>()
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            ViewBag.Count = count;
            ViewBag.LastChangeOfState = lastChangeOfState;

            return View();
        }

        [HttpPost]
        public JsonResult RefreshChangeOfState(String name, Int32 batchSize, DateTime? startTime, DateTime? endTime)
        {
            dynamic reusult;
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                {
                    var factory = new NhRepositoryFactory(context);
                    var repository = factory.Create<LocationChangeOfStateRepository>();

                    reusult = repository.Create(name, batchSize, startTime, endTime);

                    unitOfWork.Commit();
                }
            }


            return Json(reusult);
        }
    }
}
