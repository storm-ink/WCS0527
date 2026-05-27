using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Linq;
using Wcs.Framework.Cfg;
using Wcs.Framework.EventBus;
using Wcs;
using NLog;
using Wcs.FrameworkExtend;
using Newtonsoft.Json;

namespace Wcs.App.Plugins.PreTaskManager
{
    public partial class frmMain : Form
    {
        //WcsContext WcsContext;
        BindingSource _bindingSource;

        static Logger _logger = LogManager.GetCurrentClassLogger();
        public frmMain()
        {
            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<PreTask>();
            dgvGrid.DataSource = _bindingSource;

            var status = Wcs.EnumExtentions.ToKeyValueList<Framework.TaskStatus>();
            status.Insert(0, new KeyValuePair<string, string>("", ""));
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DataSource = status;

            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskAddedEvent>(onTaskAdded);
            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskArchivedEvent>(onTaskArchived);
            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskStatusChangedEvent>(onTaskStatusChanged);
            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskCurrentLocationChangedEvent>(onTaskCurrentLocationChanged);
            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskPriorityChangedEvent>(onTaskPriorityChanged);
            //Wcs.Framework.EventBus.EventBus.Instance.Subscribe<TaskUpdateEvent>(onTaskUpdate);

        }

        public frmMain(WcsContext context)
        {
            InitializeComponent();

            //WcsContext = context;
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<PreTask>();
            dgvGrid.DataSource = _bindingSource;

            var status = Wcs.EnumExtentions.ToKeyValueList<Framework.TaskStatus>();
            status.Insert(0, new KeyValuePair<string, string>("", ""));
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DataSource = status;

        }

        private void paging1_EventPaging(EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //WcsContext.Application.Logger.Error1(ex, this);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                load();
                paging1.Bind();
                this.paging1.EventPaging += new EventPagingHandler(paging1_EventPaging);//初始化自定义事件
            }
            catch (Exception ex)
            {
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务数据加载失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        void load()
        {
            List<PreTask> list = new List<PreTask>();
            var cmdState = Convert.ToString(cmbStatus.SelectedValue);
            new System.Threading.Tasks.TaskFactory().StartNew(() => {
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    paging1.TotalCount = unitOfWork.session.Query<PreTask>().Count();

                    var q = unitOfWork.session.Query<PreTask>();
                    if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                    {
                        q = q.Where(x => x.TaskCode.Contains(tbxTaskCode.Text.Trim()));
                    }

                    if (!string.IsNullOrWhiteSpace(tbxContainerCode.Text))
                    {
                        q = q.Where(x => x.ContainerCodes.Contains(tbxContainerCode.Text.Trim()));
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

                    if (!String.IsNullOrWhiteSpace(cmdState))
                    {
                        Framework.TaskStatus taskStatus = (Framework.TaskStatus)Enum.Parse(typeof(Framework.TaskStatus), Convert.ToString(cmbStatus.SelectedValue));
                        q = q.Where(x => x.Status == taskStatus);
                    }


                    list = q.ToList();
                    var key = tbxKey.Text.Trim();

                    Int32 keyIntValue;
                    Int32.TryParse(key, out keyIntValue);

                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        list = list.Where(x =>
                                        x.ContainerCodes.Contains(key) //条码
                                        || (x.Description != null && x.Description.Contains(key))
                                        || (x.TaskType != null && x.TaskType.Contains(key))
                                        || x.TaskCode.Contains(key) //任务号
                                        || x.Id == keyIntValue      //任务id
                                        || x.StartLocation.UserCode.Contains(key) //起点
                                        || x.EndLocation.UserCode.Contains(key)//终点
                                        || (x.MasterTaskCode + "").Contains(key)//父任务号
                                        ).ToList();
                    }

                    unitOfWork.Commit();
                }
            }).Wait();

            this.BeginInvoke(new Action(() =>
            {
                paging1.QueryCount = list.Count();

                if (paging1.PageSize >= 0)
                    list = list.Skip((paging1.CurrentPage - 1) * paging1.PageSize).Take(paging1.PageSize).ToList();

                _bindingSource.Clear();

                foreach (var item in list)
                {
                    _bindingSource.Add((PreTask)item);
                }
            }));
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                load();
                paging1.Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //WcsContext.Application.Logger.Error1(ex, this);
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
                var task = (PreTask)dgvGrid.CurrentRow.DataBoundItem;

                tsmiArchive.Enabled = task.Status == Framework.TaskStatus.Cancelled || task.Status == Framework.TaskStatus.Completed;
                tsmiComplete.Enabled = task.Status == Framework.TaskStatus.Suspend || task.Status == Framework.TaskStatus.Error;
                tsmiSuspend.Enabled = task.Status == Framework.TaskStatus.New || task.Status == Framework.TaskStatus.Executing;
                tsmiCancel.Enabled = task.Status == Framework.TaskStatus.Suspend || task.Status == Framework.TaskStatus.Error;
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\归档")]
        private void tsmiArchive_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;
                if (task.Status != Framework.TaskStatus.Cancelled && task.Status != Framework.TaskStatus.Completed)
                {
                    return;
                }
                String msg;
                if (task.Source == Framework.TaskSource.Wms)
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
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务删除失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\暂停")]
        private void tsmiSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;
                if (task.Status != Framework.TaskStatus.Executing && task.Status != Framework.TaskStatus.New)
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
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务暂停失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\取消")]
        private void tsmiCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;
                if (task.Status != Framework.TaskStatus.Suspend && task.Status != Framework.TaskStatus.Error)
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
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务取消失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\强制完成")]
        private void tsmiComplete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;
                if (task.Status != Framework.TaskStatus.Suspend && task.Status != Framework.TaskStatus.Error)
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
                    if (obj is PreTask)
                    {
                        Wcs.Framework.TaskHelper.Complete(obj.Id);
                    }
                    else if (obj is Framework.LogicMovement)
                    {
                        Wcs.Framework.TaskHelper.CompleteMovement(obj.Id);
                    }
                    else if (obj is Framework.EquipmentAction)
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
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\优先级调整")]
        private void toolStripMenuItem_priority_Click(object sender, EventArgs e)
        {
            Int32 priority = Convert.ToInt32((sender as ToolStripMenuItem).Text);
            if (MessageBox.Show("请检查是否为双伸堆垛机的远端任务，确认修改！", " 友情提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.OK)
            {
                updateTasksPriority(priority);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\优先级调整")]
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
                    .Where(x => (x.DataBoundItem as PreTask).Priority != priority && (x.DataBoundItem as PreTask).Status != Framework.TaskStatus.Cancelled && (x.DataBoundItem as PreTask).Status != Framework.TaskStatus.Completed)
                    .Select(x => (x.DataBoundItem as PreTask).Id)
                    .ToArray();

                if (taskids.Length == 0)
                {
                    return;
                }

                Wcs.Framework.TaskHelper.ChangePriority(priority, taskids);
            }
            catch (Exception ex)
            {
                //WcsContext.Application.Logger.Error1(ex, this);
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
                var task = dgvGrid.Rows[e.RowIndex].DataBoundItem.As<PreTask>();
                if (e.ColumnIndex == colContainerCode.Index)
                {
                    e.Value = string.Join(",", JsonConvert.DeserializeObject<List<string>>(task.ContainerCodes));
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
                    e.Value = string.Join(",", JsonConvert.DeserializeObject<Dictionary<string, string>>(task.AdditionalInfo).Select(x => String.Format("{0}={1}", x.Key, (x.Value + "").Trim())));
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
                    case Framework.TaskStatus.Completed:
                        _color = Color.Green;
                        break;
                    case Framework.TaskStatus.Cancelled:
                        _color = Color.Gray;
                        break;
                    case Framework.TaskStatus.Suspend:
                    case Framework.TaskStatus.Error:
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


        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\任务详情查看")]
        private void tsmiView_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;

                using (frmTaskViewer frm = new frmTaskViewer(task.Id, task.TaskCode))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\任务详情查看")]
        private void dgvGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            tsmiView_Click(null, null);
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\手动引发任务状态变化事件")]
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dgvGrid.CurrentRow == null)
            //    {
            //        return;
            //    }

            //    var taskId = (dgvGrid.CurrentRow.DataBoundItem as PreTask).Id;
            //    PreTask task;
            //    using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            //    {
            //        task = unitOfWork.session.Get<PreTask>(taskId);
            //        unitOfWork.Commit();
            //    }

            //    List<IEvent> events = new List<IEvent>();
            //    events.Add(new TaskStatusChangedEvent(task.Id, task.TaskCode, task.Status, task.BizType, task.Source, task.TaskType));
            //    if (task.Status == TaskStatus.Cancelled || task.Status == TaskStatus.Completed)
            //    {
            //        events.Add(new TaskFinishedEvent(task.Id, task.TaskCode, task.Status, task.BizType, task.Source, task.TaskType, task.FinishedAt.Value));
            //    }

            //    EventBus.Instance.Publish(events.ToArray());
            //}
            //catch (Exception ex)
            //{
            //    //WcsContext.Application.Logger.Error1(ex, this);
            //    MessageBox.Show(string.Format("引发任务状态改变事件失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //}
        }

        List<string> getMatchs(PreTask task)
        {
            var key = tbxKey.Text.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                return new List<string>();
            }
            Int32 keyIntValue;
            Int32.TryParse(key, out keyIntValue);

            List<string> matchs = new List<string>();
            if (task.ContainerCodes.Contains(key))
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

            return matchs;
        }

        private void tbxTaskCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\刷新任务列表")]
        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
    }
}
