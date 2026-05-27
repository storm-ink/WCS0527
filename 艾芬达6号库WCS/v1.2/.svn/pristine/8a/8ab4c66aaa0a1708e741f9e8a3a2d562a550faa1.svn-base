using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.Reports
{
    public partial class frmConveyorWarningReport : Form
    {
        public frmConveyorWarningReport()
        {
            InitializeComponent();

            dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEndTime.Value = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        void xx()
        {
            int posNo;
            int.TryParse(textBox1.Text,out posNo);


            var warnings = findWarnings(cbxDeviceName.Text, posNo <=0 ? null : (int?)posNo, dtpStartTime.Value, dtpEndTime.Value);
           

            List<List<ConveyorWarningExecutionTime>> reports = new List<List<ConveyorWarningExecutionTime>>();
            reports.Add(findExecutionTimes(warnings, "是否手动", x => x.Manual));
            reports.Add(findExecutionTimes(warnings, "离线（隔离开关断开）", x => x.Isolator));
            reports.Add(findExecutionTimes(warnings, "电路保护器断开（断路器断开）", x => x.Breaker));
            reports.Add(findExecutionTimes(warnings, "光电异常", x => x.Photocell));
            reports.Add(findExecutionTimes(warnings, "运行超时", x => x.RunOvertime));
            reports.Add(findExecutionTimes(warnings, "占位超时", x => x.OccupyOvertime));
            reports.Add(findExecutionTimes(warnings, "有任务无货", x => x.TaskNoGoods));
            reports.Add(findExecutionTimes(warnings, "X轴电机变频器故障", x => x.X_MotorVAF));
            reports.Add(findExecutionTimes(warnings, "Y轴电机变频器故障", x => x.Y_MotorVAF));
            reports.Add(findExecutionTimes(warnings, "X轴电机正反转接触器故障", x => x.X_MotorContactor));
            reports.Add(findExecutionTimes(warnings, "X轴电机抱闸接触器故障", x => x.X_MotorBraker));
            reports.Add(findExecutionTimes(warnings, "Y轴电机正反转接触器故障", x => x.Y_MotorContactor));
            reports.Add(findExecutionTimes(warnings, "Y轴电机抱闸接触器故障", x => x.Y_MotorBraker));
            reports.Add(findExecutionTimes(warnings, "顶升电机正反转接触器故障", x => x.Lift_MotorContactor));
            reports.Add(findExecutionTimes(warnings, "顶升电机抱闸接触器故障", x => x.Lift_MotorBraker));
            

            reports = reports
                .Where(x => x.Count > 0)
                .ToList();
            this.Invoke(new MethodInvoker(() =>
            {

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = reports
                    .SelectMany(x => x)
                    .OrderBy(x => x.From)
                    .ToList();

                if (reports.Count > 0)
                {
                    dgvGridCount.Rows.Clear();
                    dgvGridCount.RowCount = reports.Count + 1;
                    var count = Convert.ToDouble(reports.Sum(x => x.Count));
                    var totalSencods = reports.Sum(x => x.Sum(y => y.TotalSeconds));
                    for (int i = 0; i < reports.Count; i++)
                    {
                        var item = reports[i];
                        dgvGridCount.Rows[i].Tag = item;
                        dgvGridCount.Rows[i].Cells[0].Value = item.First().Name;
                        dgvGridCount.Rows[i].Cells[1].Value = item.Count;
                        dgvGridCount.Rows[i].Cells[2].Value = String.Format("{0:0.####}%", (item.Count / count) * 100);
                        dgvGridCount.Rows[i].Cells[3].Value = item.Sum(x => x.TotalSeconds);
                        dgvGridCount.Rows[i].Cells[4].Value = String.Format("{0:0.####}%", (item.Sum(x => x.TotalSeconds) / totalSencods) * 100);
                    }

                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].DefaultCellStyle.BackColor = dgvGridCount.Rows[dgvGridCount.RowCount - 1].DefaultCellStyle.SelectionBackColor;
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].DefaultCellStyle.ForeColor = Color.Green;
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].Cells[0].Value = "合计";
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].Cells[1].Value = count;
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].Cells[2].Value = "100%";
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].Cells[3].Value = totalSencods;
                    dgvGridCount.Rows[dgvGridCount.RowCount - 1].Cells[4].Value = "100%";

                }
                else
                {
                    dgvGridCount.Rows.Clear();
                    dgvCraneDetails.Rows.Clear();
                    dataGridView1.DataSource = null;
                }

            }));
        }

        void bindCraneDetails(List<ConveyorWarningExecutionTime> datas)
        {
            var grouping = datas.GroupBy(x => x.DeviceName)
                .ToList();
            dgvCraneDetails.RowCount = grouping.Count;
            for (int i = 0; i < grouping.Count; i++)
            {
                var data = grouping[i];
                var row = dgvCraneDetails.Rows[i];
                row.Cells[0].Value = data.First().DeviceName;
                row.Cells[1].Value = data.Count();
                row.Cells[2].Value = string.Format("{0:0.####}%",data.Count() / Convert.ToDouble(datas.Count) * 100);

                row.Cells[3].Value = data.Sum(x => x.TotalSeconds);
                row.Cells[4].Value = string.Format("{0:0.####}%", data.Sum(x=>x.TotalSeconds) / Convert.ToDouble(datas.Sum(x=>x.TotalSeconds)) * 100);
            }
        }
        
        private void dgvGridCount_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGridCount.CurrentRow == null || dgvGridCount.CurrentRow.Tag == null)
            {
                dgvCraneDetails.Rows.Clear();
                return;
            }

            bindCraneDetails((List<ConveyorWarningExecutionTime>)dgvGridCount.CurrentRow.Tag);
        }

        string timeSpanToStr(TimeSpan tm)
        {
            StringBuilder sb = new StringBuilder();
            if (tm.Seconds > 0)
            {
                sb.Insert(0, string.Format("{0}秒", tm.Seconds));
            }
            if (tm.Minutes > 0)
            {
                sb.Insert(0, string.Format("{0}分", tm.Minutes));
            }
            if (tm.Hours > 0)
            {
                sb.Insert(0, string.Format("{0}小时", tm.Hours));
            }
            if (tm.Days > 0)
            {
                sb.Insert(0, string.Format("{0}天", tm.Days));
            }

            return sb.ToString();
        }
        class ConveyorWarning
        {
            public Int32 Id { get; set; }
            public String DeviceName { get; set; }
            public DateTime CreatedAt { get; set; }
            /// <summary>
            /// 货位号
            /// </summary>
            public Int32 PosNo { get; set; }
            /// <summary>
            /// 是否手动
            /// </summary>
            public Boolean Manual { get; set; }
            /// <summary>
            /// 离线（隔离开关断开）
            /// </summary>
            public Boolean Isolator { get; set; }
            /// <summary>
            /// 电路保护器断开（断路器断开）
            /// </summary>
            public Boolean Breaker { get; set; }
            /// <summary>
            /// 光电异常
            /// </summary>
            public Boolean Photocell { get; set; }
            /// <summary>
            /// 运行超时
            /// </summary>
            public Boolean RunOvertime { get; set; }
            /// <summary>
            /// 占位超时
            /// </summary>
            public Boolean OccupyOvertime { get; set; }
            /// <summary>
            /// 有任务无货
            /// </summary>
            public Boolean TaskNoGoods { get; set; }
            /// <summary>
            /// X轴电机变频器故障
            /// </summary>
            public Boolean X_MotorVAF { get; set; }
            /// <summary>
            /// Y轴电机变频器故障
            /// </summary>
            public Boolean Y_MotorVAF { get; set; }
            /// <summary>
            /// X轴电机正反转接触器故障
            /// </summary>
            public Boolean X_MotorContactor { get; set; }
            /// <summary>
            /// X轴电机抱闸接触器故障
            /// </summary>
            public Boolean X_MotorBraker { get; set; }
            /// <summary>
            /// Y轴电机正反转接触器故障
            /// </summary>
            public Boolean Y_MotorContactor { get; set; }
            /// <summary>
            /// Y轴电机抱闸接触器故障
            /// </summary>
            public Boolean Y_MotorBraker { get; set; }
            /// <summary>
            /// 顶升电机正反转接触器故障
            /// </summary>
            public Boolean Lift_MotorContactor { get; set; }
            /// <summary>
            /// 顶升电机抱闸接触器故障
            /// </summary>
            public Boolean Lift_MotorBraker { get; set; }

            public ConveyorWarning(DataRow dataRow)
            {
                this.Id = Convert.ToInt32(dataRow["Id"]);
                this.DeviceName = Convert.ToString(dataRow["DeviceName"]);
                this.PosNo = Convert.ToInt32(dataRow["AlarmDataLog_PosNo"]);
                this.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"]);
                this.Manual = Convert.ToBoolean(dataRow["AlarmDataLog_Manual"]);
                this.Isolator = Convert.ToBoolean(dataRow["AlarmDataLog_Isolator"]);
                this.Breaker = Convert.ToBoolean(dataRow["AlarmDataLog_Breaker"]);
                this.Photocell = Convert.ToBoolean(dataRow["AlarmDataLog_Photocell"]);
                this.RunOvertime = Convert.ToBoolean(dataRow["AlarmDataLog_RunOvertime"]);
                this.OccupyOvertime = Convert.ToBoolean(dataRow["AlarmDataLog_OccupyOvertime"]);
                this.TaskNoGoods = Convert.ToBoolean(dataRow["AlarmDataLog_TaskNoGoods"]);
                this.X_MotorVAF = Convert.ToBoolean(dataRow["AlarmDataLog_X_MotorVAF"]);
                this.Y_MotorVAF = Convert.ToBoolean(dataRow["AlarmDataLog_Y_MotorVAF"]);
                this.X_MotorContactor = Convert.ToBoolean(dataRow["AlarmDataLog_X_MotorContactor"]);
                this.X_MotorBraker = Convert.ToBoolean(dataRow["AlarmDataLog_X_MotorBraker"]);
                this.Y_MotorContactor = Convert.ToBoolean(dataRow["AlarmDataLog_Y_MotorContactor"]);
                this.Y_MotorBraker = Convert.ToBoolean(dataRow["AlarmDataLog_Y_MotorBraker"]);
                this.Lift_MotorContactor = Convert.ToBoolean(dataRow["AlarmDataLog_Lift_MotorContactor"]);
                this.Lift_MotorBraker = Convert.ToBoolean(dataRow["AlarmDataLog_Lift_MotorBraker"]);
            }
        }

        class ConveyorWarningExecutionTime
        {
            public ConveyorWarningExecutionTime(String name)
            {
                this.Name = name;
            }
            public ConveyorWarning FromState { get; set; }
            public ConveyorWarning ToState { get; set; }
            public Int32 PosNo
            {
                get
                {
                    return this.FromState.PosNo;
                }
            }
            public string DeviceName
            {
                get
                {
                    return this.FromState.DeviceName;
                }
            }

            public string Name { get; set; }
            /// <summary>
            /// 原始状态的持续秒数
            /// </summary>
            public double TotalSeconds
            {
                get
                {
                    return ToState.CreatedAt.Subtract(FromState.CreatedAt).TotalSeconds;
                }
            }


            public DateTime From
            {
                get
                {
                    return this.FromState.CreatedAt;
                }
            }

            public DateTime To
            {
                get
                {
                    return this.ToState.CreatedAt;
                }
            }
        }
        List<ConveyorWarning> findWarnings(string deviceName,Int32? posNo,DateTime? from, DateTime? to)
        {
            string cmdText = @"
    SELECT [Id]
      ,[DeviceName]
      ,[CreatedAt]
      ,[AlarmDataLog_PosNo]
      ,[AlarmDataLog_Manual]
      ,[AlarmDataLog_Isolator]
      ,[AlarmDataLog_Breaker]
      ,[AlarmDataLog_Photocell]
      ,[AlarmDataLog_RunOvertime]
      ,[AlarmDataLog_OccupyOvertime]
      ,[AlarmDataLog_TaskNoGoods]
      ,[AlarmDataLog_MotorUseStatus]
      ,[AlarmDataLog_X_MotorVAF]
      ,[AlarmDataLog_Y_MotorVAF]
      ,[AlarmDataLog_X_MotorContactor]
      ,[AlarmDataLog_X_MotorBraker]
      ,[AlarmDataLog_Y_MotorContactor]
      ,[AlarmDataLog_Y_MotorBraker]
      ,[AlarmDataLog_Lift_MotorContactor]
      ,[AlarmDataLog_Lift_MotorBraker]
      ,[AlarmDataLog_Spare]
  FROM [ReceivedDataLogs]
  where discriminator='AlarmDataLog' 
  and createdAt between @fromDate and @toDate
  and ([DeviceName] = @deviceName or @deviceName is null)
  and ([AlarmDataLog_PosNo] = @posNo or @posNo is null)
  order by createdAt asc";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("posNo", posNo.HasValue?(object)posNo.Value:DBNull.Value);
                        cmd.Parameters.AddWithValue("deviceName", string.IsNullOrWhiteSpace(deviceName) ? DBNull.Value : (object)deviceName);
                        cmd.Parameters.AddWithValue("fromDate", from ?? DateTime.Parse("1900-01-01"));
                        cmd.Parameters.AddWithValue("toDate", to ?? DateTime.MaxValue);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    trans.Commit();
                }
            }

            return dataTable.Rows.Cast<DataRow>()
                .Select(x => new ConveyorWarning(x))
                .ToList();
        }

        List<ConveyorWarningExecutionTime> findExecutionTimes(List<ConveyorWarning> warnings,String warningName, Func<ConveyorWarning, Boolean> matchKey)
        {
            List<ConveyorWarningExecutionTime> result = new List<ConveyorWarningExecutionTime>();
            warnings = warnings.OrderBy(x => x.CreatedAt).ToList();
            var zz = warnings.Where(x => matchKey(x) != false).ToList();
            foreach (var item in zz)
            {
                var index=warnings.IndexOf(item);
                var endItem = warnings
                    .Skip(index)
                    .Where(x => x.DeviceName == item.DeviceName)
                    .Where(x=>x.PosNo ==item.PosNo)
                    .Where(x => matchKey(item) != matchKey(x))
                    .Where(x => item.CreatedAt < x.CreatedAt)
                   .FirstOrDefault();

                if (endItem == null)
                {
                    continue;
                }
                else
                {
                    ConveyorWarningExecutionTime executionTime = new ConveyorWarningExecutionTime(warningName)
                    {
                        FromState = item,
                        ToState = endItem
                    };
                    result.Add(executionTime);
                }
            }

            var q = result.GroupBy(x => new
            {
                x.Name,
                x.PosNo,
                x.From,
                x.To,
                x.DeviceName
            });
            result = q.Select(x => x.First()).ToList();

            return result
                .OrderBy(x => x.FromState.CreatedAt)
                .ToList();
        }

        List<string> loadAllDeviceNames()
        {
            String cmdText = @"SELECT [DeviceName] FROM [ReceivedDataLogs] where discriminator='AlarmDataLog'  GROUP BY [DeviceName] order by [DeviceName]";
            DataTable dtb = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtb);
                    }
                }
            }

            var result = dtb.Rows.Cast<DataRow>()
                .Select(x => Convert.ToString(x["DeviceName"]))
                .ToList();

            return result;
        }

        private void dgvCount_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                var tm = TimeSpan.FromSeconds(Convert.ToInt32(e.Value));
                e.Value = timeSpanToStr(tm);

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

        private void btnCount_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((stat) =>
            {
                try
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        progressBar1.Visible = true;
                        groupBox1.Enabled =
                            tabControl1.Enabled = 
                            dgvGridCount.Enabled = 
                            dgvCraneDetails.Enabled = 
                            dataGridView1.Enabled = 
                        splitContainer1.Enabled = false;
                        timer1.Start();
                    }));
                    xx();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        timer1.Stop();
                        progressBar1.Visible = false;
                        groupBox1.Enabled =
                            tabControl1.Enabled =
                            dgvGridCount.Enabled =
                            dgvCraneDetails.Enabled =
                            dataGridView1.Enabled = 
                        splitContainer1.Enabled = true;
                    }));
                }
            });
        }

        private void dgvCraneDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                var tm = TimeSpan.FromSeconds(Convert.ToInt32(e.Value));
                e.Value = timeSpanToStr(tm);

            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var tm = TimeSpan.FromSeconds(Convert.ToInt32(e.Value));
                e.Value = timeSpanToStr(tm);

            }
        }

        private void label3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"CREATE NONCLUSTERED INDEX [IX_discriminator_CreatedAt]
    ON [dbo].[ReceivedDataLogs] ([discriminator],[CreatedAt])
    INCLUDE ([Id],[DeviceName],[AlarmDataLog_PosNo],[AlarmDataLog_Manual],[AlarmDataLog_Isolator],[AlarmDataLog_Breaker],[AlarmDataLog_Photocell],[AlarmDataLog_RunOvertime],[AlarmDataLog_OccupyOvertime],[AlarmDataLog_TaskNoGoods],[AlarmDataLog_MotorUseStatus],[AlarmDataLog_X_MotorVAF],[AlarmDataLog_Y_MotorVAF],[AlarmDataLog_X_MotorContactor],[AlarmDataLog_X_MotorBraker],[AlarmDataLog_Y_MotorContactor],[AlarmDataLog_Y_MotorBraker],[AlarmDataLog_Lift_MotorContactor],[AlarmDataLog_Lift_MotorBraker],[AlarmDataLog_Spare])
    GO
    ", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmConveyorWarningReport_Load(object sender, EventArgs e)
        {
            try
            {
                var deviceNames = loadAllDeviceNames();
                deviceNames.Insert(0, "");
                cbxDeviceName.DataSource = deviceNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("设备名称集合加载失败。\n{0}", ex), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGridCount_Resize(object sender, EventArgs e)
        {
            progressBar1.Left = dgvGridCount.Left +dgvGridCount.Width/2- progressBar1.Width / 2;
            progressBar1.Top = dgvGridCount.Top + dgvGridCount.Height/2-progressBar1.Height / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
            }
            else
            {
                progressBar1.Value = 0;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dataGridView1.Rows.Count == 0;
        }

        private void 导出为excel文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, DataGridView> datagrids = new Dictionary<string, DataGridView>();
                datagrids.Add("统计", dgvGridCount);
                datagrids.Add("明细", dataGridView1);

                string fileName;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    string name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 输送线报警统计", dtpStartTime.Value, dtpEndTime.Value);

                    saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.CheckFileExists = false;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.FileName = name;
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

                datagrids.ExportAsExcel(fileName);

                if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    
    }
}
