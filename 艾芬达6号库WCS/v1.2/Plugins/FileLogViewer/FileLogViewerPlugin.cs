using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Wcs;

namespace Wcs.App.Plugins.LogsViewer
{
    [WcsPluginInfo(typeof(FileLogViewerPlugin), "文件日志", "Sineva", "2022年6月", "对日志数据进行检查、统计等操作。", false, "日志管理", "文件历史日志", 5, 2, 1)]
    public class FileLogViewerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "文件日志";
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
                System.Diagnostics.Process.Start("explorer.exe", LogWriteToFileHelper.Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
