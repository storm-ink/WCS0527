using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spiral;
using Wms;
using Wms.App;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    public class MaterialsController : Controller
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


        [OpenSessionInViewFilter]
        public ActionResult Index_List(CommonMaterialSpec spec)
        {
            try
            {
                Wms.WmsRepositories repositories = OpenSessionInViewFilterAttribute.Current.WmsRepositories;

                PageInfo pageInfo = Request.GetPageInfo();


                int totalItemCount;
                var list = repositories.MaterialRepository.SelectSatisfying(spec, q => q.OrderBy(x => x.Id), pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                pageInfo.TotalItemCount = totalItemCount;
                pageInfo.RoundPageIndex();

                ViewData.Model = new PagedList.StaticPagedList<Material>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                return PartialView();
            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult Create(FormCollection form)
        {
            Material m = new Material();
            try
            {

                using (NhRepositoryContext context = WmsApplication.RepositoryContextFactory.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        context.Session.CreateSQLQuery("set XACT_ABORT ON").ExecuteUpdate();

                        WmsRepositories repositories = new WmsRepositories(context);


                        TryUpdateModel<Material>(m, string.Empty, null, new string[] { "Id" });
                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException();
                        }

                        repositories.MaterialRepository.Add(m);

    commit:
                        unitOfWork.Commit();
                        context.Session.CreateSQLQuery("set XACT_ABORT OFF").ExecuteUpdate();

                    }
                }      
      
                string js = "backToList('/materials/create');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {
                //OpenSessionInViewFilterAttribute.Current.Open(this.HttpContext);
                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch (Exception ex)
            {
                LoggerLocator.WmsApplication.ErrorException("创建物料时出错", ex);

                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
            }
        }

        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult Edit(int id)
        {
            ViewData.Model = OpenSessionInViewFilterAttribute.Current.WmsRepositories.MaterialRepository.Get(id);
            return View();
        }

        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult Edit(int id, FormCollection form)
        {
            try
            {

                using (NhRepositoryContext context = WmsApplication.RepositoryContextFactory.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        context.Session.CreateSQLQuery("set XACT_ABORT ON").ExecuteUpdate();

                        WmsRepositories repositories = new WmsRepositories(context);


                        Material m = repositories.MaterialRepository.Get(id);
                        TryUpdateModel<Material>(m, string.Empty, null, new string[] { "Id" });
                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException();
                        }

                        repositories.MaterialRepository.Update(m);

    commit:
                        unitOfWork.Commit();
                        context.Session.CreateSQLQuery("set XACT_ABORT OFF").ExecuteUpdate();

                    }
                }      
      
                string js = "backToList('/materials/edit');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {
                OpenSessionInViewFilterAttribute.Current.Open(this.HttpContext);

                ViewData.Model = OpenSessionInViewFilterAttribute.Current.WmsRepositories.MaterialRepository.Get(id);
                return PartialView("Edit_Form");
            }
            catch (Exception ex)
            {                    
                LoggerLocator.WmsApplication.ErrorException("更新物料时出错", ex);
                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                using (NhRepositoryContext context = WmsApplication.RepositoryContextFactory.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        context.Session.CreateSQLQuery("set XACT_ABORT ON").ExecuteUpdate();

                        WmsRepositories repositories = new WmsRepositories(context);


                        repositories.MaterialRepository.Remove(id);

    commit:
                        unitOfWork.Commit();
                        context.Session.CreateSQLQuery("set XACT_ABORT OFF").ExecuteUpdate();

                    }
                }      
      
                return Content(JsonConvert.SerializeObject(new { success = true }));
            }
            catch (Exception ex)
            {
                // 错误日志
                LoggerLocator.WmsApplication.ErrorException("删除物料时出错，id: " + id, ex);
                return Content(JsonConvert.SerializeObject(new { success = false }));
            }

        }
    }
}
