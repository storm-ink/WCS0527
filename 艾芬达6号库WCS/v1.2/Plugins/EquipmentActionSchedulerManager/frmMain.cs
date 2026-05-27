using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;
using Wcs.Framework.Cfg;
using NHibernate.Linq;
using Wcs.Framework.EventBus;
using Wcs.Framework.Events;

namespace Wcs.App.Plugins.EquipmentActionSchedulerManager
{
    public partial class frmMain : Form
    {
        BindingSource _bindingSource;
        Logger _logger;

        public frmMain()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();

            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<EquipmentAction>();

            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = _bindingSource;
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                tvDeviceType.Nodes.Clear();
                TreeNode root = new TreeNode("设备类型");

                foreach (var deviceGrouping in WcsConfiguration
                        .Instance
                        .DeviceCollection
                        .ParticularDeviceCollection
                        .SelectMany(x => x.DeviceElements)
                        .Where(x=>x.Device is TaskableDevice)
                        .GroupBy(x => x.Device.GetType())
                        )
                {
                    TreeNode deviceTypeNode = root.Nodes.Add(deviceGrouping.Key.GetDisplayName());
                    foreach (var deviceElement in deviceGrouping)
                    {
                        var device=(TaskableDevice)deviceElement.Device;
                        var node = new TreeNode();
                        if (device.EquipmentActionScheduler.CurrentAction != null)
                        {
                            node.Text = string.Format("{0}_{1}", device.Name, device.EquipmentActionScheduler.CurrentAction);
                        }
                        else
                        {
                            node.Text = device.Name;
                        }
                        node.Tag = deviceElement.Device;

                        deviceTypeNode.Nodes.Add(node);
                    }
                }

                tvDeviceType.Nodes.Add(root);
                tvDeviceType.ExpandAll();

                tvDeviceType_NodeMouseClick(null, null);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("设备管理器初始化失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EventBus.Instance.Subscribe<SchedulerActionAddedEvent>(onSchedulerActionAdded);
            EventBus.Instance.Subscribe<SchedulerActionRemovedEvent>(onSchedulerActionRemoved);
            EventBus.Instance.Subscribe<SchedulerCurrentActionChangedEvent>(onSchedulerCurrentActionChanged);
            EventBus.Instance.Subscribe<EquipmentActionStatusChangedEvent>(onActionStatusChanged);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventBus.Instance.Unsubscribe<SchedulerActionAddedEvent>(onSchedulerActionAdded);
            EventBus.Instance.Unsubscribe<SchedulerActionRemovedEvent>(onSchedulerActionRemoved);
            EventBus.Instance.Unsubscribe<SchedulerCurrentActionChangedEvent>(onSchedulerCurrentActionChanged);
            EventBus.Instance.Unsubscribe<EquipmentActionStatusChangedEvent>(onActionStatusChanged);
        }

        TaskableDevice _device;
        private void tvDeviceType_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e == null)
            {
                loadSequence(null);
            }
            else
            {
                loadSequence(e.Node.Tag as TaskableDevice);
            }
        }

        private void dgvGrid_Resize(object sender, EventArgs e)
        {
            label1.Location = new Point(dgvGrid.Left + (dgvGrid.Width / 2 - label1.Width / 2), dgvGrid.Top + (dgvGrid.Height / 2 - label1.Height / 2));
        }
        
        private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var action = dgvGrid.Rows[e.RowIndex].DataBoundItem.As<EquipmentAction>();
            if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = action.Status.GetDescription();
            }
            else if (e.ColumnIndex == colDescription.Index)
            {
                e.Value = action.ToReadableDescription();
            }
            switch (action.Status)
            {
                case EquipmentActionStatus.Executing:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    break;
                case EquipmentActionStatus.Suspend:
                case EquipmentActionStatus.Error:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    break;
                case EquipmentActionStatus.Cancelled:
                case EquipmentActionStatus.Completed:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                    break;
                case EquipmentActionStatus.New:
                default:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Control.DefaultForeColor;
                    break;
            }
        }

        void loadSequence(TaskableDevice device)
        {
            _bindingSource.Clear();

            if (device == null)
            {
                label1.Visible = true;
                return;
            }
            label1.Visible = false;
            foreach (var item in device.EquipmentActionScheduler
                .Actions.OrderByDescending(x=>x.Movement.Task.Priority)
                .ThenBy(x=>x.SequenceOrdering))
            {
                _bindingSource.Add(item);
            }

            int[] ids =device.EquipmentActionScheduler.Actions.Select(x=>x.Id).ToArray();
            EquipmentAction[] _actions;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                _actions = unitOfWork.session.Query<EquipmentAction>().Where(x => x.DeviceName == device.Name && !ids.Contains(x.Id) && x.Status != EquipmentActionStatus.Cancelled && x.Status != EquipmentActionStatus.Completed).ToArray();
                unitOfWork.Commit();
            }
            foreach (var item in _actions)
            {
                _bindingSource.Add(item);
            }

            _device = device;
        }
        
        void onSchedulerActionAdded(SchedulerActionAddedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<SchedulerActionAddedEvent> act = (e) =>
                {
                    onSchedulerActionAdded(e);
                };

                this.Invoke(act, args);
            }
            else
            {
                if (tvDeviceType.SelectedNode == null)
                {
                    return;
                }

                var device = tvDeviceType.SelectedNode.Tag as Device;
                if (device == null)
                {
                    return;
                }

                if (device != args.Scheduler.Device)
                {
                    return;
                }

                EquipmentAction action;
                if (args.Action.Id == 0)
                {
                    using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        action = unitOfWork.session.Query<EquipmentAction>().Where(x => x.EquipmentTaskId == args.Action.EquipmentTaskId).FirstOrDefault();
                        unitOfWork.Commit();
                    }
                }
                else
                    action = args.Action;

                if (action != null)
                    _bindingSource.Add(args.Action);
            }
        }

        void onSchedulerActionRemoved(SchedulerActionRemovedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<SchedulerActionRemovedEvent> act = (e) =>
                {
                    onSchedulerActionRemoved(e);
                };

                this.Invoke(act, args);
            }
            else
            {
                var act = _bindingSource.DataSource.As<List<EquipmentAction>>().FirstOrDefault(x => x.Id == args.Action.Id);
                if (act!=null)
                {
                    _bindingSource.Remove(act);
                }
            }
        }

        void onSchedulerCurrentActionChanged(SchedulerCurrentActionChangedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<SchedulerCurrentActionChangedEvent> act = (e) =>
                {
                    onSchedulerCurrentActionChanged(e);
                };

                this.Invoke(act, args);
            }
            else
            {
                var node = getAllNodes(tvDeviceType.Nodes[0])
                    .Where(x => x.Tag as Device != null)
                    .FirstOrDefault(x => ((Device)x.Tag) == args.Scheduler.Device);
                if (node == null)
                {
                    return;
                }

                if (args.newCurrentAction == null)
                {
                    node.Text = args.Scheduler.Device.Name;
                }
                else
                {
                    node.Text = string.Format("{0}_{1}", args.Scheduler.Device.Name, args.newCurrentAction);
                }
            }
        }

        void onActionStatusChanged(EquipmentActionStatusChangedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<EquipmentActionStatusChangedEvent> act = (e) =>
                {
                    onActionStatusChanged(e);
                };

                this.Invoke(act, args);
            }
            else
            {
                var act = _bindingSource.DataSource.As<List<EquipmentAction>>().FirstOrDefault(x => x.Id == args.Id);
                if (act != null)
                {
                    act.Status = args.Status;

                    _bindingSource.ResetBindings(false);
                }
            }
        }

        List<TreeNode> getAllNodes(TreeNode root)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(root);
            foreach (TreeNode item in root.Nodes)
            {
                result.AddRange(getAllNodes(item));
            }

            return result;
        }

        private void tvDeviceType_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e == null || e.Node==null || e.Node.Tag==null)
            {
                return;
            }
            else
            {
                using (frmMethodTrace frm = new frmMethodTrace((e.Node.Tag as TaskableDevice).EquipmentActionScheduler))
                {
                    frm.ShowDialog();
                }
            }
        }

        private void dgvGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_device == null)
                return;

            var actionTaskId = Convert.ToInt32(dgvGrid.Rows[e.RowIndex].Cells["colEquipmentTaskId"].Value);
            var action = _device.EquipmentActionScheduler.Actions.FirstOrDefault(x => x.EquipmentTaskId == actionTaskId);
            if (action == null)
            {
                using (NHUnitOfWork unitOfwork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    action = unitOfwork.session.Query<EquipmentAction>().FirstOrDefault(x => x.EquipmentTaskId == actionTaskId);
                    unitOfwork.Commit();
                }
            }
            if (action == null || action.Status == EquipmentActionStatus.Cancelled || action.Status == EquipmentActionStatus.Completed)
                return;

            var manager = Wcs.Framework.AbstractStateManager.Contexts.FirstOrDefault(x => x.EquipmentAction.EquipmentTaskId == action.EquipmentTaskId);
            if (manager == null)
                return;

            AbstractStateManagerState frm = new AbstractStateManagerState(manager);
            frm.ShowDialog();
        }
    }
}
