using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.AuthorityManager
{
    [WcsPluginInfo(typeof(UserManagerPlugin), "权限管理", "Sineva", "2022年6月", "", true, "欢迎页", "欢迎页", 0, 0, 0)]
    public class LoginManagerPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "用户登录";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };
            
            UserService.AfterLogin += (sender, e) =>
            {
                try
                {
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        barButtonItem.Caption = "注销登录";
                        if (File.Exists("./Ico/logout_32.svg"))
                            barButtonItem.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./Ico/logout_32.svg");
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
                        barButtonItem.Caption = "用户登录";
                        if (File.Exists("./Ico/login_32.svg"))
                            barButtonItem.ImageOptions.SvgImage = DevExpress.Utils.Svg.SvgImage.FromFile("./Ico/login_32.svg");
                    });

                    var mainFrm = (RibbonForm)context.Application.MainForm;
                    mainFrm.Invoke(mi);
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            };

            return base.Initialization(context);
        }

        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                var barButtonItem = (BarButtonItem)e.Item;
                if (barButtonItem.Caption == "用户登录")
                {
                    using (frmLogin frm = new frmLogin())
                    {
                        frm.ShowDialog();
                    }
                }
                else
                {
                    var result = MessageBox.Show("确定退出登录？", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        UserService.Logout(this.Context.Application);
                }
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
    }
}
