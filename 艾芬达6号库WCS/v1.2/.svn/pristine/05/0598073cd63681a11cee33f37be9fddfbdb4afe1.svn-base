using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spiral;
using NLog;
using Spiral.Base;
using Spiral.Web;

namespace Client.WebUI.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Authorize(Roles = "admins")]
    public class RolesController : Controller
    {
        //
        // GET: /Roles/
        private static Logger _logger = LogManager.GetCurrentClassLogger();

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


        [Spiral.Web.WmsOperation("角色.查看")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public ActionResult Index_List(RoleSpec spec)
        {
            try
            {
                NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
                PageInfo pageInfo = Request.GetPageInfo();

                int totalItemCount;
                var list = repositoryFactory.Create<RoleRepositoryBase>().SelectSatisfying<Role>(
                    spec,
                    q => q.OrderBy(x => x.Id),
                    pageInfo.StartItemIndex,
                    pageInfo.PageSize,
                    out totalItemCount
                    );
                pageInfo.TotalItemCount = totalItemCount;
                pageInfo.RoundPageIndex();

                ViewData.Model = new PagedList.StaticPagedList<Role>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);


                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                return PartialView();
            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.Message);
            }

        }


        [Spiral.Web.WmsOperation("角色.添加")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Spiral.Web.WmsOperation("角色.添加")]
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            Role m = new Role();
            try
            {

                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);

                        TryUpdateModel<Role>(m, null, null, new string[] { "Id", "IsBuiltIn" }, form);
                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException(ModelState.ToMessage());
                        }

                        repositoryFactory.Create<RoleRepositoryBase>().Add(m);


#pragma warning disable 0164
                    commit:
#pragma warning restore 0164

                        unitOfWork.Commit();

                    }
                }
                string js = "backToList('/roles/create');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {

                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch (Exception ex)
            {
                _logger.ErrorException("创建角色时出错", ex);

                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
            }

        }

          
        /// <summary>
        /// 编辑  内置不予编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [Spiral.Web.WmsOperation("角色.编辑")]
        [OpenSessionInViewFilter]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
            Role m = repositoryFactory.Create<RoleRepositoryBase>().Get<Role>(id);
            ViewData.Model = m;
            return View();

        }

         [Spiral.Web.WmsOperation("角色.编辑")]
        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult Edit(int id, FormCollection form)
        {
            Role m = null;
            try
            {
                using (NhRepositoryContext context=NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork=context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        m = repositoryFactory.Create<RoleRepositoryBase>().Get<Role>(id);
                        // 判断是否为内置 内置不可修改
                        if (m.IsBuiltIn)
                        {
                             TryUpdateModel<Role>(m, null, null, new string[] { "Id", "RoleName", "IsBuiltIn" }, form);
                        }
                        else
                        {
                            TryUpdateModel<Role>(m, null, null, new string[] { "Id", "IsBuiltIn" }, form);
                        }
                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException();
                        }
                        repositoryFactory.Create<RoleRepositoryBase>().Update(m);

                        unitOfWork.Commit();
                    }
                }
                string js = "backToList('/roles/edit');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {

                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch( Exception ex)
            {
                _logger.ErrorException("编辑角色时出错",ex);

                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
            
            }

        }
         [Spiral.Web.WmsOperation("角色.删除")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            Role m = null;
            try
            {
                using (NhRepositoryContext context=NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork=context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        m = repositoryFactory.Create<RoleRepositoryBase>().Get(id);

                        if (m.IsBuiltIn)
                        {
                            throw new ApplicationException("内置角色不能删除");
                        }
                        
                        repositoryFactory.Create<RoleRepositoryBase>().Remove(id);

                        unitOfWork.Commit();


                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                _logger.ErrorException("删除角色时出错，id: " + id, ex);
                return Json(new { success = false, msg = "删除失败。" + ex.Message });
            }
        
        }

    }
}