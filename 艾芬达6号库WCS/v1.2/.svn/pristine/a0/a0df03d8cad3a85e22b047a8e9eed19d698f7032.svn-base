using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.AuthorityManager
{
    public class UserService
    {
        /// <summary>
        /// 在用户登录之后发生
        /// </summary>
        public static event EventHandler AfterLogin;
        /// <summary>
        /// 在用户注销之后发生
        /// </summary>
        public static event EventHandler AfterLogout;

        public static string HashPassword(string password, string salt)
        {
            return GetMd5Hash(salt + password + salt);
        }

        public static string GetMd5Hash(string input)
        {
            using (MD5 md = MD5.Create())
            {
                byte[] buffer = md.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            string x = GetMd5Hash(input);
            return (StringComparer.OrdinalIgnoreCase.Compare(x, hash) == 0);
        }

        static Logger _logger = LogManager.GetCurrentClassLogger();
        internal static Wcs.Security.WcsPrincipal Login(User user)
        {
            var roles = user.Roles.Select(x => x.RoleName).ToArray();
            var operations = user.Roles
                .SelectMany(x => x.Operations)
                .Select(x => x)
                .GroupBy(x => x)
                .Select(x => x.Key)
                .ToArray();

            var id = new Wcs.Security.WcsIdentity(user.UserName,user.RealName, roles, operations);
            var policy = new Wcs.Security.WcsPrincipal(id);

            Wcs.Security.WcsPrincipal.CurrentPrincipal = policy;

            if (!policy.IsEmpty)
            {
                _logger.Info1(string.Format("{0} 已登录成功。", policy.Identity.Name), typeof(UserService));
            }

            if (AfterLogin != null)
            {
                AfterLogin.Invoke(null, EventArgs.Empty);
            }

            return policy;
        }

        public static void Logout(Wcs.IWcsApplication app)
        {
            try
            {
                while (System.Windows.Forms.Application.OpenForms.Cast<System.Windows.Forms.Form>().Any(x => x!=app.MainForm))
                {

                    var frm = System.Windows.Forms.Application.OpenForms.Cast<System.Windows.Forms.Form>().FirstOrDefault(x => x != app.MainForm);
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        try
                        {
                            frm.Close();

                            frm.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.Error1(ex, null);
                        }
                    });

                    frm.Invoke(mi);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, null);
            }

            String userName=Wcs.Security.WcsPrincipal.CurrentPrincipal.Identity.Name;
            //设置凭证
            Wcs.Security.WcsPrincipal.CurrentPrincipal = Wcs.Security.WcsPrincipal.Empty;

            if (!String.IsNullOrWhiteSpace(userName))
            {
                app.Logger.Info1(string.Format("{0} 已注销登录。", userName), typeof(UserService));
            }

            if (AfterLogout != null)
            {
                AfterLogout.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
