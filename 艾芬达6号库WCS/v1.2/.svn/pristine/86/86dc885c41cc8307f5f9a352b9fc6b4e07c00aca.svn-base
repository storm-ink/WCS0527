using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.EquipmentActionSchedulerManager
{
    [WcsPluginInfo(typeof(EquipmentActionSchedulerManagerPlugin), "动作序列管理", "Sineva", "2013年4月", "提供一个可视化界面用于管理设备需要执行的动作序列，用户可以直观的看到当前设备正在执行的、等待执行的任务", false, "设备管理", "设备任务管理", 2, 1, 0)]
    public class EquipmentActionSchedulerManagerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "动作序列管理";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmMain frmMain;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备任务管理\\动作序列管理")]
        void tsmi_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmMain>("动作序列管理");
        }
    }
}
