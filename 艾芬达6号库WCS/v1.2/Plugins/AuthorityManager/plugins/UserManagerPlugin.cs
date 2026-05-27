using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Linq;
using Wcs;
using System.Runtime.InteropServices;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

namespace Wcs.App.Plugins.AuthorityManager
{
    [WcsPluginInfo(typeof(UserManagerPlugin), "权限管理", "Sineva", "2022年6月", "", true, "权限管理", "用户管理", 4, 1, 1)]
    public class UserManagerPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "用户管理";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            var currentUserLabel = context.Application.Status.ItemLinks.Add(new BarHeaderItem() { Caption = "当前用户：<未登录>" });
            UserService.AfterLogin += (sender, e) =>
            {
                try
                {
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        var id = (Wcs.Security.WcsIdentity)Wcs.Security.WcsPrincipal.CurrentPrincipal.Identity;
                        currentUserLabel.Caption = String.Format("当前用户：{0}     角色：{1}      登录时间：{2}"
                            , id.RealName
                            , String.Join(",", id.RoleNames)
                            , DateTime.Now);
                    });

                    var mainFrm = (RibbonForm)context.Application.MainForm;
                    mainFrm.Invoke(mi);
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }

            };
            UserService.AfterLogout += (sender, e) =>
            {
                try
                {
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        currentUserLabel.Caption = "当前用户：<未登录>";
                    });

                    var mainFrm = (RibbonForm)context.Application.MainForm;
                    mainFrm.Invoke(mi);
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            };

            Int32 userSessionTimeout = 1000 * 60;
            var userSessionTimeoutSetting = System.Configuration.ConfigurationSettings.AppSettings["UserSessionTimeout"];
            if (userSessionTimeoutSetting != null)
            {
                if (!Int32.TryParse(userSessionTimeoutSetting, out userSessionTimeout))
                {
                    userSessionTimeout = 1000 * 60;
                }
            }

            if (userSessionTimeout < 1000 * 30)
            {
                _logger.Warn1(String.Format("用户登录超时时间为 {0} 毫秒，小于 {1} 毫秒,已修正为 {1} 毫秒", userSessionTimeout, 1000 * 30), this);

                userSessionTimeout = 1000 * 30;
            }
            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (Wcs.Security.WcsPrincipal.CurrentPrincipal.IsEmpty)
                    {
                        continue;
                    }

                    var idleTime = getIdleTime();
                    if (idleTime.TotalMilliseconds >= userSessionTimeout)
                    {
                        _logger.Info1(string.Format("{0} 在登录后已 {1} 没有进行任务操作，系统将自动注销该登录凭证。", Wcs.Security.WcsPrincipal.CurrentPrincipal.Identity.Name, idleTime), this);

                        UserService.Logout(context.Application);
                    }
                }
            });

            return base.Initialization(context);
        }


        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\用户管理\\用户管理")]
        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Context.Application.MainForm.OpenNewForm<frmUsers>("用户管理");
            }
            catch (System.Security.SecurityException securityException)
            {
                _logger.Error1(securityException, this);
                MessageBox.Show(securityException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Security.SecurityException)
                {
                    MessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _logger.Error1(ex, this);
            }
        }

        #region xxx

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        TimeSpan getIdleTime()
        {
            long millisecondis = 0;
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            if (!GetLastInputInfo(ref vLastInputInfo))
            {
                millisecondis = 0;
            }
            else
            {
                millisecondis = Environment.TickCount - (long)vLastInputInfo.dwTime;
            }

            return TimeSpan.FromMilliseconds(millisecondis);
        }
        #endregion
    }
}
