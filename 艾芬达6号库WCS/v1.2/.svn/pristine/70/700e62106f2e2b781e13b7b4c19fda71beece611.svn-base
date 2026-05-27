using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.EventsManager
{
    [WcsPluginInfo(typeof(EventsManagerPlugin), "延迟事件管理器", "Sineva", "2014/3/8 8:52:49", "允许用户对延迟事件进行简单的管理操作。", false)]
    public class EventsManagerPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmiEventsManagerPlugin = new ToolStripMenuItem("迟延事件");
            tsmiEventsManagerPlugin.Click += tsmiEventsManagerPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiEventsManagerPlugin);

            return base.Initialization(context);
        }

        frmMain frmEventsManagerPlugin;
        void tsmiEventsManagerPlugin_Click(object sender, EventArgs e)
        {
            if (frmEventsManagerPlugin != null && !frmEventsManagerPlugin.IsDisposed && !frmEventsManagerPlugin.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == frmEventsManagerPlugin)
                    {
                        frmEventsManagerPlugin.Focus();
                        frmEventsManagerPlugin.Activate();
                        frmEventsManagerPlugin.TopMost = true;
                        return;
                    }
                }
            }

            frmEventsManagerPlugin = new frmMain();
            frmEventsManagerPlugin.MdiParent = this.Context.Application.MainForm;
            frmEventsManagerPlugin.Show();
            frmEventsManagerPlugin.WindowState = FormWindowState.Maximized;
            frmEventsManagerPlugin.Activate();
        }
    }
}
