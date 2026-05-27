using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.RequestManager
{
    [WcsPluginInfo(typeof(RequestManagerPlugin), "请求管理", "Sineva", "2014/3/18 15:15:00", "", false)]
    public class RequestManagerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmiRequestManagerPlugin = new ToolStripMenuItem("请求管理");
            tsmiRequestManagerPlugin.Click += tsmiRequestManagerPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiRequestManagerPlugin);

            return base.Initialization(context);
        }

        frmMain frmRequestManagerPlugin;
        void tsmiRequestManagerPlugin_Click(object sender, EventArgs e)
        {
            if (frmRequestManagerPlugin != null && !frmRequestManagerPlugin.IsDisposed && !frmRequestManagerPlugin.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == frmRequestManagerPlugin)
                    {
                        frmRequestManagerPlugin.Focus();
                        frmRequestManagerPlugin.Activate();
                        frmRequestManagerPlugin.TopMost = true;
                        return;
                    }
                }
            }

            frmRequestManagerPlugin = new frmMain();
            frmRequestManagerPlugin.MdiParent = this.Context.Application.MainForm;
            frmRequestManagerPlugin.Show();
            frmRequestManagerPlugin.WindowState = FormWindowState.Maximized;
            frmRequestManagerPlugin.Activate();
        }
    }
}
