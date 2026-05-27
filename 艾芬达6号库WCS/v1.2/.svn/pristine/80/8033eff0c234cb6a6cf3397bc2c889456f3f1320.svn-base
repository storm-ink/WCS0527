using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wcs;

namespace Wcs.App.Plugins.Tools
{
    [WcsPluginInfo(typeof(AutoArchiveTaskPlugin), "根据任务来源和类型归档", "yang", "2019/10/23 10:46:23", "<说明>", false, "工具箱", "系统工具", 3, 0, 0)]
    public class AutoArchiveTaskPlugin : WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "任务归档工具";
            barButtonItem.Id = 2;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}_archivedTask";
            barButtonItem.ItemClick += taskArchiveItem_click;

            var barButtonItem1 = new BarButtonItem();
            barButtonItem1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem1.Caption = "路径工具";
            barButtonItem1.Id = 1;
            barButtonItem1.Name = $"barBtn_{this.GetType().Name.ToLower()}_route";
            barButtonItem1.ItemClick += RouteTestItem_click;

            this.barButtonItems = new BarButtonItem[] { barButtonItem, barButtonItem1 };

            return base.Initialization(context);
        }


        frmRouteTest frmRouteTest;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "工具箱\\系统工具\\路径工具")]
        private void RouteTestItem_click(object sender, EventArgs e)
        {
            if (frmRouteTest != null && !frmRouteTest.IsDisposed && !frmRouteTest.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    frmRouteTest.WindowState = FormWindowState.Maximized;
                    frmRouteTest.Focus();
                    frmRouteTest.Activate();
                    return;
                }
            }
            frmRouteTest = new frmRouteTest();
            frmRouteTest.Show();
            frmRouteTest.Activate();
        }

        frmAutoArchiveTask frmAutoArchiveTask;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "工具箱\\系统工具\\任务归档工具")]
        void taskArchiveItem_click(object sender, EventArgs e)
        {
            if (frmAutoArchiveTask != null && !frmAutoArchiveTask.IsDisposed && !frmAutoArchiveTask.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    frmAutoArchiveTask.WindowState = FormWindowState.Maximized;
                    frmAutoArchiveTask.Focus();
                    frmAutoArchiveTask.Activate();
                    return;
                }
            }
            frmAutoArchiveTask = new frmAutoArchiveTask();
            frmAutoArchiveTask.Show();
            frmAutoArchiveTask.Activate();

        }


        //任务类型_任务方向设置 _frmTaskDirectionSetting;
        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "小工具\\任务类型设置")]
        //void taskDirectionSettingItem_click(object sender, EventArgs e)
        //{
        //    if (_frmTaskDirectionSetting != null && !_frmTaskDirectionSetting.IsDisposed && !_frmTaskDirectionSetting.Disposing)
        //    {
        //        foreach (Form form in Application.OpenForms)
        //        {
        //            _frmTaskDirectionSetting.WindowState = FormWindowState.Maximized;
        //            _frmTaskDirectionSetting.Focus();
        //            _frmTaskDirectionSetting.Activate();
        //            return;
        //        }
        //    }
        //    _frmTaskDirectionSetting = new 任务类型_任务方向设置();
        //    _frmTaskDirectionSetting.Show();
        //    _frmTaskDirectionSetting.Activate();

        //}
    }
}
