using DevExpress.XtraBars;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.AuthorityManager
{
    [WcsPluginInfo(typeof(UserManagerPlugin), "权限管理", "Sineva", "2022年6月", "", true, "欢迎页", "欢迎页", 0, 0, 1)]
    public class ChangePasswordManagerPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "修改密码";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (frmChangePassword frm = new frmChangePassword())
                {
                    frm.ShowDialog();
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
