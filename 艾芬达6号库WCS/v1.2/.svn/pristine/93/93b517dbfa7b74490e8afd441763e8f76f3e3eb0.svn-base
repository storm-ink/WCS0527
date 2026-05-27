using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.Reports
{
    [WcsPluginInfo(typeof(ReportsPlugin), "数据报表", "Sineva", "2014/4/28 9:01:33", "提供设备运行数据报表、报警统计、任务运行时间统计等报表", false)]
    public class ReportsPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            //如果需要添加到菜单项中，可使用以下注释的代码模板
            ToolStripMenuItem tsmiReportsPlugin = new ToolStripMenuItem("数据报表");
            tsmiReportsPlugin.Click += tsmiReportsPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.View).DropDownItems.Add(tsmiReportsPlugin);

            var 堆垛机报警Item = tsmiReportsPlugin.DropDownItems.Add("堆垛机报警统计");
            堆垛机报警Item.Click += (object sender, EventArgs e) =>
            {
                lauch("堆垛机报警统计");
            };

            var 输送线报警统计Item = tsmiReportsPlugin.DropDownItems.Add("输送线报警统计");
            输送线报警统计Item.Click += (object sender, EventArgs e) =>
            {
                lauch("输送线报警统计");
            };


            var 主任务执行时间统计Item = tsmiReportsPlugin.DropDownItems.Add("主任务执行时间统计");
            主任务执行时间统计Item.Click += (object sender, EventArgs e) =>
            {
                lauch("主任务执行时间统计");
            };

            var 设备任务执行时间统计Item = tsmiReportsPlugin.DropDownItems.Add("设备任务执行时间统计");
            设备任务执行时间统计Item.Click += (object sender, EventArgs e) =>
            {
                lauch("设备任务执行时间统计");
            };

            return base.Initialization(context);
        }

        void tsmiReportsPlugin_Click(object sender, EventArgs e)
        {
            lauch("");
        }

        void lauch(string report)
        {
            try
            {
                Process.Start(typeof(ReportsPlugin).Assembly.Location,report);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
