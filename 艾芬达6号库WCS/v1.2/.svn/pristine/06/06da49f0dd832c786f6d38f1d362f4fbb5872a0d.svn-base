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

namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    [WcsPluginInfo(typeof(ArchivedPreTaskManagerPlugin), "任务管理器", "Sineva", "2013年7月", "", true, "任务管理", "计划中任务", 1, 0, 1)]
    public class ArchivedPreTaskManagerPlugin : Wcs.WcsPlugin
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
            barButtonItem.Caption = "历史计划";
            barButtonItem.Id = 2;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\历史计划任务\\查看")]
        void tsmi_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmMain>("历史计划");
        }
    }
}
