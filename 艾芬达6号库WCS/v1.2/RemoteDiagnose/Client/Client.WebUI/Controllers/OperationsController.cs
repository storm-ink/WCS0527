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
    [Authorize(Roles="admins")]
    public class OperationsController : Controller
    {
        //
        // GET: /Opezations/
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index(bool? isReturned)
        {
            ReturnInfo returnInfo = new ReturnInfo(Session,this.GetType().ToString());

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


        [Spiral.Web.WmsOperation("权限.查看")]
        [OpenSessionInViewFilter]
        [HttpPost]
        public ActionResult Index_List(OperationSpec spec)
        {
            try
            {
                NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
                PageInfo pageInfo = Request.GetPageInfo();

                int totalItemCount;
                var list = repositoryFactory.Create<OperationRepository>().SelectSatisfying<Operation>(
                    spec,
                    q=>q.OrderBy(x=>x.Code),
                    pageInfo.StartItemIndex,
                    pageInfo.PageSize,
                    out totalItemCount
                    );
                pageInfo.TotalItemCount = totalItemCount;
                pageInfo.RoundPageIndex();

                ViewData.Model = new PagedList.StaticPagedList<Operation>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize,null);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
              
                return PartialView();

            }
            catch (Exception ex)
            {

                return Content("错误"+ex.Message);
            }
          
        }


        private void PutAllRolesIntoViewBag()
        {
            NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
            ViewBag.Roles = repositoryFactory.Create<RoleRepositoryBase>().SelectSatisfying(null).OrderBy(x => x.RoleName).ToList();
        }


        private void UpdateRoles(Operation operation, FormCollection form, NhRepositoryFactory repositoryFactory)
        {

            operation.AllowedRoles.Clear();
            var adminRole = repositoryFactory.Create<RoleRepositoryBase>().GetByName("admins");
            operation.AllowedRoles.Add(adminRole);

            if (!String.IsNullOrWhiteSpace(form["Roles"]))
            {
               
                foreach (var item in form["Roles"].Split(','))
                {
                    int roleId = int.Parse(item);
                    Role role = repositoryFactory.Create<RoleRepositoryBase>().Load(roleId);
                    operation.AllowedRoles.Add(role);
                }
            }
        }


        [Spiral.Web.WmsOperation("权限.设置")]
        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult SetPermissions(int id)
        {
            NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
            Operation m = repositoryFactory.Create<OperationRepository>().Get<Operation>(id);

            ViewData.Model = m;
            PutAllRolesIntoViewBag();
            return View();
        }

          [Spiral.Web.WmsOperation("权限.设置")]
        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult SetPermissions(int id, FormCollection form)
        {
            Operation m = null;
            try
            {
                using (NhRepositoryContext context=NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        m = repositoryFactory.Create<OperationRepository>().Get<Operation>(id);

                        TryUpdateModel<Operation>(m, null, null, new string[] { "Id" }, form);

                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException(ModelState.ToMessage());
                        }

                        UpdateRoles(m, form, repositoryFactory);

                        repositoryFactory.Create<OperationRepository>().Update(m);

                        unitOfWork.Commit();

                    }
                }

                WmsOperationAttribute.RemoveCache(this.HttpContext.Cache);

                string js = "backToList('/roles/ SetPermissions');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {
                this.HttpContext.GetOsivAttribute().Open(this.HttpContext);
                PutAllRolesIntoViewBag();
                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch (Exception ex)
            {
                _logger.ErrorException("编辑Operation时出错", ex);
                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");

            }
        
        }

    }
}
