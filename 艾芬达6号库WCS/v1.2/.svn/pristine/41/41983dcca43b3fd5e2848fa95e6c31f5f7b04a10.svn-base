using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;


namespace Wcs.App.Plugins.RackLocation
{
   [WcsPluginInfo(typeof(RackLocationPlugin), "生成货架货位", "paopao", "2014/4/15 19:00:23", "<说明>", false)]
     public class RackLocationPlugin:WcsPlugin
    {
       public override bool Initialization(WcsContext context)
       {
           ToolStripMenuItem tsmi = new ToolStripMenuItem("生成货架货位");
           tsmi.Click += tsmi_Click;
           context.Application.GetMenu(WcsApplicationMenuType.Edit).DropDownItems.Add(tsmi);

           return base.Initialization(context);
       }

       frmMain frmMain;

       void tsmi_Click(object sender,EventArgs e)
       {
           if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
           {
               foreach (Form form in Application.OpenForms)
               {
                   frmMain.WindowState = FormWindowState.Maximized;
                   frmMain.Focus();
                   frmMain.Activate();
                   return;
               }
           }
           frmMain = new frmMain();
           frmMain.Show();

       }
    }
}
