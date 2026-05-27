using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs;
using System.Windows.Forms;
using System.Diagnostics;

namespace Wcs.App.Plugins.堆垛机任务故障查询
{
    [WcsPluginInfo(typeof(CraneTaskWarningsPlugin), "堆垛机任务故障查询", "paopao", "2014/4/23 8:07:00", "", false, "", "", 0)]
    public class CraneTaskWarningsPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmi故障统计Plugin = new ToolStripMenuItem("堆垛机任务故障查询");

            tsmi故障统计Plugin.Click += tsmi故障统计Plugin_Click;

            context.Application.GetMenu(WcsApplicationMenuType.View).DropDownItems.Add(tsmi故障统计Plugin);
            return base.Initialization(context);
        }


        void tsmi故障统计Plugin_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(typeof(CraneTaskWarningsPlugin).Assembly.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
