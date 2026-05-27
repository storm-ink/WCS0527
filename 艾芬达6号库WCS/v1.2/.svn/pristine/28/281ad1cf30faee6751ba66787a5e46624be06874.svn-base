using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.Debugger
{
    [WcsPluginInfo(typeof(DebuggerPlugin), "线程列表", "Sineva", "2022年6月", "配合现场调试运维使用", false, "工具箱", "调试工具", 3, 2, 0)]
    public class DebuggerPlugin:Wcs.WcsPlugin
    {
        frmThreadsViewer _frmThreadsViewer;
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "线程列表";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}_route";
            barButtonItem.ItemClick += mi_threadsViewer_click;

            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "工具箱\\调试工具\\线程列表")]
        void mi_threadsViewer_click(object sender, EventArgs e)
        {
            if (_frmThreadsViewer != null && !_frmThreadsViewer.IsDisposed && !_frmThreadsViewer.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == _frmThreadsViewer)
                    {
                        _frmThreadsViewer.Focus();
                        _frmThreadsViewer.Activate();
                        _frmThreadsViewer.TopMost = true;
                        return;
                    }
                }
            }

            _frmThreadsViewer = new frmThreadsViewer();
            _frmThreadsViewer.Show();
        }
    }
}
