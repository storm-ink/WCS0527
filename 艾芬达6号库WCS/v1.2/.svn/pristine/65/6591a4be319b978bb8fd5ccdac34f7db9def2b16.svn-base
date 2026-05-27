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

namespace Wcs.App.Plugins.ArchivedTaskManager
{
    public partial class frmTaskViewer : Form
    {
        Logger _logger;
        BindingSource _movementsBindingSource;
        BindingSource _actionsBindingSource;

        Int32 _taskId;
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
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            dgvMovementsGrid.CellFormatting += dgvMovementsGrid_CellFormatting;
            dgvActionsGrid.CellFormatting += dgvActionsGrid_CellFormatting;
            _movementsBindingSource.CurrentItemChanged += _movementsBindingSource_CurrentItemChanged;

        }

        void load(Int32 taskId)
        {
            Task task = null;
            new System.Threading.Tasks.TaskFactory().StartNew(() =>
            {
                using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Get<Task>(taskId);

                    unitOfWork.Commit();
                }
            }).Wait();

            this.BeginInvoke(new Action(() =>
            {

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
                if (task.AdditionalInfo == null || task.AdditionalInfo.Count == 0)
                    tbx_additionalinfo.Text = "-";
                else
                    tbx_additionalinfo.Text = string.Join(",", task.AdditionalInfo.Select(x => x.Key + "=" + x.Value));

                var logicMovement = task.Movements.FirstOrDefault();
                DateTime? sentAt = null;
                if (logicMovement != null)
                {
                    var equipmentAction = logicMovement.EquipmentActions.FirstOrDefault();
                    if (equipmentAction != null)
                    {
                        if (equipmentAction.SentAt != null)
                        {
                            lblSentAt.Text = equipmentAction.SentAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            sentAt = equipmentAction.SentAt;
                        }
                        else if (equipmentAction.FinishedAt != null)
                        {
                            lblSentAt.Text = equipmentAction.FinishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            sentAt = equipmentAction.FinishedAt;
                        }
                        else
                        {
                            lblSentAt.Text = equipmentAction.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                            sentAt = equipmentAction.CreatedAt;
                        }
                    }
                    else
                        lblSentAt.Text = "-";
                }
                else
                    lblSentAt.Text = "-";
                if (task.FinishedAt == null)
                {
                    lblFinishedAt.Text = "-";
                    lbltotalTimeSpan.Text = "-";
                    lblRunningTimeSpan.Text = "-";
                    lblWaitingTimeSpan.Text = "-";
                }
                else
                {
                    lblFinishedAt.Text = task.FinishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    var total = task.FinishedAt.Value.Subtract(task.CreatedAt);
                    lbltotalTimeSpan.Text = total.ToString();
                    var running = calRunningTimeSpan(task);
                    if (running == null)
                    {
                        lblRunningTimeSpan.Text = "-";
                        lblWaitingTimeSpan.Text = "-";
                    }
                    else
                    {
                        lblRunningTimeSpan.Text = running.ToString();
                        lblWaitingTimeSpan.Text = (total - running).ToString();
                    }
                }
                lblCurrentLocation.Text = string.Format("{0}@{1}", task.CurrentLocation.UserCode, task.CurrentLocation.DeviceName);
                lblContainerCodes.Text = string.Join(",", task.ContainerCodes.ToArray());

                _movementsBindingSource.Clear();
                foreach (var item in task.Movements.OrderBy(x => x.Id))
                {
                    _movementsBindingSource.Add((LogicMovement)item);
                }

                _movementsBindingSource_CurrentItemChanged(null, null);

                #region 补充之前没有增加时间统计的处理
                bool update = false;
                if (!task.AdditionalInfo.ContainsKey("TTS"))
                {
                    task.AdditionalInfo.Add("TTS", lbltotalTimeSpan.Text.Trim());
                    update = true;
                }
                if (!task.AdditionalInfo.ContainsKey("RTS"))
                {
                    task.AdditionalInfo.Add("RTS", lblRunningTimeSpan.Text.Trim());
                    update = true;
                }
                if (!task.AdditionalInfo.ContainsKey("WTS"))
                {
                    task.AdditionalInfo.Add("WTS", lblWaitingTimeSpan.Text.Trim());
                    update = true;
                }
                if (update)
                {
                    using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork())
                    {
                        unitOfWork.session.Update(task);
                        unitOfWork.Commit();
                    }
                }
                #endregion
            }));
        }

        private TimeSpan? calRunningTimeSpan(Task task)
        {
            try
            {
                if (task.Movements == null)
                    return null;
                TimeSpan runningTimeSpan = new TimeSpan();
                foreach (var logicMovement in task.Movements)
                {
                    if (logicMovement.EquipmentActions == null)
                        continue;
                    foreach (var action in logicMovement.EquipmentActions)
                    {
                        if (action.FinishedAt == null)
                            continue;
                        if (action.SentAt == null)
                            continue;

                        runningTimeSpan += action.FinishedAt.Value - action.SentAt.Value;
                    }
                }
                return runningTimeSpan;
            }
            catch
            {
                return null;
            }
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
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            else if (e.ColumnIndex == colSentAt.Index)
            {
                if (task.EquipmentActions == null || task.EquipmentActions.Count() == 0)
                {
                    e.Value = "-";
                }
                else
                {
                    var action = task.EquipmentActions.FirstOrDefault();
                    if (action == null || action.SentAt == null)
                    {
                        e.Value = "-";
                    }
                    else
                        e.Value = action.SentAt.Value.ToString("HH:mm:ss");
                }
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
                    //var route = Wcs.Framework.Cfg.WcsConfiguration.Instance.RouteCollection.Routes.SingleOrDefault(x => x.Id == task.RouteId.Value);
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
            else if (e.ColumnIndex == colRunningTime.Index)
            {
                if (task.EquipmentActions == null || task.EquipmentActions.Count() == 0 || task.FinishedAt == null)
                {
                    e.Value = "-";
                }
                else
                {
                    var action = task.EquipmentActions.FirstOrDefault();
                    if (action == null || action.SentAt == null)
                    {
                        e.Value = "-";
                    }
                    else
                        e.Value = task.FinishedAt.Value.Subtract(action.SentAt.Value).ToString();
                }
            }
            else if (e.ColumnIndex == colWaitingTime.Index)
            {
                if (task.EquipmentActions == null || task.EquipmentActions.Count() == 0)
                {
                    e.Value = "-";
                }
                else
                {
                    var action = task.EquipmentActions.FirstOrDefault();
                    if (action == null || action.SentAt == null)
                    {
                        e.Value = "-";
                    }
                    else
                        e.Value = action.SentAt.Value.Subtract(action.CreatedAt).ToString();
                }
            }
            else if (e.ColumnIndex == coltotalTimeSpan.Index)
            {
                if (task.FinishedAt == null)
                {
                    e.Value = "-";
                }
                else
                {
                    e.Value = task.FinishedAt.Value.Subtract(task.CreatedAt).ToString();
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
            dgvMovementsGrid.CellFormatting -= dgvMovementsGrid_CellFormatting;
            dgvActionsGrid.CellFormatting -= dgvActionsGrid_CellFormatting;
            _movementsBindingSource.CurrentItemChanged -= _movementsBindingSource_CurrentItemChanged;
        }
        #endregion

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
