using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.Reports
{
    public partial class frmEquipmentActionsReport : Form
    {
        public frmEquipmentActionsReport()
        {
            InitializeComponent();

            dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var nextDate = DateTime.Now.AddDays(1);
            dtpEndTime.Value = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day);            
        }



        private void frmEquipmentActionsReport_Load(object sender, EventArgs e)
        {
            try
            {
                var deviceNames = loadAllDeviceNames();
                deviceNames.Insert(0,"");
                cbxDeviceName.DataSource = deviceNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("设备名称集合加载失败。\n{0}", ex), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            try
            {
                var tasks = findTasks(dtpStartTime.Value, dtpEndTime.Value, cbxDeviceName.Text.Trim());
                tasks = tasks.OrderBy(x => x.Id).ToList();

                dgvGrid.AutoGenerateColumns = false;
                dgvGrid.DataSource = tasks;

                int recordCount = tasks.Count;
                lblRecordCount.Text = recordCount.ToString();
                int minSeconds = recordCount > 0 ? tasks.Min(x => x.TotalSeconds) : 0;
                int maxSencods = recordCount > 0 ? tasks.Max(x => x.TotalSeconds) : 0;
                int agvSeconds = recordCount > 0 ? tasks.Sum(x => x.TotalSeconds) / tasks.Count : 0;
                lblMinSeconds.Text = timeSpanToStr(TimeSpan.FromSeconds(minSeconds));
                lblMaxSeconds.Text = timeSpanToStr(TimeSpan.FromSeconds(maxSencods));
                lblAgvSeconds.Text = timeSpanToStr(TimeSpan.FromSeconds(agvSeconds));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colTotalSeconds.Index)
            {
                var seconds = Convert.ToInt32(e.Value);
                e.Value = timeSpanToStr(TimeSpan.FromSeconds(seconds));
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

        List<string> loadAllDeviceNames()
        {
            String cmdText = @"SELECT [DeviceName] FROM [EquipmentActions] GROUP BY [DeviceName] order by [DeviceName]";
            DataTable dtb = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsBakConnectionString))
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

        List<TaskInfo> findTasks(DateTime startDate, DateTime endDate, String deviceName)
        {
            String cmdText = @"SELECT a.[Id]
      ,a.[discriminator]
      ,a.[EquipmentTaskId]
      ,a.[DeviceName]
      ,a.[Status]
      ,a.[CreatedAt]
      ,a.[FinishedAt]
      ,a.[SentAt]
      ,b.[RouteId]
      ,b.StartLocation_DeviceCode
      ,b.StartLocation_DeviceName
      ,b.StartLocation_UserCode
      ,b.EndLocation_DeviceCode
      ,b.EndLocation_DeviceName
      ,b.EndLocation_UserCode
      ,DATEDIFF(S,a.SentAt,a.FinishedAt) as TotalSeconds
  FROM [EquipmentActions] a
  left join [LogicMovements] b on a.[LogicMovementId]=b.[Id]
  where a.SentAt is not null and a.FinishedAt is not null
  and a.[CreatedAt] between @fromDate and @toDate
  and (a.[DeviceName]=@deviceName or @deviceName is null)
  order by a.Id asc";

            DataTable dtb = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsBakConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("fromDate", startDate);
                    cmd.Parameters.AddWithValue("toDate", endDate);
                    cmd.Parameters.AddWithValue("deviceName", String.IsNullOrWhiteSpace(deviceName) ? (object)DBNull.Value : (object)deviceName);

                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtb);
                    }
                }
            }

            var result = dtb.Rows.Cast<DataRow>().Select(x => new TaskInfo
            {
                Id = Convert.ToInt32(x["Id"]),
                discriminator = Convert.ToString(x["discriminator"]),
                EquipmentTaskId = Convert.ToInt32(x["EquipmentTaskId"]),
                DeviceName = Convert.ToString(x["DeviceName"]),
                Status = Convert.ToInt32(x["Status"]),
                CreatedAt = Convert.ToDateTime(x["CreatedAt"]),
                FinishedAt = Convert.ToDateTime(x["FinishedAt"]),
                SentAt = Convert.ToDateTime(x["SentAt"]),
                RouteId = Convert.ToInt32(x["RouteId"]),
                StartLocation_DeviceCode = Convert.ToString(x["StartLocation_DeviceCode"]),
                StartLocation_DeviceName = Convert.ToString(x["StartLocation_DeviceName"]),
                StartLocation_UserCode = Convert.ToString(x["StartLocation_UserCode"]),
                EndLocation_DeviceCode = Convert.ToString(x["EndLocation_DeviceCode"]),
                EndLocation_DeviceName = Convert.ToString(x["EndLocation_DeviceName"]),
                EndLocation_UserCode = Convert.ToString(x["EndLocation_UserCode"]),
                TotalSeconds = Convert.ToInt32(x["TotalSeconds"])
            })
            .ToList();

            var taskGrouping = result.GroupBy(x => x.Id);
            if (taskGrouping.Any(x => x.Count() > 1))
            {
                result = new List<TaskInfo>();
                foreach (var item in taskGrouping)
                {
                    var x = item.First();
                    result.Add(x);
                }

                return result;
            }
            else
            {
                return result;
            }
        }

        class TaskInfo
        {
            public Int32 Id{get;set;}
            public String discriminator{get;set;}
            public Int32 EquipmentTaskId{get;set;}
            public String DeviceName{get;set;}
            public Int32 Status{get;set;}
            public DateTime CreatedAt{get;set;}
            public DateTime FinishedAt{get;set;}
            public DateTime SentAt{get;set;}
            public Int32 RouteId{get;set;}
            public String StartLocation_DeviceCode{get;set;}
            public String StartLocation_DeviceName{get;set;}
            public String StartLocation_UserCode{get;set;}
            public String EndLocation_DeviceCode{get;set;}
            public String EndLocation_DeviceName{get;set;}
            public String EndLocation_UserCode{get;set;}
            public Int32 TotalSeconds { get; set; }
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

        private void 导出为excel文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string name;
                if (!string.IsNullOrWhiteSpace(cbxDeviceName.Text))
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} {2} 的设备任务执行时间统计", dtpStartTime.Value, dtpEndTime.Value, cbxDeviceName.Text);
                }
                else
                {
                    name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 所有设备任务的执行时间统计", dtpStartTime.Value, dtpEndTime.Value);
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

            dgvGrid.ExportAsExcel(fileName);

            if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Process.Start(fileName);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dgvGrid.Rows.Count == 0;
        }
    }
}
