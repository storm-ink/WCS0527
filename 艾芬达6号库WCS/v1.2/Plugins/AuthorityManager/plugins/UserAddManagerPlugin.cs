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

namespace Wcs.App.Plugins.AuthorityManager
{
    [WcsPluginInfo(typeof(UserAddManagerPlugin), "权限管理", "Sineva", "2022年6月", "", true, "权限管理", "用户管理", 4, 1, 0)]
    public class UserAddManagerPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "添加新用户";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "权限管理\\用户管理\\添加用户")]
        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (frmUserEdit frm = new frmUserEdit(0))
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
