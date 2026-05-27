using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;

namespace Wcs.App.Plugins.LogTrace
{
    [WcsPluginInfo(typeof(LogTracePlugin), "日志跟踪", "Sineva", "2014/3/1 10:13:28", "提供一个可跟踪显示实时日志信息的窗口。", false, "日志管理", "实时日志", 5, 0, 1)]
    public class LogTracePlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "日志跟踪";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmiLogTracePlugin_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmLogTrace_TraceWindow frmMain;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "日志管理\\实时日志\\日志跟踪")]
        void tsmiLogTracePlugin_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmLogTrace_TraceWindow>("实时日志");
        }
    }
}
