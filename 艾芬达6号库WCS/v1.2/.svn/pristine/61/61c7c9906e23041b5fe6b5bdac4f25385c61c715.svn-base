using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;
using NHibernate.Linq;
using Wcs.Framework.Events;
using Wcs.Framework.Cfg;
using Wcs.Framework.EventBus;
using Wcs;
using NLog;
namespace Wcs.App.Plugins.TaskManager
{
    public partial class frmMain : Form
    {
        WcsContext WcsContext;
        BindingSource _bindingSource;

        static Logger _logger = LogManager.GetCurrentClassLogger();

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\查看")]
        public frmMain(WcsContext context)
        {
            InitializeComponent();

            WcsContext = context;
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<Task>();
            dgvGrid.DataSource = _bindingSource;

            var status = Wcs.EnumExtentions.ToKeyValueList<TaskStatus>();
            status.Insert(0, new KeyValuePair<string, string>("", ""));
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DataSource = status;

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskAddedEvent>(onTaskAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskArchivedEvent>(onTaskArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskCurrentLocationChangedEvent>(onTaskCurrentLocationChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskPriorityChangedEvent>(onTaskPriorityChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskUpdateEvent>(onTaskUpdate);
        }

        private void onTaskUpdate(TaskUpdateEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskUpdateEvent> act = (_args) =>
                    {
                        onTaskUpdate(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);
                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks == null)
                    {
                        return;
                    }
                    var task = tasks.FirstOrDefault(x => x.TaskCode == args.newTask.TaskCode);
                    if (task == null)
                    {
                        return;
                    }
                    
                    task.StartLocation = args.newTask.StartLocation;
                    task.EndLocation = args.newTask.EndLocation;
                    task.Description = args.ToDescription();

                    _bindingSource.ResetBindings(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务数据加载失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskAddedEvent>(onTaskAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskArchivedEvent>(onTaskArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskCurrentLocationChangedEvent>(onTaskCurrentLocationChanged);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<TaskPriorityChangedEvent>(onTaskPriorityChanged);
        }

        void load()
        {
            List<Task> list;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<Task>();
                if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                {
                    q = q.Where(x => x.TaskCode.Contains(tbxTaskCode.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxContainerCode.Text))
                {
                    q = q.Where(x => x.ContainerCodes.Any(containerCode => containerCode.Contains(tbxContainerCode.Text.Trim())));
                }

                if (!string.IsNullOrWhiteSpace(tbxStartLocation.Text))
                {
                    q = q.Where(x => x.StartLocation.UserCode.Contains(tbxStartLocation.Text.Trim())
                        || x.StartLocation.DeviceCode.Contains(tbxStartLocation.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxEndLocation.Text))
                {
                    q = q.Where(x => x.EndLocation.UserCode.Contains(tbxEndLocation.Text.Trim())
                        || x.EndLocation.DeviceCode.Contains(tbxEndLocation.Text.Trim()));
                }

                if (!String.IsNullOrWhiteSpace(Convert.ToString(cmbStatus.SelectedValue)))
                {
                    TaskStatus taskStatus = (TaskStatus)Enum.Parse(typeof(TaskStatus), Convert.ToString(cmbStatus.SelectedValue));
                    q = q.Where(x => x.Status == taskStatus);
                }


                list = q.ToList();
                var key = tbxKey.Text.Trim();

                Int32 keyIntValue;
                Int32.TryParse(key, out keyIntValue);

                if (!string.IsNullOrWhiteSpace(key))
                {
                    list = list.Where(x =>
                                    x.ContainerCodes.Any(containerCode => containerCode.Contains(key)) //条码
                                    || (x.Description != null && x.Description.Contains(key))
                                    || (x.TaskType != null && x.TaskType.Contains(key))
                                    || x.CurrentLocation.UserCode.Contains(key)
                                    || x.CurrentLocation.DeviceCode.Contains(key)
                                    || x.TaskCode.Contains(key) //任务号
                                    || x.Id == keyIntValue      //任务id
                                    || x.StartLocation.UserCode.Contains(key) //起点
                                    || x.EndLocation.UserCode.Contains(key)//终点
                                    || (x.MasterTaskCode + "").Contains(key)//父任务号
                                    || x.Movements
                                        .Any(movement => movement.Id == keyIntValue       //逻辑动作id
                                          || movement.RouteId == keyIntValue              //路径id
                                          || movement.DeviceName.Contains(key) //设备名称
                                          || movement.StartLocation.UserCode.Contains(key) //起点
                                          || movement.EndLocation.UserCode.Contains(key)//终点
                                          || movement.EquipmentActions.Any(action => action.Id == keyIntValue
                                                                                  || action.EquipmentTaskId == keyIntValue
                                                                                  || action.DeviceName.Contains(key) //设备名称
                                                                           )
                                            )
                                    ).ToList();
                }

                unitOfWork.Commit();
            }
            _bindingSource.Clear();

            foreach (var item in list)
            {
                _bindingSource.Add((Task)item);
            }
        }

        void onTaskAdded(TaskAddedEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskAddedEvent> act = (_args) =>
                    {
                        onTaskAdded(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);
                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks != null)
                    {
                        var task = tasks.FirstOrDefault(x => x.TaskCode == args.Task.TaskCode);
                        if (task != null)
                        {
                            task.Id = args.Task.Id;
                            task.CurrentLocation = args.Task.CurrentLocation;
                            task.Status = args.Task.Status;
                            task.Description = args.Task.Description;
                            task.ContainerCodes.Clear();
                            task.ContainerCodes.AddAll(args.Task.ContainerCodes);
                            task.BizType = args.Task.BizType;
                            task.AdditionalInfo = args.Task.AdditionalInfo;
                            task.CreatedAt = args.Task.CreatedAt;
                            task.Direction = args.Task.Direction;
                            task.StartLocation = args.Task.StartLocation;
                            task.EndLocation = args.Task.EndLocation;
                            task.FinishedAt = args.Task.FinishedAt;
                            task.FromRequest = args.Task.FromRequest;
                            task.Priority = args.Task.Priority;
                            task.Source = args.Task.Source;
                            task.TaskCode = args.Task.TaskCode;
                            task.TaskType = args.Task.TaskType;

                            _bindingSource.ResetBindings(false);
                            return;
                        }
                    }

                    _bindingSource.Add((Task)args.Task);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }

        void onTaskArchived(TaskArchivedEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskArchivedEvent> act = (_args) =>
                    {
                        onTaskArchived(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);
                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks != null)
                    {

                        var task = tasks.FirstOrDefault(x => x.TaskCode == args.TaskCode);
                        if (task == null)
                        {
                            return;
                        }

                        _bindingSource.Remove((Task)task);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }

        void onTaskStatusChanged(TaskStatusChangedEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskStatusChangedEvent> act = (_args) =>
                    {
                        onTaskStatusChanged(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);

                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks == null)
                    {
                        return;
                    }
                    var task = tasks.FirstOrDefault(x => x.TaskCode == args.TaskCode);
                    if (task == null)
                    {
                        return;
                    }

                    task.Status = args.Status;

                    _bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }

        void onTaskCurrentLocationChanged(TaskCurrentLocationChangedEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskCurrentLocationChangedEvent> act = (_args) =>
                    {
                        onTaskCurrentLocationChanged(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);

                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks == null)
                    {
                        return;
                    }

                    var task = tasks.FirstOrDefault(x => x.TaskCode == args.TaskCode);
                    if (task == null)
                    {
                        return;
                    }

                    task.CurrentLocation = args.CurrentLocation;

                    _bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }

        void onTaskPriorityChanged(TaskPriorityChangedEvent args)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action<TaskPriorityChangedEvent> act = (_args) =>
                    {
                        onTaskPriorityChanged(_args);
                    };

                    //this.Invoke(act, args);
                    this.BeginInvoke(act, args);
                }
                else
                {
                    List<Task> tasks = _bindingSource.Cast<Task>().ToList();
                    if (tasks == null)
                    {
                        return;
                    }

                    var task = tasks.FirstOrDefault(x => x.TaskCode == args.TaskCode);
                    if (task == null)
                    {
                        return;
                    }

                    task.Priority = args.Priority;

                    _bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, args);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                WcsContext.Application.Logger.Error1(ex, this);
            }
        }

        private void cmsMainTask_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.SelectedRows.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            tsmiPriority.Enabled = dgvGrid.SelectedRows.Count > 0;
            tsmiView.Enabled = dgvGrid.CurrentRow != null;

            tsmiSuspend.Enabled =
                tsmiArchive.Enabled =
                    tsmiCancel.Enabled =
                        tsmiComplete.Enabled = dgvGrid.CurrentRow != null && dgvGrid.SelectedRows.Count == 1;

            if (dgvGrid.SelectedRows.Count == 1)
            {
                var task = (Task)dgvGrid.CurrentRow.DataBoundItem;

                tsmiArchive.Enabled = task.Status == TaskStatus.Cancelled || task.Status == TaskStatus.Completed;
                tsmiComplete.Enabled = task.Status == TaskStatus.Suspend || task.Status == TaskStatus.Error;
                tsmiSuspend.Enabled = task.Status == TaskStatus.New || task.Status == TaskStatus.Executing;
                tsmiCancel.Enabled = task.Status == TaskStatus.Suspend || task.Status == TaskStatus.Error;
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\归档")]
        private void tsmiArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Task;
                if (task.Status != TaskStatus.Cancelled && task.Status != TaskStatus.Completed)
                {
                    return;
                }
                String msg;
                if (task.Source == TaskSource.Wms)
                {
                    msg = string.Format("{0} 来自于上层业务系统，人工归档可能会导致一些业务操作被忽略，从而导致业务系统的相关状态不一致。\n\n是否继续？", task);
                }
                else
                {
                    msg = string.Format("确定要归档 {0} 吗？", task);
                }
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                _logger.Trace1(string.Format("准备人工归档 {0} ...", task), this, task);
                Wcs.Framework.TaskHelper.Archive(task.Id);
                _logger.Trace1(string.Format("人工归档 {0} 成功", task), this, task);
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务删除失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\暂停")]
        private void tsmiSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Task;
                if (task.Status != TaskStatus.Executing && task.Status != TaskStatus.New)
                {
                    return;
                }

                String msg = string.Format("是否确认要暂停 {0}？", task);

                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                _logger.Trace1(string.Format("准备人工暂停 {0} ...", task), this, task);
                Wcs.Framework.TaskHelper.Suspend(task.Id);
                _logger.Trace1(string.Format("人工暂停 {0} 成功", task), this, task);
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务暂停失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\取消")]
        private void tsmiCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Task;
                if (task.Status != TaskStatus.Suspend && task.Status != TaskStatus.Error)
                {
                    return;
                }

                String msg = string.Format("是否确认要取消 {0}？", task);

                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                _logger.Trace1(string.Format("准备人工取消任务 {0} ...", task), this, task);
                Wcs.Framework.TaskHelper.CancelTask(task.Id);
                _logger.Trace1(string.Format("人工取消任务 {0} 成功", task), this, task);
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务取消失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\强制完成")]
        private void tsmiComplete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Task;
                if (task.Status != TaskStatus.Suspend && task.Status != TaskStatus.Error)
                {
                    return;
                }

                //String msg = string.Format("在强制完成前请确认容器 {0} 是否已离开 {1}，并且已到达 {2}。\n\n是否继续？",
                //    String.Join(",",task.ContainerCodes.ToArray()),
                //    task.StartLocation,
                //    task.EndLocation);

                //if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                //{
                //    return;
                //}

                //Wcs.Framework.TaskHelper.Complete (task.Id);

                using (frmSetObjectToCompleted frm = new frmSetObjectToCompleted(task.Id))
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
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\优先级调整")]
        private void toolStripMenuItem_priority_Click(object sender, EventArgs e)
        {
            Int32 priority = Convert.ToInt32((sender as ToolStripMenuItem).Text);
            if (MessageBox.Show("请检查是否为双伸堆垛机的远端任务，确认修改！", " 友情提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
            {
                updateTasksPriority(priority);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\优先级调整")]
        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Int32 priority;
                if (Int32.TryParse((sender as ToolStripTextBox).Text, out priority))
                {
                    (sender as ToolStripTextBox).Text = "";
                    updateTasksPriority(priority);
                    cmsMainTask.Hide();
                }
            }
        }

        private void updateTasksPriority(Int32 priority)
        {
            try
            {
                var taskids = dgvGrid.SelectedRows.Cast<DataGridViewRow>()
                    .Where(x => (x.DataBoundItem as Task).Priority != priority && (x.DataBoundItem as Task).Status != TaskStatus.Cancelled && (x.DataBoundItem as Task).Status != TaskStatus.Completed)
                    .Select(x => (x.DataBoundItem as Task).Id)
                    .ToArray();

                if (taskids.Length == 0)
                {
                    return;
                }

                Wcs.Framework.TaskHelper.ChangePriority(priority, taskids);
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("更改任务优先级失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
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
            try
            {
                var task = dgvGrid.Rows[e.RowIndex].DataBoundItem.As<Task>();
                if (e.ColumnIndex == colContainerCode.Index)
                {
                    e.Value = string.Join(",", task.ContainerCodes.ToArray());
                }
                else if (e.ColumnIndex == colStatus.Index)
                {
                    e.Value = task.Status.GetDescription();
                }
                else if (e.ColumnIndex == colBizType.Index)
                {
                    e.Value = task.BizType.GetDescription();
                }
                else if (e.ColumnIndex == colAdditionalInfo.Index)
                {
                    e.Value = string.Join(",", task.AdditionalInfo.Select(x => String.Format("{0}={1}", x.Key, (x.Value + "").Trim())));
                }
                else if (e.ColumnIndex == colTaskCode.Index)
                {
                    if (!String.IsNullOrWhiteSpace(task.MasterTaskCode)
                        && !String.Equals(task.MasterTaskCode, task.TaskCode, StringComparison.CurrentCultureIgnoreCase))
                    {
                        e.Value = String.Format("{0}({1})", task.TaskCode, task.MasterTaskCode);
                    }
                }

                Color _color;
                switch (task.Status)
                {
                    case TaskStatus.Completed:
                        _color = Color.Green;
                        break;
                    case TaskStatus.Cancelled:
                        _color = Color.Gray;
                        break;
                    case TaskStatus.Suspend:
                    case TaskStatus.Error:
                        _color = Color.Red;
                        break;
                    default:
                        if (Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings.ContainsKey("任务超时显示报警设置"))
                        {
                            var overTime = Convert.ToInt32(Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection._settings["任务超时显示报警设置"]);
                            var timespan = DateTime.Now.Subtract(task.CreatedAt).TotalMinutes;
                            if (timespan > overTime)
                                _color = Color.Gold;
                            else
                                _color = Control.DefaultForeColor;
                        }
                        else
                            _color = Control.DefaultForeColor;
                        break;
                }

                dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = _color;

                string match = string.Join(",", getMatchs(task).ToArray());
                foreach (DataGridViewColumn col in dgvGrid.Columns)
                {
                    dgvGrid.Rows[e.RowIndex].Cells[col.Index].ToolTipText = match;
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
        }


        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\任务详情查看")]
        private void tsmiView_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Task;

                using (frmTaskViewer frm = new frmTaskViewer(task.Id))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\任务详情查看")]
        private void dgvGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            tsmiView_Click(null, null);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\手动引发任务状态变化事件")]
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var taskId = (dgvGrid.CurrentRow.DataBoundItem as Task).Id;
                Task task;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Get<Task>(taskId);
                    unitOfWork.Commit();
                }

                List<IEvent> events = new List<IEvent>();
                events.Add(new TaskStatusChangedEvent(task.Id, task.TaskCode, task.Status, task.BizType, task.Source, task.TaskType));
                if (task.Status == TaskStatus.Cancelled || task.Status == TaskStatus.Completed)
                {
                    events.Add(new TaskFinishedEvent(task.Id, task.TaskCode, task.Status, task.BizType, task.Source, task.TaskType, task.FinishedAt.Value));
                }

                EventBus.Instance.Publish(events.ToArray());
            }
            catch (Exception ex)
            {
                WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("引发任务状态改变事件失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        List<string> getMatchs(Task task)
        {
            var key = tbxKey.Text.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                return new List<string>();
            }
            Int32 keyIntValue;
            Int32.TryParse(key, out keyIntValue);

            List<string> matchs = new List<string>();
            if (task.ContainerCodes.Any(containerCode => containerCode.Contains(key)))
            {
                matchs.Add(string.Format("条码号 {0} 包含", string.Join(",", task.ContainerCodes.ToArray())));
            }

            if (task.TaskCode.Contains(key))
            {
                matchs.Add(string.Format("任务号 {0} 包含", task.TaskCode));
            }

            if (task.StartLocation.UserCode.Contains(key))
            {
                matchs.Add(string.Format("任务起始位置 {0} 包含", task.StartLocation.UserCode));
            }

            if (task.EndLocation.UserCode.Contains(key))
            {
                matchs.Add(string.Format("任务结束位置 {0} 包含", task.EndLocation.UserCode));
            }

            if (task.Id == keyIntValue)
            {
                matchs.Add(string.Format("id 等于 {0}", task.Id));
            }

            foreach (var movement in task.Movements)
            {
                if (movement.Id == keyIntValue)
                {
                    matchs.Add(string.Format("id 等于 {0}", movement.Id));
                }

                if (movement.RouteId == keyIntValue)
                {
                    matchs.Add(string.Format("路径 id 等于 {0}", movement.RouteId));
                }

                if (movement.DeviceName.Contains(key))
                {
                    matchs.Add(string.Format("设备名 {0} 包含", movement.DeviceName));
                }

                if (movement.StartLocation.UserCode.Contains(key))
                {
                    matchs.Add(string.Format("任务起始位置 {0} 包含", movement.StartLocation.UserCode));
                }

                if (movement.EndLocation.UserCode.Contains(key))
                {
                    matchs.Add(string.Format("任务结束位置 {0} 包含", movement.EndLocation.UserCode));
                }

                foreach (var action in movement.EquipmentActions)
                {
                    if (action.Id == keyIntValue)
                    {
                        matchs.Add(string.Format("id 等于 {0}", action.Id));
                    }

                    if (action.EquipmentTaskId == keyIntValue)
                    {
                        matchs.Add(string.Format("设备任务号 等于 {0}", action.EquipmentTaskId));
                    }

                    if (action.DeviceName.Contains(key))
                    {
                        matchs.Add(string.Format("设备名 {0} 包含", action.DeviceName));
                    }
                }
            }

            return matchs;
        }

        private void tbxTaskCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\刷新任务列表")]
        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
    }
}
