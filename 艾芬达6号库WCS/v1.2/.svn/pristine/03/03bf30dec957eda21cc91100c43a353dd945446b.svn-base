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
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index( bool? isReturned)
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
        [HttpPost]
        public ActionResult Index_List(UserSpec spec)
        {
            try
            {
                NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());

                PageInfo pageInfo = Request.GetPageInfo();

                int totalItemCount;
                var list = repositoryFactory.Create<UserRepositoryBase>().SelectSatisfying<User>(
                    spec,
                    q=>q.OrderBy(x => x.Id),
                    pageInfo.StartItemIndex,
                    pageInfo.PageSize,
                    out totalItemCount
                    );
                pageInfo.TotalItemCount = totalItemCount;
                pageInfo.RoundPageIndex();

                ViewData.Model = new PagedList.StaticPagedList<User>(list,pageInfo.PageIndex,pageInfo.PageSize,pageInfo.TotalItemCount);

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


        private void PutAllRolesIntoViewBag()
        {
            NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
            ViewBag.Roles = repositoryFactory.Create<RoleRepositoryBase>().SelectSatisfying(null).OrderBy(x => x.RoleName).ToList();
        }


        private void UpdateRoles(User user, FormCollection form, NhRepositoryFactory repositoryFactory)
        {

            user.Roles.Clear();

            if (user.UserName == "admin")
            {
                Role adminRole = repositoryFactory.Create<RoleRepositoryBase>().GetByName("admins");
                user.AddToRole(adminRole);
            }

            if (!String.IsNullOrWhiteSpace(form["Roles"]))
            {
                if (user == null)
                {
                    throw new ApplicationException("用户不存在");
                }

                foreach (var item in form["Roles"].Split(','))
                {
                    int roleId = int.Parse(item);
                    Role role = repositoryFactory.Create<RoleRepositoryBase>().Load(roleId);
                    user.AddToRole(role);
                }
            }
        }


        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult Create()
        {
            PutAllRolesIntoViewBag();
            return View();
        }

        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult Create(FormCollection form)
        {
            User m = new User();
            try
            {
                using (NhRepositoryContext context= NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork=context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        TryUpdateModel<User>(m, null, null, new string[] { "Id", "Roles", "Password", "PasswordSalt", "IsBuiltIn" }, form);

                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException(ModelState.ToMessage());
                        }

                        m.PasswordSalt = Guid.NewGuid().ToString();
                        m.Password = UserService.HashPassword(Request["Password"], m.PasswordSalt);


                        UpdateRoles(m, form, repositoryFactory);

                        repositoryFactory.Create<UserRepositoryBase>().Add(m);


#pragma warning disable 0164
                    commit:
#pragma warning restore 0164

                        unitOfWork.Commit();

                    }
                }
                string js = "backToList('/users/create');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {
                this.HttpContext.GetOsivAttribute().Open(this.HttpContext);
                PutAllRolesIntoViewBag();

                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch(Exception ex)
            {
                _logger.ErrorException("创建用户时出错", ex);

                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
            }
        }

        [HttpGet]
        [OpenSessionInViewFilter]
        public ActionResult Edit(int id)
        {

            NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(this.HttpContext.GetNhRepositoryContext());
            User m = repositoryFactory.Create<UserRepositoryBase>().Get<User>(id);
            m.Password = null;
            ViewData.Model = m;
            PutAllRolesIntoViewBag();
            return View();
        
        }

        [HttpPost]
        [OpenSessionInViewFilter(true)]
        public ActionResult Edit(int id,FormCollection form)
        {
            User m = null;
            try
            {
                using (NhRepositoryContext context=NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork=context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        m = repositoryFactory.Create<UserRepositoryBase>().Get<User>(id);

                        

                        if (m.IsBuiltIn)
                        {
                            TryUpdateModel<User>(m, null, null, new string[] { "Id", "UserName", "Roles", "Password", "PasswordSalt", "IsBuiltIn" }, form);
                        }
                        else
                        {
                            TryUpdateModel<User>(m, null, null, new string[] { "Id", "Roles", "Password", "PasswordSalt", "IsBuiltIn" }, form);
                        }


                        if (!ModelState.IsValid)
                        {
                            throw new UpdateModelException(ModelState.ToMessage());
                        }

                        if (!string.IsNullOrWhiteSpace(form["Password"]))
                        {
                            m.Password = UserService.HashPassword(form["Password"], m.PasswordSalt);
                        }
                        UpdateRoles(m, form, repositoryFactory);
                        //if (!String.IsNullOrWhiteSpace(form["Workshop"]))
                        //{
                        //    m.SetWorkshop(form["Workshop"]);
                        //}

                        repositoryFactory.Create<UserRepositoryBase>().Update(m);

                        unitOfWork.Commit();


                    }
                }
                string js = "backToList('/roles/edit');";
                return JavaScript(js);
            }
            catch (UpdateModelException)
            {
                this.HttpContext.GetOsivAttribute().Open(this.HttpContext);
                PutAllRolesIntoViewBag();
                ViewData.Model = m;
                return PartialView("Edit_Form");
            }
            catch(Exception ex)
            {
                _logger.ErrorException("编辑用户时出错", ex);
                return JavaScript("errorMsgShow('" + Server.HtmlEncode(ex.Message) + "')");
        
            }
        
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            User m = null;
            try
            {
                using (NhRepositoryContext context=NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork=context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);
                        m = repositoryFactory.Create<UserRepositoryBase>().Get<User>(id);
                        if (m != null)
                        {
                            if (m.IsBuiltIn)
                            {
                                throw new ApplicationException("内置用户不能删除");
                            }
                            repositoryFactory.Create<UserRepositoryBase>().Remove(id);
                        }
                        unitOfWork.Commit();
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {

                _logger.ErrorException("删除用户时出错，id#" + id, ex);
                return Json(new {success=false,msg="删除失败。"+ex.Message});
            }
        }

    }
}
