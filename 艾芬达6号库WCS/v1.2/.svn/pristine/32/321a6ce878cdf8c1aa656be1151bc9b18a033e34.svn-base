using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceEventQueueSettings
{
    public partial class frmDeviceEventQueueSettings : Form
    {
        public frmDeviceEventQueueSettings()
        {
            InitializeComponent();
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备事件\\记录事件总线日志")]
        private void Item3_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            var v = !item.Checked;
            item.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>("eventBusEventPublisher-logger-enabled", v);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备事件\\记录事件派发日志")]
        private void Item2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            var v = !item.Checked;
            item.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>("deviceEventQueue-logger-enabled", v);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备事件\\异步处理")]
        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            var v = !item.Checked;
            item.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>("设备事件.异步处理", v);
        }
    }
}
