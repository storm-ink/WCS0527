using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.Conveyor;
using Spiral;
namespace Client.WebUI.Areas.Conveyor.Controllers
{
    public class HoldSignalController : Controller
    {

        [WmsOperation("输送线.占位信号")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index_List(HoldSignalSpec spec, FormCollection form)
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
                        var repository = factory.Create<ConveyorRepository<HoldSignal>>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<HoldSignal>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

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

    }
}
