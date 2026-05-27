using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wcs.Framework;
using Wcs.Framework.Cfg;

namespace Wcs.App.Plugins.DeviceManager
{
    public partial class frmMain : Form
    {
        Logger _logger;
        public frmMain()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
        }

        Device SelectedDevice
        {
            get
            {
                if (lvDevices.SelectedItems.Count == 0)
                {
                    return null;
                }

                ListViewItem item = lvDevices.SelectedItems[0];
                Device device = item.Tag as Device;

                return device;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (var device in WcsConfiguration
                    .Instance
                    .DeviceCollection
                    .ParticularDeviceCollection
                    .SelectMany(x => x.DeviceElements)
                    .Select(x => x.Device)
                    )
                {
                    device.Disconnected += device_Disconnected;
                    device.Connected += device_Connected;
                }
                tvDeviceType.Nodes.Clear();
                TreeNode root = new TreeNode("设备类型");
                foreach (var deviceGrouping in WcsConfiguration
                    .Instance
                    .DeviceCollection
                    .ParticularDeviceCollection
                    .SelectMany(x => x.DeviceElements)
                    .GroupBy(x => x.Device.GetType())
                    )
                {
                    TreeNode deviceTypeNode = new TreeNode(deviceGrouping.Key.GetDisplayName());
                    deviceTypeNode.Tag = deviceGrouping.Key;
                    root.Nodes.Add(deviceTypeNode);
                }

                tvDeviceType.Nodes.Add(root);
                tvDeviceType.ExpandAll();

                showDevices(null);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("设备管理器初始化失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void device_Connected(Device device, ConnectedEventArgs args)
        {
            args.Handled = true;
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    device_Connected(device, args);
                }));
            }
            else
            {
                var listViewItems = lvDevices.Items.Find(device.Name, true);
                if (listViewItems.Length > 0)
                {
                    listViewItems[0].Selected = !listViewItems[0].Selected;
                    listViewItems[0].Selected = !listViewItems[0].Selected;
                }
            }
        }

        void device_Disconnected(Device device, DisconnectEventArgs args)
        {
            args.Handled = true;

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    device_Disconnected(device, args);
                }));
            }
            else
            {
                var listViewItems = lvDevices.Items.Find(device.Name, true);
                if (listViewItems.Length > 0)
                {
                    listViewItems[0].Selected = !listViewItems[0].Selected;
                    listViewItems[0].Selected = !listViewItems[0].Selected;
                }
            }
        }

        private void tvDeviceType_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;

            Type deviceType = null;
            if (e.Node.Tag != null)
            {
                deviceType = (Type)e.Node.Tag;
            }

            showDevices(deviceType);
        }

        private void showDevices(Type deviceType)
        {
            try
            {
                lvDevices.SuspendLayout();

                lvDevices.Groups.Clear();
                lvDevices.Items.Clear();
                var devices = WcsConfiguration
                    .Instance
                    .DeviceCollection
                    .ParticularDeviceCollection
                    .SelectMany(x => x.DeviceElements)
                    .Where(x => deviceType == null || x.Device.GetType() == deviceType)
                    .GroupBy(x => x.Device.GetType());

                foreach (var deviceGrouping in devices)
                {
                    ListViewGroup lvg = new ListViewGroup(deviceGrouping.Key.GetDisplayName());
                    lvDevices.Groups.Add(lvg);
                    foreach (var deviceElement in deviceGrouping)
                    {
                        var item = new ListViewItem(deviceElement.Device.Name, 0, lvg);
                        item.Tag = deviceElement.Device;
                        item.Name = deviceElement.Device.Name;
                        lvDevices.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
            finally
            {
                lvDevices.ResumeLayout();
            }
        }

        private void lvDevices_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            try
            {
                e.DrawDefault = true;
                Device device = e.Item.Tag as Device;
                if (device != null)
                {
                    Image img = null;
                    if (device.IsConnected && (device.Warnings != null && device.Warnings.Length != 0))
                    {
                        img = imageList3.Images["warning"];
                    }
                    else if (device.IsConnected && (device.Warnings == null || device.Warnings.Length == 0))
                    {
                        img = imageList3.Images["online"];
                    }
                    else if (!device.IsConnected)
                    {
                        img = imageList3.Images["offline"];
                    }
                    if (img != null)
                    {
                        e.Graphics.DrawImage(img, e.Bounds.X + e.Bounds.Width - img.Width, e.Bounds.Y);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\查看状态")]
        private void lvDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = lvDevices.GetItemAt(e.X, e.Y);
            try
            {
                if (lvi != null)
                {
                    var device = (lvi.Tag as Device);

                    //显示操作界面
                    using (IDeviceUserInterface ui = device.CreateUserInterface())
                    {
                        if (ui == null)
                        {
                            MessageBox.Show(string.Format("{0} 未配置用户界面。", device.Name), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        ui.Show(device);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var device = SelectedDevice;
            if (device == null)
            {
                e.Cancel = true;
                return;
            }

            if (device.IsConnected)
            {
                this.tsmiConnect.Text = "断开";
            }
            else
            {
                this.tsmiConnect.Text = "连接";
            }

            if (device.Locker.IsEmpty)
            {
                this.tsmiLock.Text = "锁定";
            }
            else
            {
                this.tsmiLock.Text = "解锁";
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\连接设备")]
        private void tsmiConnect_Click(object sender, EventArgs e)
        {
            var device = SelectedDevice;

            if (device == null)
            {
                return;
            }


            if (this.tsmiConnect.Text == "断开")
            {
                if (device.IsConnected)
                {
                    device.Disconnect();
                }
            }
            else
            {
                if (!device.IsConnected)
                {
                    //防止使用同步调用时引起的UI线程堵死
                    System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
                    {
                        ((Device)stat).Connect();
                    }, device);
                }
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\查看状态")]
        private void tsmiStatus_Click(object sender, EventArgs e)
        {
            var device = SelectedDevice;

            if (device == null)
            {
                return;
            }

            using (IDeviceUserInterface ui = device.CreateUserInterface())
            {
                if (ui == null)
                {
                    MessageBox.Show(string.Format("{0} 未配置用户界面。", device.Name), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ui.Show(device);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\锁定、解锁")]
        private void tsmiLock_Click(object sender, EventArgs e)
        {
            var device = SelectedDevice;

            if (device == null)
            {
                return;
            }

            if (tsmiLock.Text == "解锁")
            {
                if (!device.Locker.IsEmpty)
                {
                    string msg = String.Format("是否要强制清除 {0} {1} {2}\n\n设备锁在清除后有可能会立即运行\n请先确认所有人员已离开设备运行区域\n\n是否继续？"
                                            , device.GetType().GetDisplayName()
                                            , device.Name
                                            , device.Locker);
                    if (MessageBox.Show(msg, "强制清除设备锁定提示"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }

                    device.Unlock(device.Locker);
                }
            }
            else
            {
                if (device.Locker.IsEmpty)
                {
                    device.Lock(new LockerInfo(System.Environment.MachineName, LockerInfo.GetIpAddress()));
                }
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "设备管理\\设备管理\\引发无法连接故障")]
        private void 引发无法连接故障ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var device = SelectedDevice;

                if (device == null)
                {
                    return;
                }

                if (device.IsConnected)
                {
                    return;
                }

                device.FireUnableToConnectError();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
