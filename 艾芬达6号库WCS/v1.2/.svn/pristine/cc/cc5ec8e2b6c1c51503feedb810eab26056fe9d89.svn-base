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
using Wcs.Framework.Events;
using NHibernate.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Wcs.App.Plugins.TaskManager
{
    public partial class frmTaskViewer : Form
    {
        Logger _logger;
        BindingSource _movementsBindingSource;
        BindingSource _actionsBindingSource;

        TaskChooseEndLocationHandler taskChooseEndLocationHandler;

        Int32 _taskId;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\任务详情查看")]
        public frmTaskViewer(Int32 taskId)
        {
            _taskId = taskId;
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();


            dgvMovementsGrid.AutoGenerateColumns =
                dgvActionsGrid.AutoGenerateColumns = false;

            _movementsBindingSource = new BindingSource();
            _movementsBindingSource.DataSource = new List<LogicMovement>();
            dgvMovementsGrid.DataSource = _movementsBindingSource;

            _actionsBindingSource = new BindingSource();
            _actionsBindingSource.DataSource = new List<EquipmentAction>();
            dgvActionsGrid.DataSource = _actionsBindingSource;


        }

        private void frmTaskViewer_Load(object sender, EventArgs e)
        {
            try
            {
                load(_taskId);
                taskChooseEndLocationHandler = Wcs.Framework.Cfg.WcsConfiguration.Instance.TaskChouseEndLocationHandlersElement.TaskChooseEndLocationHandler;
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                this.Close();
                return;
            }

            dgvMovementsGrid.CellFormatting += dgvMovementsGrid_CellFormatting;
            dgvActionsGrid.CellFormatting += dgvActionsGrid_CellFormatting;
            _movementsBindingSource.CurrentItemChanged += _movementsBindingSource_CurrentItemChanged;

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskArchivedEvent>(onTaskArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskCurrentLocationChangedEvent>(onTaskCurrentLocationChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskPriorityChangedEvent>(onTaskPriorityChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskFinishedEvent>(onTaskFinishedChanged);

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<LogicMovementAddedEvent>(onLogicMovementAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<LogicMovementStatusChangedEvent>(onMovementStatusChanged);

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<EquipmentActionStatusChangedEvent>(onActionStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<EquipmentActionWarningAddedEvent>(onEquipmentActionWarningAdded);

        }

        #region 任务事件
        void onTaskArchived(TaskArchivedEvent args)
        {
            if (_taskId != args.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<TaskArchivedEvent> act = (_args) =>
                {
                    onTaskArchived(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                btnCancel.Enabled =
                    btnCompleted.Enabled =
                    btnResume.Enabled =
                    btnSuspend.Enabled =
                    btnArchive.Enabled =
            btnChangeEndLocation.Enabled =
                dgvMovementsGrid.Enabled =
                    dgvActionsGrid.Enabled = false;

                lblTaskCode.Text += "<已归档>";
                this.Text += "<已归档>";
            }
        }

        void onTaskStatusChanged(TaskStatusChangedEvent args)
        {
            if (_taskId != args.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<TaskStatusChangedEvent> act = (_args) =>
                {
                    onTaskStatusChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                lblStatus.Text = args.Status.GetDescription();

                setButtonsStatus(args.Status);
            }
        }

        void onTaskCurrentLocationChanged(TaskCurrentLocationChangedEvent args)
        {
            if (_taskId != args.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<TaskCurrentLocationChangedEvent> act = (_args) =>
                {
                    onTaskCurrentLocationChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                lblCurrentLocation.Text = string.Format("{0}@{1}", args.CurrentLocation.UserCode, args.CurrentLocation.DeviceName);
            }
        }

        void onTaskPriorityChanged(TaskPriorityChangedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<TaskPriorityChangedEvent> act = (_args) =>
                {
                    onTaskPriorityChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                lblPriority.Text = args.Priority.ToString();
            }
        }

        void onTaskFinishedChanged(TaskFinishedEvent args)
        {
            if (_taskId != args.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<TaskFinishedEvent> act = (_args) =>
                {
                    onTaskFinishedChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                lblStatus.Text = args.Status.GetDescription();
                lblFinishedAt.Text = args.FinishedAt.ToString("yyyy-MM-dd HH:mm:ss");
                setButtonsStatus(args.Status);
            }
        }

        void onMovementStatusChanged(LogicMovementStatusChangedEvent args)
        {
            if (_taskId != args.TaskId)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<LogicMovementStatusChangedEvent> act = (_args) =>
                {
                    onMovementStatusChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<LogicMovement> movements = _movementsBindingSource.Cast<LogicMovement>().ToList();
                if (movements == null)
                {
                    return;
                }
                var movement = movements.FirstOrDefault(x => x.Id == args.Id);
                if (movement == null)
                {
                    return;
                }

                movement.Status = args.Status;

                _movementsBindingSource.ResetBindings(false);
            }
        }

        void onActionStatusChanged(EquipmentActionStatusChangedEvent args)
        {
            if (_taskId != args.TaskId)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<EquipmentActionStatusChangedEvent> act = (_args) =>
                {
                    onActionStatusChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<EquipmentAction> actions = _actionsBindingSource.Cast<EquipmentAction>().ToList();
                if (actions == null)
                {
                    return;
                }
                var action = actions.FirstOrDefault(x => x.Id == args.Id);
                if (action == null)
                {
                    return;
                }

                action.Status = args.Status;

                _actionsBindingSource.ResetBindings(false);
            }
        }

        void onLogicMovementAdded(LogicMovementAddedEvent args)
        {
            if (_taskId != args.Movement.Task.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<LogicMovementAddedEvent> act = (_args) =>
                {
                    onLogicMovementAdded(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<LogicMovement> movements = _movementsBindingSource.Cast<LogicMovement>().ToList();
                if (movements != null)
                {
                    var movement = movements.FirstOrDefault(x => x.Id == args.Movement.Id);
                    if (movement != null)
                    {
                        movement.Id = args.Movement.Id;
                        movement.Status = args.Movement.Status;
                        _movementsBindingSource.ResetBindings(false);
                        return;
                    }
                }

                _movementsBindingSource.Add((LogicMovement)args.Movement);
            }
        }

        void onEquipmentActionWarningAdded(EquipmentActionWarningAddedEvent args)
        {
            if (_taskId != args.Warning.EquipmentAction.Movement.Task.Id)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                Action<EquipmentActionWarningAddedEvent> act = (_args) =>
                {
                    onEquipmentActionWarningAdded(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<EquipmentAction> actions = _actionsBindingSource.Cast<EquipmentAction>().ToList();
                if (actions == null)
                {
                    return;
                }
                var action = actions.FirstOrDefault(x => x.Id == args.Warning.EquipmentAction.Id);
                if (action == null)
                {
                    return;
                }

                action.Warnings.Add(args.Warning);

                _actionsBindingSource.ResetBindings(false);
            }
        }

        #endregion

        void load(Int32 taskId)
        {
            _movementsBindingSource.Clear();
            _actionsBindingSource.Clear();

            dgvMovementsGrid.AutoGenerateColumns =
                dgvActionsGrid.AutoGenerateColumns = false;

            Task task;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                task = unitOfWork.session.Get<Task>(taskId);

                unitOfWork.Commit();
            }

            if (task == null)
            {
                throw new Exception(string.Format("未找到 id 为 {0} 的 Task 对象", taskId));
            }

            this.Text = string.Format("查看任务 {0}#{1} 信息", task.Id, task.TaskCode);

            lblTaskCode.Text = string.Format("{0}#{1}", task.Id, task.TaskCode);
            lblStartLocation.Text = string.Format("{0}@{1}", task.StartLocation.UserCode, task.StartLocation.DeviceName);
            lblEndLocation.Text = string.Format("{0}@{1}", task.EndLocation.UserCode, task.EndLocation.DeviceName);
            lblBzType.Text = task.BizType.GetDescription();
            lblSource.Text = task.Source.GetDescription();
            lblStatus.Text = task.Status.GetDescription();
            lblPriority.Text = task.Priority.ToString();
            lblCreatedAt.Text = task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
            if (task.FinishedAt == null)
            {
                lblFinishedAt.Text = "-";
            }
            else
            {
                lblFinishedAt.Text = task.FinishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            lblCurrentLocation.Text = string.Format("{0}@{1}", task.CurrentLocation.UserCode, task.CurrentLocation.DeviceName);
            lblContainerCodes.Text = string.Join(",", task.ContainerCodes.ToArray());

            if (Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings.ContainsKey("AbleTaskChouseEndLocation"))
                btnChangeEndLocation.Visible = Convert.ToBoolean(Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings["AbleTaskChouseEndLocation"]);            

            setButtonsStatus(task.Status);

            _movementsBindingSource.Clear();
            foreach (var item in task.Movements)
            {
                _movementsBindingSource.Add((LogicMovement)item);
            }

            _movementsBindingSource_CurrentItemChanged(null, null);
        }

        void setButtonsStatus(TaskStatus taskStatus)
        {
            btnArchive.Enabled = taskStatus == TaskStatus.Completed || taskStatus == TaskStatus.Cancelled;
            btnSuspend.Enabled = taskStatus == TaskStatus.New || taskStatus == TaskStatus.Executing;
            btnResume.Enabled = taskStatus == TaskStatus.Suspend || taskStatus == TaskStatus.Error;
            btnCompleted.Enabled = taskStatus == TaskStatus.Suspend || taskStatus == TaskStatus.Error;
            btnCancel.Enabled = taskStatus == TaskStatus.Suspend || taskStatus == TaskStatus.Error;
            btnChangeEndLocation.Enabled = taskStatus == TaskStatus.Suspend;
        }

        #region 对象事件
        void _movementsBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            try
            {
                _actionsBindingSource.Clear();
                if (_movementsBindingSource.Current == null)
                {
                    return;
                }
                var movement = (LogicMovement)_movementsBindingSource.Current;
                foreach (var action in movement.EquipmentActions)
                {
                    _actionsBindingSource.Add((EquipmentAction)action);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void dgvMovementsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var task = dgvMovementsGrid.Rows[e.RowIndex].DataBoundItem.As<LogicMovement>();
            if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = task.Status.GetDescription();
            }
            else if (e.ColumnIndex == colCreatedAt.Index)
            {
                e.Value = task.CreatedAt.ToString("HH:mm:ss");
            }
            else if (e.ColumnIndex == colFinishedAt.Index)
            {
                if (task.FinishedAt == null)
                {
                    e.Value = "-";
                }
                else
                {
                    e.Value = task.FinishedAt.Value.ToString("HH:mm:ss");
                }
            }
            else if (e.ColumnIndex == colRouteNo.Index)
            {
                if (task.RouteId.GetValueOrDefault(0) != 0)
                {
                    var route = RouteHelper.RouteHeads.FirstOrDefault(x => x.Id == task.RouteId.Value);
                    if (route != null)
                    {
                        e.Value = string.Format("{0}#{1}", route.Id, route.No);
                    }
                    else
                    {
                        e.Value = string.Format("{0}(未找到)", task.RouteId);
                    }
                }
                else
                {
                    e.Value = task.RouteId;
                }
            }
        }

        private void dgvActionsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var action = dgvActionsGrid.Rows[e.RowIndex].DataBoundItem.As<EquipmentAction>();
            if (e.ColumnIndex == colWarning.Index)
            {
                var warning = action.Warnings.LastOrDefault();
                if (warning == null)
                {
                    e.Value = "";

                    dgvActionsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Control.DefaultForeColor;
                }
                else
                {
                    e.Value = warning.Description;
                    dgvActionsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                }
            }
            else if (e.ColumnIndex == colDevice.Index)
            {
                e.Value = action.DeviceName;
            }
            else if (e.ColumnIndex == colReadableDescription.Index)
            {
                e.Value = action.ToReadableDescription();
            }
            else if (e.ColumnIndex == colActionStatus.Index)
            {
                e.Value = action.Status.GetDescription();
            }
        }

        private void frmTaskViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskArchivedEvent>(onTaskArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskCurrentLocationChangedEvent>(onTaskCurrentLocationChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskPriorityChangedEvent>(onTaskPriorityChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskFinishedEvent>(onTaskFinishedChanged);

            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<LogicMovementAddedEvent>(onLogicMovementAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<LogicMovementStatusChangedEvent>(onMovementStatusChanged);

            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<EquipmentActionStatusChangedEvent>(onActionStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<EquipmentActionWarningAddedEvent>(onEquipmentActionWarningAdded);


            dgvMovementsGrid.CellFormatting -= dgvMovementsGrid_CellFormatting;
            dgvActionsGrid.CellFormatting -= dgvActionsGrid_CellFormatting;
            _movementsBindingSource.CurrentItemChanged -= _movementsBindingSource_CurrentItemChanged;
        }
        #endregion

        #region 任务处理
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\归档")]
        private void tsmiArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否确认要归档任务？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                _logger.Trace1(string.Format("准备人工归档 {0} ...", _taskId), this);
                Wcs.Framework.TaskHelper.Archive(_taskId);
                _logger.Trace1(string.Format("人工归档 {0} 成功", _taskId), this);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务归档失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\暂停")]
        private void tsmiSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否确认要暂停任务？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
                {
                    SetForegroundWindow(this.Handle);
                    this.dgvMovementsGrid.Focus();
                    Thread.Sleep(200);
                    SetForegroundWindow(this.Handle);
                    this.dgvMovementsGrid.Focus();
                    return;
                }

                _logger.Trace1(string.Format("准备人工暂停 {0} ...", _taskId), this);
                Wcs.Framework.TaskHelper.Suspend(_taskId);
                _logger.Trace1(string.Format("人工暂停 {0} 成功", _taskId), this);
                //this.Activate();
                //this.Select(true, true);
                //this.Focus();
                SetForegroundWindow(this.Handle);
                this.dgvMovementsGrid.Focus();
                Thread.Sleep(200);
                SetForegroundWindow(this.Handle);
                this.dgvMovementsGrid.Focus();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务暂停失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\取消")]
        private void tsmiCancel_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmSetObjectToCompleted frm = new frmSetObjectToCompleted(_taskId))
                {
                    if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }

                    var obj = frm.SelectedObject;

                    if (obj is Task)
                    {
                        _logger.Trace1(string.Format("准备人工取消任务 {0} ...", _taskId), this);
                        Wcs.Framework.TaskHelper.Cancle(obj, _taskId);
                    }
                    else if (obj is LogicMovement)
                    {
                        _logger.Trace1(string.Format("准备人工取消逻辑动作 {0} ...", _taskId), this);
                        Wcs.Framework.TaskHelper.Cancle(obj, obj.Id);
                    }
                    else if (obj is EquipmentAction)
                    {
                        _logger.Trace1(string.Format("准备人工取消物理动作 {0} ...", _taskId), this);
                        Wcs.Framework.TaskHelper.Cancle(obj, obj.Id);
                    }
                    else

                _logger.Trace1(string.Format("人工取消 {0} 成功", _taskId), this);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务取消失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\强制完成")]
        private void tsmiComplete_Click(object sender, EventArgs e)
        {
            try
            {
                //String msg = string.Format("在强制完成前请确认容器 {0} 是否已离开 {1}，并且已到达 {2}。\n\n是否继续？",
                //    String.Join(",", lblContainerCodes.Text),
                //    lblStartLocation.Text,
                //    lblEndLocation.Text);

                //if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                //{
                //    return;
                //}

                //Wcs.Framework.TaskHelper.Complete(_taskId);

                using (frmSetObjectToCompleted frm = new frmSetObjectToCompleted(_taskId))
                {
                    if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }

                    var obj = frm.SelectedObject;

                    _logger.Trace1(string.Format("准备人工完成 {0} ...", (object)obj), this, (object)obj);
                    if (obj is Task)
                    {
                        Wcs.Framework.TaskHelper.Complete(obj.Id);
                    }
                    else if (obj is LogicMovement)
                    {
                        Wcs.Framework.TaskHelper.CompleteMovement(obj.Id);
                    }
                    else if (obj is EquipmentAction)
                    {
                        Wcs.Framework.TaskHelper.CompleteAction(obj.Id);
                    }
                    else
                    {
                        throw new NotImplementedException(string.Format("未实现对 {0} 类型的任务的强制完成处理。", obj.GetType()));
                    }
                    _logger.Trace1(string.Format("人工完成 {0} 成功", (object)obj), this, (object)obj);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\继续执行")]
        private void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                Task task;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Get<Task>(_taskId);

                    unitOfWork.Commit();
                }

                if (task == null)
                {
                    MessageBox.Show(string.Format("未找到 id 为 {0} 的任务", _taskId), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                var stopRoute = TaskHelper.GetTaskResumeAtRoute(_taskId);
                if (stopRoute != null && stopRoute.AllowStartFromMidway)
                {
                    using (frmResumeChooseCurrentLocation frm = new frmResumeChooseCurrentLocation(task, stopRoute))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            _logger.Trace1(string.Format("准备人工继续执行 {0}，位置 {1} ...", task, frm.CurrentLocation), this, task);

                            Wcs.Framework.TaskHelper.Resume(_taskId, frm.CurrentLocation);

                            _logger.Trace1(string.Format("人工继续执行 {0} 成功", task), this, task);
                        }
                    }
                }
                else
                {
                    var currentLocation = LocationConverter.ToLocation(task.CurrentLocation);

                    _logger.Trace1(string.Format("准备人工继续执行 {0}，位置 {1} ...", task, currentLocation), this, task);



                    Wcs.Framework.TaskHelper.Resume(_taskId, currentLocation);

                    _logger.Trace1(string.Format("人工继续执行 {0} 成功", task), this, task);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务继续执行失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            }
        }
        #endregion

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void dgvActionsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvActionsGrid.CurrentRow == null)
            {
                return;
            }

            var action = dgvActionsGrid.CurrentRow.DataBoundItem.As<EquipmentAction>();
            if (action == null)
            {
                return;
            }

            var device = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Where(x => x.Device.Name == action.DeviceName)
                .Select(x => x.Device)
                .FirstOrDefault();

            if (device.GetType().FullName.Contains("Proxy.ClientDevice") && device.GetType().GetMethod("Show", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public) != null)
            {
                var mi = device.GetType().GetMethod("Show", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                mi.Invoke(device, null);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\调整终点")]
        private void btnChangeEndLocation_Click(object sender, EventArgs e)
        {
            try
            {
                Task task;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Get<Task>(_taskId);
                    unitOfWork.Commit();
                }

                if (task == null)
                {
                    MessageBox.Show(string.Format("未找到 id 为 {0} 的任务", _taskId), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                LocationInfo newEndLocation = null;
                if (task.Source == TaskSource.Wms)
                {
                    if (taskChooseEndLocationHandler == null)
                    {
                        MessageBox.Show("未配置向WMS请求变更任务终点的处理程序");
                        return;
                    }
                    var result = taskChooseEndLocationHandler.Hand(task, LocationConverter.UserCodeToLcation(task.CurrentLocation.UserCode));
                    if (result)
                        MessageBox.Show("请求变更成功");
                    else
                        MessageBox.Show("请求变更失败");
                }
                else
                {
                    using (frmChooseNewEndLocation frm = new frmChooseNewEndLocation(task))
                    {
                        if (frm.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        newEndLocation = frm.NewEndLocation;
                    }
                }
                if (newEndLocation == null)
                    return;

                var path = RouteHelper.AbleArrived(LocationConverter.ToLocation(task.CurrentLocation), LocationConverter.ToLocation(newEndLocation));
                if (!path)
                {
                    MessageBox.Show(String.Format("{0}到新的任务终点{1}不连通。", task.CurrentLocation, newEndLocation), Application.CompanyName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                List<Wcs.Framework.EventBus.IEvent> events = new List<Framework.EventBus.IEvent>();
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    task = unitOfWork.session.Get<UndefinedEndLocationTask>(task.Id);

                    var lastMovement = task.Movements.OrderByDescending(x => x.CreatedAt)
                            .FirstOrDefault();

                    _logger.Debug1(string.Format("人工开始将任务终点由{0}调整为{1}...", task.EndLocation, newEndLocation), this, task);

                    task.EndLocation = newEndLocation;

                    if (
                        lastMovement != null &&
                        (
                            lastMovement.Status == LogicMovementStatus.New
                            || lastMovement.Status == LogicMovementStatus.Error
                            || lastMovement.Status == LogicMovementStatus.Suspend
                        )
                    )
                    {
                        bool isNewLogicMovement;
                        task.Movements.Remove(lastMovement);

                        _logger.Debug1(string.Format("删除{0}（起点：{1}，终点{2}，路径Id：{3}）", lastMovement, lastMovement.StartLocation, lastMovement.EndLocation, lastMovement.RouteId), this, task);

                        var newMovement = task.GetNextMovement(unitOfWork, out isNewLogicMovement);

                        _logger.Debug1(string.Format("添加{0}（起点：{1}，终点{2}，路径Id：{3}）", newMovement, newMovement.StartLocation, newMovement.EndLocation, newMovement.RouteId), this, task);

                        newMovement.Status = LogicMovementStatus.Suspend;

                        foreach (var act in newMovement.EquipmentActions)
                        {
                            act.Status = EquipmentActionStatus.Suspend;
                        }

                        events.Add(new Wcs.Framework.Events.LogicMovementAddedEvent(newMovement));

                        foreach (var act in lastMovement.EquipmentActions)
                        {
                            var device = DeviceConverter.ToDevice<TaskableDevice>(act.DeviceName);
                            device.EquipmentActionScheduler.Remove(act);
                        }
                    }

                    unitOfWork.session.Flush();

                    unitOfWork.Commit();

                    _logger.Info1(String.Format("终点调整为{0}成功。", task.EndLocation), this, task);

                }

                lblEndLocation.Text = newEndLocation.UserCode;

                Wcs.Framework.EventBus.EventBus.Instance.Publish(events.ToArray());

                load(task.Id);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("调整任务终点失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }
    }
}
