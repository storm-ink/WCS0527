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
    public partial class frmTaskReport : Form
    {
        public frmTaskReport()
        {
            InitializeComponent();

            dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var nextDate = DateTime.Now.AddDays(1);
            dtpEndTime.Value = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day);
        }
        
        private void btnCount_Click(object sender, EventArgs e)
        {
            try
            {
                Int32? direction=null;
                if (cbxDirection.SelectedIndex > 0)
                {
                    direction = cbxDirection.SelectedIndex-1;
                }
                var tasks = findTasks(dtpStartTime.Value, dtpEndTime.Value, direction, tbxStartLocation.Text.Trim(), tbxEndLocation.Text.Trim());
                tasks = tasks.OrderBy(x => x.Id).ToList();

                dgvGrid.AutoGenerateColumns = false;
                dgvGrid.DataSource = tasks;

                int recordCount = tasks.Count;
                lblRecordCount.Text = recordCount.ToString();
                int minSeconds = recordCount>0?tasks.Min(x => x.TotalSeconds):0;
                int maxSencods = recordCount>0?tasks.Max(x => x.TotalSeconds):0;
                int agvSeconds = recordCount>0?tasks.Sum(x => x.TotalSeconds) / tasks.Count:0;
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

        List<TaskInfo> findTasks(DateTime startDate, DateTime endDate, Int32? direction, String startLocation,String endLocation)
        {
            string cmdText=@"                    
SELECT [Id]
      ,[TaskCode]
      ,[ReqeustId]
      ,[Source]
      ,[StartLocation_DeviceCode]
      ,[StartLocation_UserCode]
      ,[StartLocation_DeviceName]
      ,[EndLocation_DeviceCode]
      ,[EndLocation_UserCode]
      ,[EndLocation_DeviceName]
      ,[CurrentLocation_DeviceCode]
      ,[CurrentLocation_UserCode]
      ,[CurrentLocation_DeviceName]
      ,[Direction]
      ,[Status]
      ,[BizType]
      ,[CreatedAt]
      ,[FinishedAt]
      ,[Description]
      ,[Priority]
      ,DATEDIFF(S,CreatedAt,FinishedAt) as TotalSeconds
      ,Value as ContainerCode
FROM [Tasks]
left join [TaskContainerCodes] on [Tasks].Id=[TaskContainerCodes].TaskId
where FinishedAt is not null
and CreatedAt between @fromDate and @endDate
and (Direction=@direction or @direction is null)
and (StartLocation_UserCode like '%'+@startLocation+'%' or @startLocation is null)
and (EndLocation_UserCode like '%'+@endLocation+'%' or @endLocation is null)
order by CreatedAt asc";

            DataTable dtb = new DataTable();
            using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsBakConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("fromDate", startDate);
                    cmd.Parameters.AddWithValue("endDate", endDate);
                    cmd.Parameters.AddWithValue("direction", direction==null ? (object)DBNull.Value : (object)direction);
                    cmd.Parameters.AddWithValue("startLocation", String.IsNullOrWhiteSpace(startLocation) ? (object)DBNull.Value : (object)startLocation);
                    cmd.Parameters.AddWithValue("endLocation", String.IsNullOrWhiteSpace(endLocation) ? (object)DBNull.Value : (object)endLocation);

                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dtb);
                    }
                }
            }

            var result = dtb.Rows.Cast<DataRow>().Select(x => new TaskInfo
            {
                Id = Convert.ToInt32(x["Id"]),
                TaskCode = Convert.ToString(x["TaskCode"]),
                CreatedAt = Convert.ToDateTime(x["CreatedAt"]),
                Description = Convert.ToString(x["Description"]),
                Direction = Convert.ToInt32(x["Direction"]),
                EndLocationDeviceName = Convert.ToString(x["EndLocation_DeviceName"]),
                EndLocationUserCode = Convert.ToString(x["EndLocation_UserCode"]),
                FinishedAt = Convert.ToDateTime(x["FinishedAt"]),
                Source = Convert.ToInt32(x["Source"]),
                StartLocationDeviceName = Convert.ToString(x["StartLocation_DeviceName"]),
                StartLocationUserCode = Convert.ToString(x["StartLocation_UserCode"]),
                Status = Convert.ToInt32(x["Status"]),
                TotalSeconds = Convert.ToInt32(x["TotalSeconds"]),
                ContainerCodes = Convert.ToString(x["ContainerCode"])
            })
            .ToList();

            var taskGrouping = result.GroupBy(x => x.TaskCode);
            if (taskGrouping.Any(x => x.Count() > 1))
            {
                result = new List<TaskInfo>();
                foreach (var item in taskGrouping)
                {
                    var x = item.First();
                    x.ContainerCodes = string.Join(",", item.Select(o => o.ContainerCodes).ToArray());
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
            public String TaskCode{get;set;}
            public String ContainerCodes { get; set; }
            public Int32 Source{get;set;}
            public String StartLocationUserCode{get;set;}
            public String StartLocationDeviceName{get;set;}
            public String EndLocationUserCode{get;set;}
            public String EndLocationDeviceName{get;set;}
            public Int32 Direction{get;set;}
            public Int32 Status{get;set;}
            public DateTime CreatedAt{get;set;}
            public DateTime FinishedAt{get;set;}
            public String Description{get;set;}
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = dgvGrid.Rows.Count == 0;
        }

        private void 导出为excel文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string name = string.Format("{0:yyyy年MM月dd日 HH时mm分ss秒} 至 {1:yyyy年MM月dd日 HH时mm分ss秒} 主任务执行时间统计", dtpStartTime.Value, dtpEndTime.Value);

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
    }
}
