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
using Wcs.FrameworkExtend;
using Newtonsoft.Json;

namespace Wcs.App.Plugins.PreTaskManager
{
    public partial class frmTaskViewer : Form
    {
        Logger _logger;

        TaskChooseEndLocationHandler taskChooseEndLocationHandler;

        Int32 _taskId;
        string _taskCoded;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\任务详情查看")]
        public frmTaskViewer(Int32 taskId,string taskCode)
        {
            _taskId = taskId;
            _taskCoded = taskCode;
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
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
        }
        void load(Int32 taskId)
        {
            PreTask task;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                task = unitOfWork.session.Get<PreTask>(taskId);

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
            tbx_description.Text = task.Description;
            if (string.IsNullOrWhiteSpace(task.AdditionalInfo))
                tbx_additionalinfo.Text = "-";
            else
                tbx_additionalinfo.Text = string.Join(",", JsonConvert.DeserializeObject<Dictionary<string,string>>(task.AdditionalInfo).Select(x => x.Key + "=" + x.Value));

            if (task.FinishedAt == null)
            {
                lblFinishedAt.Text = "-";
            }
            else
            {
                lblFinishedAt.Text = task.FinishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            //lblCurrentLocation.Text = string.Format("{0}@{1}", task.CurrentLocation.UserCode, task.CurrentLocation.DeviceName);
            lblContainerCodes.Text = string.Join(",", JsonConvert.DeserializeObject<List<string>>(task.ContainerCodes));

            btnChangeEndLocation.Visible = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("AblePreTaskChouseEndLocation", false);

            //setButtonsStatus(task.Status);
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

        private void frmTaskViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        #endregion

        //#region 任务处理
        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\归档")]
        //private void tsmiArchive_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MessageBox.Show("是否确认要归档任务？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
        //        {
        //            return;
        //        }

        //        _logger.Trace1(string.Format("准备人工归档 {0} ...", _taskId), this);
        //        Wcs.Framework.TaskHelper.Archive(_taskId);
        //        _logger.Trace1(string.Format("人工归档 {0} 成功", _taskId), this);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("任务归档失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (PreTaskSchedulerFilterHelper.lastPreTaskSchedulerFilterResult)
                {
                    var result = PreTaskSchedulerFilterHelper.lastPreTaskSchedulerFilterResult[_taskCoded];
                    textBox1.Text =$"是否被否决：{result.Defeated.ToString().ToUpper()}\r\n否决原因：{result.Reason}" ;
                }
            }
            catch
            {
            }
        }

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\暂停")]
        //private void tsmiSuspend_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (MessageBox.Show("是否确认要暂停任务？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) != System.Windows.Forms.DialogResult.Yes)
        //        {
        //            SetForegroundWindow(this.Handle);
        //            this.dgvMovementsGrid.Focus();
        //            Thread.Sleep(200);
        //            SetForegroundWindow(this.Handle);
        //            this.dgvMovementsGrid.Focus();
        //            return;
        //        }

        //        _logger.Trace1(string.Format("准备人工暂停 {0} ...", _taskId), this);
        //        Wcs.Framework.TaskHelper.Suspend(_taskId);
        //        _logger.Trace1(string.Format("人工暂停 {0} 成功", _taskId), this);
        //        //this.Activate();
        //        //this.Select(true, true);
        //        //this.Focus();
        //        SetForegroundWindow(this.Handle);
        //        this.dgvMovementsGrid.Focus();
        //        Thread.Sleep(200);
        //        SetForegroundWindow(this.Handle);
        //        this.dgvMovementsGrid.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("任务暂停失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\取消")]
        //private void tsmiCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (frmSetObjectToCompleted frm = new frmSetObjectToCompleted(_taskId))
        //        {
        //            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        //            {
        //                return;
        //            }

        //            var obj = frm.SelectedObject;

        //            if (obj is Task)
        //            {
        //                _logger.Trace1(string.Format("准备人工取消任务 {0} ...", _taskId), this);
        //                Wcs.Framework.TaskHelper.Cancle(obj, _taskId);
        //            }
        //            else if (obj is LogicMovement)
        //            {
        //                _logger.Trace1(string.Format("准备人工取消逻辑动作 {0} ...", _taskId), this);
        //                Wcs.Framework.TaskHelper.Cancle(obj, obj.Id);
        //            }
        //            else if (obj is EquipmentAction)
        //            {
        //                _logger.Trace1(string.Format("准备人工取消物理动作 {0} ...", _taskId), this);
        //                Wcs.Framework.TaskHelper.Cancle(obj, obj.Id);
        //            }
        //            else

        //                _logger.Trace1(string.Format("人工取消 {0} 成功", _taskId), this);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("任务取消失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\强制完成")]
        //private void tsmiComplete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //String msg = string.Format("在强制完成前请确认容器 {0} 是否已离开 {1}，并且已到达 {2}。\n\n是否继续？",
        //        //    String.Join(",", lblContainerCodes.Text),
        //        //    lblStartLocation.Text,
        //        //    lblEndLocation.Text);

        //        //if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
        //        //{
        //        //    return;
        //        //}

        //        //Wcs.Framework.TaskHelper.Complete(_taskId);

        //        using (frmSetObjectToCompleted frm = new frmSetObjectToCompleted(_taskId))
        //        {
        //            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        //            {
        //                return;
        //            }

        //            var obj = frm.SelectedObject;

        //            _logger.Trace1(string.Format("准备人工完成 {0} ...", (object)obj), this, (object)obj);
        //            if (obj is Task)
        //            {
        //                Wcs.Framework.TaskHelper.Complete(obj.Id);
        //            }
        //            else if (obj is LogicMovement)
        //            {
        //                Wcs.Framework.TaskHelper.CompleteMovement(obj.Id);
        //            }
        //            else if (obj is EquipmentAction)
        //            {
        //                Wcs.Framework.TaskHelper.CompleteAction(obj.Id);
        //            }
        //            else
        //            {
        //                throw new NotImplementedException(string.Format("未实现对 {0} 类型的任务的强制完成处理。", obj.GetType()));
        //            }
        //            _logger.Trace1(string.Format("人工完成 {0} 成功", (object)obj), this, (object)obj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("任务强制完成失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\继续执行")]
        //private void btnResume_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Task task;
        //        using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            task = unitOfWork.session.Get<Task>(_taskId);

        //            unitOfWork.Commit();
        //        }

        //        if (task == null)
        //        {
        //            MessageBox.Show(string.Format("未找到 id 为 {0} 的任务", _taskId), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return;
        //        }

        //        var stopRoute = TaskHelper.GetTaskResumeAtRoute(_taskId);
        //        if (stopRoute != null && stopRoute.AllowStartFromMidway)
        //        {
        //            using (frmResumeChooseCurrentLocation frm = new frmResumeChooseCurrentLocation(task, stopRoute))
        //            {
        //                if (frm.ShowDialog() == DialogResult.OK)
        //                {
        //                    _logger.Trace1(string.Format("准备人工继续执行 {0}，位置 {1} ...", task, frm.CurrentLocation), this, task);

        //                    Wcs.Framework.TaskHelper.Resume(_taskId, frm.CurrentLocation);

        //                    _logger.Trace1(string.Format("人工继续执行 {0} 成功", task), this, task);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var currentLocation = LocationConverter.ToLocation(task.CurrentLocation);

        //            _logger.Trace1(string.Format("准备人工继续执行 {0}，位置 {1} ...", task, currentLocation), this, task);



        //            Wcs.Framework.TaskHelper.Resume(_taskId, currentLocation);

        //            _logger.Trace1(string.Format("人工继续执行 {0} 成功", task), this, task);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("任务继续执行失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //    }
        //}
        //#endregion

        //private void btnEsc_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.Close();
        //}

        //private void dgvActionsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (dgvActionsGrid.CurrentRow == null)
        //    {
        //        return;
        //    }

        //    var action = dgvActionsGrid.CurrentRow.DataBoundItem.As<EquipmentAction>();
        //    if (action == null)
        //    {
        //        return;
        //    }

        //    var device = Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
        //        .Where(x => x.Device.Name == action.DeviceName)
        //        .Select(x => x.Device)
        //        .FirstOrDefault();

        //    if (device.GetType().FullName.Contains("Proxy.ClientDevice") && device.GetType().GetMethod("Show", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public) != null)
        //    {
        //        var mi = device.GetType().GetMethod("Show", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        //        mi.Invoke(device, null);
        //    }
        //}

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\当前任务\\调整终点")]
        //private void btnChangeEndLocation_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Task task;
        //        using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
        //        {
        //            task = unitOfWork.session.Get<Task>(_taskId);
        //            unitOfWork.Commit();
        //        }

        //        if (task == null)
        //        {
        //            MessageBox.Show(string.Format("未找到 id 为 {0} 的任务", _taskId), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return;
        //        }

        //        LocationInfo newEndLocation = null;
        //        if (task.Source == TaskSource.Wms)
        //        {
        //            if (taskChooseEndLocationHandler == null)
        //            {
        //                MessageBox.Show("未配置向WMS请求变更任务终点的处理程序");
        //                return;
        //            }
        //            var result = taskChooseEndLocationHandler.Hand(task, LocationConverter.UserCodeToLcation(task.CurrentLocation.UserCode));
        //            if (result)
        //                MessageBox.Show("请求变更成功");
        //            else
        //                MessageBox.Show("请求变更失败");
        //        }
        //        else
        //        {
        //            using (frmChooseNewEndLocation frm = new frmChooseNewEndLocation(task))
        //            {
        //                if (frm.ShowDialog() != DialogResult.OK)
        //                {
        //                    return;
        //                }

        //                newEndLocation = frm.NewEndLocation;
        //            }
        //        }
        //        if (newEndLocation == null)
        //            return;

        //        var path = RouteHelper.AbleArrived(LocationConverter.ToLocation(task.CurrentLocation), LocationConverter.ToLocation(newEndLocation));
        //        if (!path)
        //        {
        //            MessageBox.Show(String.Format("{0}到新的任务终点{1}不连通。", task.CurrentLocation, newEndLocation), Application.CompanyName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return;
        //        }

        //        List<Wcs.Framework.EventBus.IEvent> events = new List<Framework.EventBus.IEvent>();
        //        using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
        //        {
        //            task = unitOfWork.session.Get<UndefinedEndLocationTask>(task.Id);

        //            var lastMovement = task.Movements.OrderByDescending(x => x.CreatedAt)
        //                    .FirstOrDefault();

        //            _logger.Debug1(string.Format("人工开始将任务终点由{0}调整为{1}...", task.EndLocation, newEndLocation), this, task);

        //            task.EndLocation = newEndLocation;

        //            if (
        //                lastMovement != null &&
        //                (
        //                    lastMovement.Status == LogicMovementStatus.New
        //                    || lastMovement.Status == LogicMovementStatus.Error
        //                    || lastMovement.Status == LogicMovementStatus.Suspend
        //                )
        //            )
        //            {
        //                bool isNewLogicMovement;
        //                task.Movements.Remove(lastMovement);

        //                _logger.Debug1(string.Format("删除{0}（起点：{1}，终点{2}，路径Id：{3}）", lastMovement, lastMovement.StartLocation, lastMovement.EndLocation, lastMovement.RouteId), this, task);

        //                var newMovement = task.GetNextMovement(unitOfWork, out isNewLogicMovement);

        //                _logger.Debug1(string.Format("添加{0}（起点：{1}，终点{2}，路径Id：{3}）", newMovement, newMovement.StartLocation, newMovement.EndLocation, newMovement.RouteId), this, task);

        //                newMovement.Status = LogicMovementStatus.Suspend;

        //                foreach (var act in newMovement.EquipmentActions)
        //                {
        //                    act.Status = EquipmentActionStatus.Suspend;
        //                }

        //                events.Add(new Wcs.Framework.Events.LogicMovementAddedEvent(newMovement));

        //                foreach (var act in lastMovement.EquipmentActions)
        //                {
        //                    var device = DeviceConverter.ToDevice<TaskableDevice>(act.DeviceName);
        //                    device.EquipmentActionScheduler.Remove(act);
        //                }
        //            }

        //            unitOfWork.session.Flush();

        //            unitOfWork.Commit();

        //            _logger.Info1(String.Format("终点调整为{0}成功。", task.EndLocation), this, task);

        //        }

        //        lblEndLocation.Text = newEndLocation.UserCode;

        //        Wcs.Framework.EventBus.EventBus.Instance.Publish(events.ToArray());

        //        load(task.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error1(ex, this);
        //        MessageBox.Show(string.Format("调整任务终点失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //    }
        //}
    }
}
