using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;

namespace Wcs.App.Plugins.LogsViewer
{
    [WcsPluginInfo(typeof(LogsViewerPlugin), "日志查询", "Sineva", "2022年6月", "对日志数据进行检查、统计等操作。", false, "日志管理", "日志查询",5, 1, 0)]
    public class LogsViewerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "日志查询";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmiLogsViewerPlugin_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }


        void tsmiLogsViewerPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(typeof(LogsViewerPlugin).Assembly.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
