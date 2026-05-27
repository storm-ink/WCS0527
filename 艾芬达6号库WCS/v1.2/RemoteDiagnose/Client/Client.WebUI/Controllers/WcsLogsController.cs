using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Controllers
{
    public class WcsLogsController : Controller
    {
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

        [HttpPost]
        public ActionResult Index_List(Client.Logs.WcsLogSpec spec, FormCollection form)
        {
            PageInfo pageInfo = Request.GetPageInfo();

            try
            {
                 using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        var repositoryFactory = new NhRepositoryFactory(context);

                        int totalItemCount;
                        var list = repositoryFactory.Create<Client.Logs.WcsLogDiagnosisDataRepository>()
                            .SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);

                        ViewData.Model = new PagedList.StaticPagedList<Client.Logs.WcsLogDiagnosisData>(list, pageInfo.PageIndex, pageInfo.PageSize, totalItemCount);

                        unitOfWork.Commit();
                    }
                }

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (String.Equals(Request["mode"], "bythread", StringComparison.InvariantCultureIgnoreCase))
                {
                    return PartialView("Index_List_bythread");
                }
                return PartialView();


            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.ToString());
            }
        }

        public ActionResult Details(int id)
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var repositoryFactory = new NhRepositoryFactory(context);

                    var log = repositoryFactory.Create<Client.Logs.WcsLogDiagnosisDataRepository>().Get(id);

                    ViewData.Model = log;

                    unitOfWork.Commit();
                }
            }

            return View();
        }

    }
}
