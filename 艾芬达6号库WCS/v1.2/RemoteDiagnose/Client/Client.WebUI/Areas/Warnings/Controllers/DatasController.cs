using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Areas.Warnings.Controllers
{
    public class DatasController : Controller
    {
        //
        // GET: /Warnings/Datas/

        [WmsOperation("报警.数据查看")]
        [OpenSessionInViewFilter]
        public ActionResult Index()
        {
            var context = HttpContext.GetNhRepositoryContext();
            var repositoryFactory = new NhRepositoryFactory(context);
            ViewBag.DeviceTypes = repositoryFactory.Create<WarningRepository>().GetDeviceTypes();

            return View();
        }

        [WmsOperation("报警.数据查看")]
        [HttpPost]
        public ActionResult Index_List(WarningSpec spec,Boolean? export, FormCollection form)
        {
            PageInfo pageInfo = Request.GetPageInfo();

            try
            {
                List<Warning> list;
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        int totalItemCount;
                        var factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<WarningRepository>();
                        if (export != true)
                        {
                            list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                            pageInfo.TotalItemCount = totalItemCount;
                            pageInfo.RoundPageIndex();
                        }
                        else
                        {
                            list = repository.SelectSatisfying(spec);
                        }

                        unitOfWork.Commit();
                    }
                }

                if (export != true)
                {

                    ViewData.Model = new PagedList.StaticPagedList<Warning>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                    ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                    returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    return PartialView();
                }
                else
                {
                    return File(System.Text.Encoding.UTF8.GetBytes(ExportExcelHelper.ExportAsExcel(list)),"text/xml","报警记录.xml");
                }


            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.ToString());
            }
        }
    }
}
