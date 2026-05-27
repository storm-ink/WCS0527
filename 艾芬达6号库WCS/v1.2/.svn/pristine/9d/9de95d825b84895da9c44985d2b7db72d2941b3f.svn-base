
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;

namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    [WcsPluginInfo(typeof(CraneManualClientPlugin), "双货位堆垛机操作终端", "Sineva", "2014/11/19 12:41:01", "", false)]
    public class CraneManualClientPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmiCraneManualClientPlugin = new ToolStripMenuItem("双货位堆垛机操作终端");
            tsmiCraneManualClientPlugin.Click += tsmiCraneManualClientPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiCraneManualClientPlugin);

            return base.Initialization(context);
        }

        void tsmiCraneManualClientPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                 Process.Start(typeof(CraneManualClientPlugin).Assembly.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
