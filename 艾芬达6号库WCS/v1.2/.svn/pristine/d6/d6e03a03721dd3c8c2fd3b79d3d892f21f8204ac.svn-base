using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ConveyorLocation
{
    [WcsPluginInfo(typeof(ConveyorLocationPlugin),"生成输送线货位", "paopao", "2014/4/15 19:46:23", "<说明>", false)]
     public  class ConveyorLocationPlugin:WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem("生成输送线货位");
            tsmi.Click += tsmi_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Edit).DropDownItems.Add(tsmi);

            return base.Initialization(context);
        }

        frmConveyorLocation frmConveyorLocation;

        void tsmi_Click(object sender, EventArgs e)
        {
            if (frmConveyorLocation != null && !frmConveyorLocation.IsDisposed && !frmConveyorLocation.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    frmConveyorLocation.WindowState = FormWindowState.Maximized;
                    frmConveyorLocation.Focus();
                    frmConveyorLocation.Activate();
                    return;
                }
            }
            frmConveyorLocation = new frmConveyorLocation();
            frmConveyorLocation.Show();

        }
 
    }
}
