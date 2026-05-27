using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.DeviceManager
{
    [WcsPluginInfo(typeof(DeviceManagerPlugin), "设备管理器", "Sineva", "2022年6月", "以可视化的形式展现各类设备的当前状态信息，对分析任务和设备的异常情况有很大帮助", false, "设备管理", "设备管理", 2, 0, 0)]
    public class DeviceManagerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "设备管理";
            barButtonItem.Id = 1;
            barButtonItem.Name = "barBtn_addNewRole";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmMain frmMain;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\查看")]
        void tsmi_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmMain>("设备管理器");
        }
    }
}
