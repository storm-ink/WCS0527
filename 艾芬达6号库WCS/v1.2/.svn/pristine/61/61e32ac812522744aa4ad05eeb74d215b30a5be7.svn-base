using DevExpress.XtraBars;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.HomePage
{
    [WcsPluginInfo(typeof(HomePagePlugin), "欢迎页", "Sineva", "2022年6月", "", true, "欢迎页", "欢迎页", 0, 0, 0)]
    public class HomePagePlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        public override bool Initialization(Wcs.WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "项目概况";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        private void tsmi_Click(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Context.Application.MainForm.OpenNewForm<frmHomePage>("项目概况");
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