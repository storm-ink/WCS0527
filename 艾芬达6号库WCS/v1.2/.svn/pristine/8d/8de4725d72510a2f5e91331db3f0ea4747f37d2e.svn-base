using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;
using Wcs.Framework.EventBus;
using Wcs.Framework.Events;

namespace Wcs.App.Plugins.PreTaskManager
{
    [WcsPluginInfo(typeof(TaskManagerPlugin), "任务管理器", "Sineva", "2013年7月", "提供一个可视化界面用于管理任务，包括常用的删除、暂停、继续执行等操作。同时界面内还提供了一个跟踪任务状态变化日志显示的功能", true, "任务管理", "任务管理", 1, 0, 0)]
    public class PreTaskManagerPlugin : Wcs.WcsPlugin
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public override int Priority
        {
            get { return 1; }
        }


        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "当前任务";
            barButtonItem.Id = 2;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\查看")]
        void tsmi_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmMain>("当前任务");
        }
    }
}
