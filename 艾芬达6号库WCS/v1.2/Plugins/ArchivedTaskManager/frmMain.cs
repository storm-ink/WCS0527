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
using System.Collections;
using NHibernate.Transform;
using NLog;

namespace Wcs.App.Plugins.ArchivedTaskManager
{
    public partial class frmMain : Form
    {
        const string NullTaskType = "【空字符串】";
        //WcsContext WcsContext;
        BindingSource _bindingSource;
        Logger _logger = LogManager.GetCurrentClassLogger();
        public frmMain()
        {
            InitializeComponent();
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<Hashtable>();
            dgvGrid.DataSource = _bindingSource;
        }

        public frmMain(WcsContext context)
            : this()
        {

            //WcsContext = context;
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<Hashtable>();
            dgvGrid.DataSource = _bindingSource;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {
                loadTaskTypes();
            }, null);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        //        void load()
        //        {
        //            List<Task> list;
        //            using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
        //            {
        //                Dictionary<string, object> pars = new Dictionary<string,object>();

        //                var hql = @"
        //select t
        //from Task t
        //join t.Movements m
        //join m.EquipmentActions a
        //where 1=1 ";
        //                if (dtpStartDate.Checked)
        //                {
        //                    hql += " and t.CreatedAt>=:startDate";
        //                    pars.Add("startDate",dtpStartDate.Value);
        //                }

        //                if (dtpEndDate.Checked)
        //                {
        //                    hql += " and t.CreatedAt<:endDate";
        //                    pars.Add("endDate", dtpEndDate.Value);
        //                }

        //                if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
        //                {
        //                    hql += " and t.TaskCode like :taskCode";
        //                    pars.Add("taskCode", '%'+tbxTaskCode.Text.Trim()+'%');
        //                }

        //                if (!string.IsNullOrWhiteSpace(tbxStartLocation.Text))
        //                {
        //                    hql += " and (t.StartLocation.UserCode like :startLocation or t.StartLocation.DeviceCode like :startLocation)";
        //                    pars.Add("startLocation", '%' + tbxStartLocation.Text.Trim() + '%');
        //                }

        //                if (!string.IsNullOrWhiteSpace(tbxEndLocation.Text))
        //                {
        //                    hql += " and (t.EndLocation.UserCode like :endLocation or t.EndLocation.DeviceCode like :endLocation)";
        //                    pars.Add("endLocation", '%' + tbxEndLocation.Text.Trim() + '%');
        //                }

        //                if (cbxFromWms.CheckState == CheckState.Checked)
        //                {
        //                    hql += " and t.Source=:source";
        //                    pars.Add("source", TaskSource.Wms);
        //                }
        //                else if (cbxFromWms.CheckState == CheckState.Unchecked)
        //                {
        //                    hql += " and t.Source<>:source";
        //                    pars.Add("source", TaskSource.Wms);
        //                }

        //                Int32 equipmentTaskId;
        //                if (int.TryParse(tbxEquipmentTaskID.Text, out equipmentTaskId) && equipmentTaskId > 0)
        //                {
        //                    hql += " and a.EquipmentTaskId=:equipmentTaskId";
        //                    pars.Add("equipmentTaskId", equipmentTaskId);
        //                }

        //                var q2 = unitOfWork.session.CreateQuery(hql);


        //                foreach (var p in pars)
        //                {
        //                    q2.SetParameter(p.Key, p.Value);
        //                }

        //                list = q2
        //                    .List<Task>()
        //                    .Take(Convert.ToInt32(numericUpDown1.Value))
        //                    .ToList();

        //                unitOfWork.Commit();
        //            }

        //            _bindingSource.Clear();

        //            foreach (var item in list)
        //            {
        //                _bindingSource.Add(item);
        //            }
        //        }

        void load()
        {
            IList<Hashtable> list;
            using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                Dictionary<string, object> pars = new Dictionary<string, object>();

                var sql = string.Format(@"
SELECT top {0} t.*
,StartLocation_UserCode as StartLocation
,EndLocation_UserCode as EndLocation
,CurrentLocation_UserCode as CurrentLocation
	  FROM [Tasks] t
	  where 1 =1", Convert.ToInt32(numericUpDown1.Value)); ;

                if (dtpStartDate.Checked)
                {
                    sql += " and t.CreatedAt>=:startDate";
                    pars.Add("startDate", dtpStartDate.Value);
                }

                if (dtpEndDate.Checked)
                {
                    sql += " and t.CreatedAt<:endDate";
                    pars.Add("endDate", dtpEndDate.Value);
                }

                if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                {
                    sql += " and t.TaskCode like :taskCode";
                    pars.Add("taskCode", '%' + tbxTaskCode.Text.Trim() + '%');
                }

                if (!string.IsNullOrWhiteSpace(cbxTaskTypes.Text))
                {
                    var v = cbxTaskTypes.Text.Trim();
                    if (v == NullTaskType)
                    {
                        sql += " and (t.TaskType = '' or t.TaskType is null)";
                    }
                    else
                    {
                        sql += " and t.TaskType like :taskType";
                        pars.Add("taskType", '%' + cbxTaskTypes.Text.Trim() + '%');
                    }
                }

                if (!string.IsNullOrWhiteSpace(tbxStartLocation.Text))
                {
                    sql += " and (t.StartLocation_UserCode like :startLocation or t.StartLocation_DeviceCode like :startLocation)";
                    pars.Add("startLocation", '%' + tbxStartLocation.Text.Trim() + '%');
                }

                if (!string.IsNullOrWhiteSpace(tbxEndLocation.Text))
                {
                    sql += " and (t.EndLocation_UserCode like :endLocation or t.EndLocation_DeviceCode like :endLocation)";
                    pars.Add("endLocation", '%' + tbxEndLocation.Text.Trim() + '%');
                }

                if (cbxFromWms.CheckState == CheckState.Checked)
                {
                    sql += " and t.Source=:source";
                    pars.Add("source", (int)TaskSource.Wms);
                }
                else if (cbxFromWms.CheckState == CheckState.Unchecked)
                {
                    sql += " and t.Source<>:source";
                    pars.Add("source", (int)TaskSource.Wms);
                }

                Int32 equipmentTaskId;
                if (int.TryParse(tbxEquipmentTaskID.Text, out equipmentTaskId) && equipmentTaskId > 0)
                {
                    sql += @" and exists(
			            select m.Id from LogicMovements m
			            join EquipmentActions a on m.Id=a.LogicMovementId
			            where a.EquipmentTaskId=:equipmentTaskId
	               )";
                    pars.Add("equipmentTaskId", equipmentTaskId);
                }

                if (!String.IsNullOrWhiteSpace(tbxContainerCode.Text))
                {
                    sql += " and exists(select 1 from TaskContainerCodes where TaskId=t.Id and Value =:containerCode)";
                    pars.Add("containerCode", tbxContainerCode.Text.Trim());
                }

                sql = string.Format(@"select *,STUFF(
  (select ','+tc.value from [TaskContainerCodes] tc where tc.TaskId=tmp.id  FOR XML PATH('')),1,1,''
    ) as [ContainerCode]
    ,
 STUFF(
  (select ','+tai.[Key] + '='+tai.value from [TaskAdditionalInfo] tai where tmp.Id=tai.TaskId  FOR XML PATH('')),1,1,''
    ) as [TaskAdditionalInfo]
 from (
	  {0} order by t.Id desc
  ) tmp order by Id desc", sql);

                var q2 = unitOfWork.session.CreateSQLQuery(sql);


                foreach (var p in pars)
                {
                    q2.SetParameter(p.Key, p.Value);
                }

                q2.SetResultTransformer(NHibernate.Transform.Transformers.AliasToEntityMap);

                list = q2.List<Hashtable>();

                unitOfWork.Commit();
            }

            _bindingSource.Clear();

            foreach (var item in list)
            {
                _bindingSource.Add(item);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {
                try
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            groupBox1.Enabled =
                            dgvGrid.Enabled = false;
                            load();

                            if (Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("CheckTaskTimeSpans", false))
                            {
                                foreach (DataGridViewRow item in dgvGrid.Rows)
                                {
                                    var tsk = item.DataBoundItem as Hashtable;
                                    if (!tsk.ContainsKey("Id") || tsk["Id"] == null)
                                        continue;

                                    var taskid = Convert.ToInt32(tsk["Id"]);
                                    Task task;
                                    using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork())
                                    {
                                        task = unitOfWork.session.Get<Task>(taskid);
                                        if (task != null)
                                        {
                                            if (task.AdditionalInfo == null || !task.AdditionalInfo.ContainsKey("TTS") || !task.AdditionalInfo.ContainsKey("RTS") || !task.AdditionalInfo.ContainsKey("WTS"))
                                            {
                                                var total = task.FinishedAt.Value.Subtract(task.CreatedAt);
                                                if (task.AdditionalInfo.ContainsKey("TTS"))
                                                    task.AdditionalInfo["TTS"] = total.ToString();
                                                else
                                                    task.AdditionalInfo.Add("TTS", total.ToString());

                                                var rts = TaskHelper.CalTaskRunningTimeSpan(task);
                                                if (rts == null)
                                                    rts = new TimeSpan();
                                                if (task.AdditionalInfo.ContainsKey("RTS"))
                                                    task.AdditionalInfo["RTS"] = rts.ToString();
                                                else
                                                    task.AdditionalInfo.Add("RTS", rts.ToString());

                                                var wts = total - rts;
                                                if (task.AdditionalInfo.ContainsKey("WTS"))
                                                    task.AdditionalInfo["WTS"] = wts.ToString();
                                                else
                                                    task.AdditionalInfo.Add("WTS", wts.ToString());

                                                unitOfWork.session.Update(task);
                                            }
                                        }
                                        unitOfWork.Commit();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error1(ex, this);
                            MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //WcsContext.Application.Logger.Error1(ex, this);
                        }
                        finally
                        {
                            groupBox1.Enabled =
                           dgvGrid.Enabled = true;
                        }

                    }));
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }

            }, null);
        }

        private void dgvList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                //MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var task = dgvGrid.Rows[e.RowIndex].DataBoundItem.As<Hashtable>();

            e.Value = task[dgvGrid.Columns[e.ColumnIndex].DataPropertyName];

            if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = ((TaskStatus)e.Value).GetDescription();
            }
            else if (e.ColumnIndex == colBizType.Index)
            {
                e.Value = ((TaskBizType)e.Value).GetDescription();
            }
            else if (e.ColumnIndex == colTaskCode.Index)
            {
                if (!String.IsNullOrWhiteSpace(Convert.ToString(task["MasterTaskCode"]))
                    && !String.Equals(Convert.ToString(task["MasterTaskCode"]), Convert.ToString(task["TaskCode"]), StringComparison.CurrentCultureIgnoreCase))
                {
                    e.Value = String.Format("{0}({1})", task["TaskCode"], task["MasterTaskCode"]);
                }
            }
            else if (e.ColumnIndex == colFinishedAt.Index)
            {
                if (task["FinishedAt"] == null)
                    e.Value = "-";
                else
                    e.Value = task["FinishedAt"];
            }
            else if (e.ColumnIndex == colTotalTime.Index)
            {
                if (task["FinishedAt"] == null)
                    e.Value = "-";
                else
                    e.Value = ((DateTime)task["FinishedAt"]).Subtract((DateTime)task["CreatedAt"]);
            }
            else if (e.ColumnIndex == colRunningTime.Index)
            {
                string taskAdditionalInfo = "";
                if (task["TaskAdditionalInfo"] != null)
                    taskAdditionalInfo = task["TaskAdditionalInfo"].ToString();

                if (string.IsNullOrWhiteSpace(taskAdditionalInfo))
                    e.Value = "";
                else
                {
                    var rts = taskAdditionalInfo.Split(',').FirstOrDefault(x => x.StartsWith("RTS"));
                    if (string.IsNullOrWhiteSpace(rts))
                        e.Value = "";
                    else
                    {
                        var _rts = rts.Split('=').ToArray();
                        if (_rts.Length == 2)
                            e.Value = _rts[1];
                        else
                            e.Value = "";
                    }
                }
            }
            else if (e.ColumnIndex == colWaitTime.Index)
            {
                string taskAdditionalInfo = "";
                if (task["TaskAdditionalInfo"] != null)
                    taskAdditionalInfo = task["TaskAdditionalInfo"].ToString();

                if (string.IsNullOrWhiteSpace(taskAdditionalInfo))
                    e.Value = "";
                else
                {
                    var wts = taskAdditionalInfo.Split(',').FirstOrDefault(x => x.StartsWith("WTS"));
                    if (string.IsNullOrWhiteSpace(wts))
                        e.Value = "";
                    else
                    {
                        var _wts = wts.Split('=').ToArray();
                        if (_wts.Length == 2)
                            e.Value = _wts[1];
                        else
                            e.Value = "";
                    }
                }
            }

            var status = (TaskStatus)task["Status"];
            switch (status)
            {
                case TaskStatus.Completed:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    break;
                case TaskStatus.Cancelled:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                    break;
                case TaskStatus.Suspend:
                case TaskStatus.Error:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    break;
                default:
                    dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Control.DefaultForeColor;
                    break;
            }
        }

        private void tsmiView_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as Hashtable;

                using (frmTaskViewer frm = new frmTaskViewer(Convert.ToInt32(task["Id"])))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("打开任务详情失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            tsmiView_Click(null, null);
        }

        private void tbxTaskCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.CheckFileExists = false;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.FileName = "日志";
                    saveFileDialog.Filter = "XML 电子表格 2003|*.xml";
                    saveFileDialog.ValidateNames = true;
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    {
                        return;
                    }

                    fileName = saveFileDialog.FileName;
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return;
                }

                dgvGrid.ExportAsExcel(fileName);

                if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void loadTaskTypes()
        {
            try
            {
                using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var taskTypes = unitOfWork.session.Query<Task>()
                        .GroupBy(x => x.TaskType)
                        .Select(x => x.Key)
                        .ToArray();

                    unitOfWork.Commit();

                    this.Invoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            cbxTaskTypes.Items.Clear();
                            cbxTaskTypes.Items.Add("");
                            foreach (var item in taskTypes)
                            {
                                if (String.IsNullOrWhiteSpace(item))
                                {
                                    var key = "【空字符串】";
                                    cbxTaskTypes.Items.Add(key);
                                }
                                else
                                {
                                    cbxTaskTypes.Items.Add(item);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error1(ex, this);
                            //WcsContext.Application.Logger.Error1(ex, this);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                //WcsContext.Application.Logger.Error1(ex, this);
            }
        }
    }
}
