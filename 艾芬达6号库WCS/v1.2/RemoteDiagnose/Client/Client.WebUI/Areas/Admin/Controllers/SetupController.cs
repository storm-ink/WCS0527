using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Configuration;
using NHibernate.Linq;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using System.IO;
using Spiral;
using Spiral.Web;
using Spiral.Base;

namespace WebUI.Areas.Admin.Controllers
{
    public class SetupController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportSchema()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportSchema(FormCollection form)
        {

            try
            {
                if (WebConfigurationManager.AppSettings["允许创建数据库表"] != "T")
                {
                    throw new ApplicationException("不允许创建数据库表。");
                }

                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        if (Setup.IsSqlServer())
                        {
                            Setup.CreateSchema(context.Session, "wms", "dbo");
                        }

                        unitOfWork.Commit();
                    }
                }

                Setup.ExportSchema();

                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);

                        Role role = new Role();
                        role.RoleName = "admins";
                        role.IsBuiltIn = true;
                        repositoryFactory.Create<RoleRepositoryBase>().Add(role);

                        {
                            Client.User user = new Client.User();
                            user.UserName = "admin";
                            user.RealName = "admin";
                            user.IsBuiltIn = true;
                            user.PasswordSalt = Guid.NewGuid().ToString();
                            user.Password = UserService.HashPassword("admin888", user.PasswordSalt);
                            user.AddToRole(role);
                            repositoryFactory.Create<UserRepositoryBase>().Add(user);
                        }

                        unitOfWork.Commit();

                    }
                }


                TempData["msg"] = "成功";
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.ToString();
            }
            return RedirectToAction("ExportSchema");
        }


        public ActionResult CreateRacks()
        {
            ViewBag.Title = "生成货架";
            return View();
        }


        public ActionResult ImportOperations()
        {
            ViewData.Model = WmsOperationsHelper.GetAllOperationCodes();
            return View();
        }

        [HttpPost]
        public ActionResult ImportOperations(FormCollection form)
        {
            try
            {
                int count = 0;
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        NhRepositoryFactory repositoryFactory = new NhRepositoryFactory(context);

                        var list = WmsOperationsHelper.GetAllOperationCodes();
                        var adminRole = repositoryFactory.Create<RoleRepositoryBase>().GetByName("admins");

                        var repo = repositoryFactory.Create<OperationRepository>();
                        var existing = repositoryFactory.Create<OperationRepository>().SelectSatisfying(null);
                        foreach (var item in list)
                        {
                            if (existing.Any(x => String.Equals(x.Code, item, StringComparison.OrdinalIgnoreCase)) == false)
                            {
                                Operation op = new Operation();
                                op.Code = item;
                                op.Description = item;
                                op.AllowedRoles.Add(adminRole);
                                repo.Add(op);
                                count++;

                            }
                        }


                        unitOfWork.Commit();

                    }
                }
                WmsOperationAttribute.RemoveCache(this.HttpContext.Cache);
                TempData["msg"] = string.Format("导入了 {0} 个操作", count);

            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }

            return RedirectToAction("ImportOperations");

        }
    }


}
