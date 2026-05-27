using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.DataAnalysis
{
    [WcsPluginInfo(typeof(DataAnalysisPlugin), "数据分析工具", "Sineva", "2014/3/31 11:26:23", "对设备运行数据进行相关分析、统计。", false, "", "", 0)]
    public class DataAnalysisPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            //如果需要添加到菜单项中，可使用以下注释的代码模板
            ToolStripMenuItem tsmiDataAnalysisPlugin = new ToolStripMenuItem("数据分析");
            tsmiDataAnalysisPlugin.Click += tsmiDataAnalysisPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiDataAnalysisPlugin);

            return base.Initialization(context);
        }

        void tsmiDataAnalysisPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(typeof(DataAnalysisPlugin).Assembly.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
