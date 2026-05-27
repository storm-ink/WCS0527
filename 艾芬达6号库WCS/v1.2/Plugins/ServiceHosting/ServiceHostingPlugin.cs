using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.ServiceHosting
{
    [WcsPluginInfo(typeof(ServiceHostingPlugin), "Wcf服务宿主", "Sineva", "2014/3/8 12:41:01", "提供一个可托管 Wcf 服务的进程，同时用户还可以对托管的服务进行停止或开始操作", true)]
    public class ServiceHostingPlugin : Wcs.WcsPlugin
    {
        List<dynamic> _hosts;
        Logger _logger;
        public override bool Initialization(WcsContext context)
        {
            ToolStripMenuItem tsmiServiceHostingPlugin = new ToolStripMenuItem("Wcf服务宿主");
            tsmiServiceHostingPlugin.Click += tsmiServiceHostingPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add(tsmiServiceHostingPlugin);

            _logger = NLog.LogManager.GetCurrentClassLogger();

            _hosts = new List<dynamic>();
            foreach (var wcfServiceElement in Wcs.Framework.Cfg.WcsConfiguration.Instance.ServiceElement.WcfServiceCollection.WcfServiceElements)
            {
                try
                {
                    Type hostType=typeof(WcfHosting<>).MakeGenericType(new Type[]{wcfServiceElement.Type});

                    var hosting = hostType.Assembly.CreateInstance(hostType.FullName, false, System.Reflection.BindingFlags.CreateInstance, null, null, null, null);
                    hosting.GetType().GetMethod("Launch").Invoke(hosting, null);
                    _hosts.Add(hosting);
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            }

            return base.Initialization(context);
        }

        frmMain frmServiceHostingPlugin;
        void tsmiServiceHostingPlugin_Click(object sender, EventArgs e)
        {
            if (frmServiceHostingPlugin != null && !frmServiceHostingPlugin.IsDisposed && !frmServiceHostingPlugin.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == frmServiceHostingPlugin)
                    {
                        frmServiceHostingPlugin.Focus();
                        frmServiceHostingPlugin.Activate();
                        frmServiceHostingPlugin.TopMost = true;
                        return;
                    }
                }
            }

            frmServiceHostingPlugin = new frmMain(_hosts);
            frmServiceHostingPlugin.MdiParent = this.Context.Application.MainForm;
            frmServiceHostingPlugin.Show();
            frmServiceHostingPlugin.WindowState = FormWindowState.Maximized;
            frmServiceHostingPlugin.Activate();
        }
    }
}
