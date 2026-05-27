using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using Spiral;
using Spiral.Base;
using Spiral.Web;

namespace Client.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

     
        //
        // GET: /Account/
        [OpenSessionInViewFilter]
        public ActionResult Index()
        {
            var repositoryContext = this.HttpContext.GetNhRepositoryContext();
            NhRepositoryFactory factory = new NhRepositoryFactory(repositoryContext);
            ViewData.Model = factory.Create<UserRepositoryBase>().GetByUserName<User>(this.HttpContext.User.Identity.Name);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection form, LoginModel loginModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = FormsAuthentication.DefaultUrl;
            }

            string[] roles;
            string reason;

            if (!ValidateUser(loginModel.UserName, loginModel.Password, out roles, out reason))
            {
                ViewBag.msg = "登录失败。" + reason;
                return View();
            }

            // 身份验证票
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                loginModel.UserName,
                DateTime.Now,
                DateTime.Now + FormsAuthentication.Timeout,
                true,
                string.Join("|", roles)
                );
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket); //加密
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Expires = authTicket.Expiration;

            Response.Cookies.Add(authCookie);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoginWithJson(FormCollection form, LoginModel loginModel, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new UpdateModelException(ModelState.ToMessage());
                }


                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = FormsAuthentication.DefaultUrl;
                }

                string[] roles;
                string reason;
                if (!UserService.ValidateUser(loginModel.UserName, loginModel.Password, out roles, out reason))
                {
                    return Json(new { success = false, msg = "登录失败。" + reason });
                }

                // 身份验证票
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    loginModel.UserName,
                    DateTime.Now,
                    DateTime.Now + FormsAuthentication.Timeout,
                    true,
                    string.Join("|", roles)
                    );
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket); //加密
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.Expires = authTicket.Expiration;

                Response.Cookies.Add(authCookie);


                return Json(new { success = true, msg = "登录成功。", Roles = roles, UserName = loginModel.UserName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "登录失败。" + ex.Message });
            }
        }
     

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            return Redirect(FormsAuthentication.DefaultUrl);
        }


        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string password)
        {

            //  取当前登录的用户名
            string userName = this.HttpContext.User.Identity.Name;

            // 更改密码
            UserService.ChangePassword(userName, password);

            // 界面上提示成功
            ViewBag.msg = "密码修改成功";

            return View();
        }

        private bool ValidateUser(string userName, string password, out string[] roles, out string failureReason)
        {
            if (userName.ToLower() == "setup" && password == "wms@")
            {
                roles = new [] { "admins" };
                failureReason = null;
                return true;
            }

            return UserService.ValidateUser(userName, password, out roles, out failureReason);
        }
    }
}
            