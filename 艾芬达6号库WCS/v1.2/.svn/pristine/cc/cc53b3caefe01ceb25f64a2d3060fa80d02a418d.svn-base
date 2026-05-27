using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Client.WebUI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


        }


        public void Application_AuthenticateRequest()
        {
            ErpLoginHandler();

            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return;
            }

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket.Expired)
            {
                // 若票证过期，则验证失败
                return;
            }

            // 主体替换
            string[] roles = ticket.UserData.Split(new char[] { '|' });
            FormsIdentity id = new FormsIdentity(ticket);
            Context.User = new GenericPrincipal(id, roles);

            // 更新票证
            FormsAuthenticationTicket ticketRenewed = FormsAuthentication.RenewTicketIfOld(ticket);
            if (ticketRenewed != ticket)
            {
                string encryptedTicket = FormsAuthentication.Encrypt(ticketRenewed); //加密
                HttpCookie newAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                newAuthCookie.Expires = ticketRenewed.Expiration;
                Response.Cookies.Set(newAuthCookie);
            }

        }

        void ErpLoginHandler()
        {
            if (Context.Request.UrlReferrer == null)
            {
                return;
            }

            if (!Context.Request.UrlReferrer.Host.Contains("192.168.1.236")
                && !Context.Request.UrlReferrer.Host.Contains("61.190.13.166"))
            {
                return;
            }

            String userName = Context.Request.QueryString["userName"];
            if (String.IsNullOrWhiteSpace(userName))
            {
                return;
            }

            // 身份验证票
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now + FormsAuthentication.Timeout,
                true,
                "erp"
                );
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket); //加密
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.Expires = authTicket.Expiration;
            Context.Response.Cookies.Add(authCookie);
            Context.Request.Cookies.Add(authCookie);
        }
    }
}