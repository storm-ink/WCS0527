using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.EventBus;
using Wcs.Framework.Events;

namespace Wcs.App.Plugins.ArchivedTaskManager
{
    [WcsPluginInfo(typeof(ArchivedTaskManagerPlugin), "归档任务查询", "Sineva", "2022年6月", "", true, "任务管理", "执行中任务", 1, 1, 1)]
    public class ArchivedTaskManagerPlugin : Wcs.WcsPlugin
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "历史任务";
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmMain frmMain;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\历史任务\\查看")]//20231113
        void tsmi_Click(object sender, EventArgs e)
        {
            try
            {
                this.Context.Application.MainForm.OpenNewForm<frmMain>("历史任务");
            }
            catch (System.Security.SecurityException securityException)
            {
                _logger.Error1(securityException, this);
                XtraMessageBox.Show(securityException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Security.SecurityException)
                {
                    XtraMessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _logger.Error1(ex, this);
            }
        }
    }
}
