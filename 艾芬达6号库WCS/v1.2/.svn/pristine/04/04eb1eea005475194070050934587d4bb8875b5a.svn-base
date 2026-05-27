using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Wcs.App.Plugins.Reports
{
    public partial class frmCraneWarningReport : Form
    {
        public frmCraneWarningReport()
        {
            InitializeComponent();

            dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEndTime.Value = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
        }
        private void btnCount_Click(object sender, EventArgs e)
        {
            var rows = FindErrorExecutionTimes(cbxCrane.Text, dtpStartTime.Value, dtpEndTime.Value);
            if (cbxFilterFirstAndLastErrors.Checked)
            {
                rows = rows.Where(x => x.From.Date == x.To.Date);
            }
            if (cbxIgMinValue.Checked)
            {
                rows = rows.Where(x => x.TotalSeconds >= Convert.ToDouble(nudMinValue.Value));
            }
            //剔除掉重复的故障记录（因为是定时读取堆垛机信息，一次故障可能有多条日志记录）
            var nrows = from a in rows
                        group a by new { a.DeviceName, a.To } into grp
                        let min = grp.Min(a => a.From)
                        from row in grp
                        where row.From == min
                        select row;
            var countRows = nrows.GroupBy(x => x.Code)
                .Select(x => new
                {
                    Code = x.Key,
                    Description = x.First().Description,
                    Count = x.Count(),
                    TotalSecnods = x.Sum(r => r.TotalSeconds),
                    Percent = string.Format("{0:0.####}%", (x.Count() / Convert.ToDouble(rows.Count())) * 100),
                    TotalSecondsPercent = string.Format("{0:0.####}%", (x.Sum(r => r.TotalSeconds) / rows.Sum(r => r.TotalSeconds)) * 100)
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = nrows.ToList();

            chart1.Series.Clear();
            #region 设置样式
            //设置图表类型为 线性图
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            chart1.Legends[0].Enabled = false;
            #endregion

            var series = chart1.Series.Add("series1");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series.ChartArea = chart1.ChartAreas[0].Name;
            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";

            foreach (var item in countRows)
            {
                var name = string.IsNullOrWhiteSpace(item.Description) ? "<NULL>" : item.Description;
                var pointIndex = series.Points.AddXY(name, item.Count);
                var point = series.Points[pointIndex];
                point.Label = string.Format("{0}:#PERCENT", name, item.Count);
                point.LegendText = name;
            }

            var t = countRows.GetType().GetGenericArguments()[0];
            var sumRow = (dynamic)t.Assembly.CreateInstance(t.FullName, false,
                System.Reflection.BindingFlags.CreateInstance, null,
                new object[] { "合计", "", nrows.Count(), nrows.Sum(x => x.TotalSeconds), "100%", "100%" }, null, null);

            countRows.Add(sumRow);

            dgvGridCount.AutoGenerateColumns = false;
            dgvGridCount.DataSource = countRows;
        }

        private void dgvGridCount_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colTotalSeonds.Index)
            {
                var seconds = ((dynamic)dgvGridCount.Rows[e.RowIndex].DataBoundItem).TotalSecnods;
                var tm = TimeSpan.FromSeconds(seconds);
                e.Value = timeSpanToStr(tm);

            }
        }

        private void dgvGridCount_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if ((e.RowIndex + e.RowCount) == ((dynamic)dgvGridCount.DataSource).Count)
            {
                var row = dgvGridCount.Rows[e.RowIndex + e.RowCount - 1];
                row.DefaultCellStyle.ForeColor = Color.Green;
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor;
            }
        }


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == Column3.Index)
            {
                var seconds = ((dynamic)dataGridView1.Rows[e.RowIndex].DataBoundItem).TotalSeconds;
                var tm = TimeSpan.FromSeconds(seconds);
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

        #region Codes

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

        public IEnumerable<CraneErrorTime> FindErrorTimes(string deviceName, DateTime? from, DateTime? to)
        {
            string sql = @"select devicename,[createdat],[RequestStateCommandReplyDataLog_errorCode] as [state]
  from dbo.ReceivedDataLogs 
  where (devicename=@deviceName or @deviceName is null) and discriminator='RequestStateCommandReplyDataLog'
  and CreatedAt between @fromDate and @endDate
  and [RequestStateCommandReplyDataLog_errorCode] is not null
  and [RequestStateCommandReplyDataLog_errorCode]<>0
  order by CreatedAt asc";

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("deviceName", string.IsNullOrWhiteSpace(deviceName) ? DBNull.Value : (object)deviceName);
                        cmd.Parameters.AddWithValue("fromDate", from ?? DateTime.Parse("1900-01-01"));
                        cmd.Parameters.AddWithValue("endDate", to ?? DateTime.MaxValue);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    trans.Commit();
                }
            }

            return dataTable.Rows.Cast<DataRow>()
                .Select(x => new CraneErrorTime
                {
                    DeviceName = Convert.ToString(x["deviceName"]),
                    Code = Convert.ToString(x["state"]),
                    Description = getAlarm(Convert.ToString(x["state"])),
                    StartTime = Convert.ToDateTime(x["createdAt"])
                })
                .ToList();
        }

        public IEnumerable<CraneErrorExecutionTime> FindErrorExecutionTimes(string deviceName, DateTime? from, DateTime? to)
        {
            //发生故障时间到第一次正常运行的时间认为是堆垛机故障的时间
            string sql = @"select top 1 devicename,[createdat],[RequestStateCommandReplyDataLog_errorCode] as [state]
  from dbo.ReceivedDataLogs 
  where devicename=@deviceName and discriminator='RequestStateCommandReplyDataLog'
  and CreatedAt>@fromDate and [RequestStateCommandReplyDataLog_errorCode]=0 and [RequestStateCommandReplyDataLog_State]<>12 and [RequestStateCommandReplyDataLog_State]<>8  and [RequestStateCommandReplyDataLog_State]<>0
  order by CreatedAt asc";

            List<CraneErrorExecutionTime> result = new List<CraneErrorExecutionTime>();
            var stateTimes = FindErrorTimes(deviceName, from, to);
            stateTimes = stateTimes.OrderBy(x => x.StartTime);
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dtb = new DataTable();
                        foreach (var item in stateTimes)
                        {
                            cmd.Parameters.Clear();
                            dtb.Rows.Clear();
                            cmd.Parameters.AddWithValue("deviceName", item.DeviceName);
                            cmd.Parameters.AddWithValue("fromDate", item.StartTime.AddMilliseconds(1).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            cmd.Parameters.AddWithValue("code", item.Code);
                            adp.Fill(dtb);
                            if (dtb.Rows.Count == 0)
                            {
                                break;
                            }

                            CraneErrorTime t = new CraneErrorTime
                            {
                                DeviceName = Convert.ToString(dtb.Rows[0]["deviceName"]),
                                Code = Convert.ToString(dtb.Rows[0]["state"]),
                                Description = getAlarm(Convert.ToString(dtb.Rows[0]["state"])),
                                StartTime = Convert.ToDateTime(dtb.Rows[0]["createdAt"])
                            };


                            result.Add(new CraneErrorExecutionTime
                            {
                                FromState = item,
                                ToState = t
                            });
                        }

                    }
                }
            }
            var q = result.GroupBy(x => new
            {
                x.Code,
                x.From,
                x.DeviceName
            });
            result = q.Select(x => x.First()).ToList();

            return result.OrderBy(x => x.FromState.StartTime);
        }

        public class CraneErrorTime
        {
            public String DeviceName { get; set; }
            public DateTime StartTime { get; set; }
            public String Code { get; set; }
            public String Description { get; set; }
        }

        public class CraneErrorExecutionTime
        {
            public CraneErrorTime FromState { get; set; }
            public CraneErrorTime ToState { get; set; }
            public string DeviceName
            {
                get
                {
                    return this.FromState.DeviceName;
                }
            }
            /// <summary>
            /// 错误编码
            /// </summary>
            public string Code
            {
                get
                {
                    return this.FromState.Code;
                }
            }

            public string Description
            {
                get
                {
                    return this.FromState.Description;
                }
            }
            /// <summary>
            /// 原始状态的持续秒数
            /// </summary>
            public double TotalSeconds
            {
                get
                {
                    return ToState.StartTime.Subtract(FromState.StartTime).TotalSeconds;
                }
            }


            public DateTime From
            {
                get
                {
                    return this.FromState.StartTime;
                }
            }

            public DateTime To
            {
                get
                {
                    return this.ToState.StartTime;
                }
            }

            public override string ToString()
            {
                if (FromState.StartTime.Date == ToState.StartTime.Date)
                {
                    return string.Format("{0} 从 {1:HH:mm:ss} 执行到 {2:HH:mm:ss}，共保持了 {3} 秒", FromState.Code, FromState.StartTime, ToState.StartTime, TotalSeconds);
                }
                else
                {
                    return string.Format("{0} 从 {1} 执行到 {2}，共保持了 {3} 秒", FromState.Code, FromState.StartTime, ToState.StartTime, TotalSeconds);
                }
            }
        }

        static Dictionary<string, string> _Alarm = new Dictionary<string, string>();
        string getAlarm(string code)
        {
            if (code == "0")
            {
                return "正常";
            }

            if (_Alarm.Count == 0)
            {
                var files = Directory.GetFiles(Application.StartupPath, "alarms.xml", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    return code;
                }

                String fileName;
                if (files.Length > 1)
                {
                    fileName = files.FirstOrDefault(x => x.Contains("堆垛机"));
                }
                else
                {
                    fileName = files[0];
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                foreach (XmlNode item in doc.SelectNodes("/alarms/alarm"))
                {
                    _Alarm.Add(item.Attributes["code"].Value, item.Attributes["name"].Value);
                }
            }

            if (_Alarm.ContainsKey(code))
            {
                return _Alarm[code];
            }


            return code;
        }
        #endregion

        private void lblCreateIndex_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"CREATE NONCLUSTERED INDEX [ix_ReceivedDataLogs_ErrorCode] ON [dbo].[ReceivedDataLogs] 
    (
	    [discriminator] ASC,
	    [DeviceName] ASC,
	    [CreatedAt] ASC
    )
    INCLUDE ( [RequestStateCommandReplyDataLog_ErrorCode]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dgvGridCount.Rows.Count <= 1;
        }

        private void 导出为excel文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string name;
                if (!string.IsNullOrWhiteSpace(cbxCrane.Text))
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} {2} 的报警统计", dtpStartTime.Value, dtpEndTime.Value, cbxCrane.Text);
                }
                else
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 所有堆垛机的报警统计", dtpStartTime.Value, dtpEndTime.Value);
                }
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

            dgvGridCount.ExportAsExcel(fileName);

            if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Process.Start(fileName);
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dgvGridCount.Rows.Count <= 1;
        }

        private void 另存为图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string name;
                if (!string.IsNullOrWhiteSpace(cbxCrane.Text))
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} {2} 的报警统计", dtpStartTime.Value, dtpEndTime.Value, cbxCrane.Text);
                }
                else
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 所有堆垛机的报警统计", dtpStartTime.Value, dtpEndTime.Value);
                }
                saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.AddExtension = true;
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.DefaultExt = "jpg";
                saveFileDialog.FileName = name;
                saveFileDialog.Filter = "JPEG 文件交换格式|*.jpg";
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

            chart1.SaveImage(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            if (MessageBox.Show("保存成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Process.Start(fileName);
            }
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dataGridView1.Rows.Count == 0;
        }

        private void 导出为excel文档ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fileName;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string name;
                if (!string.IsNullOrWhiteSpace(cbxCrane.Text))
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} {2} 的报警统计", dtpStartTime.Value, dtpEndTime.Value, cbxCrane.Text);
                }
                else
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 所有堆垛机的报警统计", dtpStartTime.Value, dtpEndTime.Value);
                }
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

            dataGridView1.ExportAsExcel(fileName);

            if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Process.Start(fileName);
            }
        }

        private void cbxIgMinValue_CheckedChanged(object sender, EventArgs e)
        {
            nudMinValue.Enabled = cbxIgMinValue.Checked;
        }


    }

}
