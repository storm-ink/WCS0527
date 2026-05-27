using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wcs.App.Plugins.LogsViewer.设备数据;

namespace Wcs.App.Plugins.LogsViewer
{
    public partial class frmReceivedDatas : Form
    {
        public DateTime LogStartTime { get; private set; }
        public String LogDeviceName { get; private set; }
        public frmReceivedDatas()
        {
            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
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
            if (e.ColumnIndex == colDetails.Index)
            {
                var obj = dgvGrid.Rows[e.RowIndex].DataBoundItem;

                e.Value = Convert.ToString(obj);
            }
        }

        private void dgvGrid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DeviceStatusData dtr;
                if (dgvGrid.CurrentRow == null)
                {
                    dtr = null;
                }
                else
                {
                    dtr = dgvGrid.CurrentRow.DataBoundItem as DeviceStatusData;
                }

                bindDetails(dtr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志查看器", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindDetails(DeviceStatusData data)
        {
            if (data == null)
            {
                tbxLogDetails.Clear();
            }
            else
            {
                tbxLogDetails.Clear();
                tbxLogDetails.AppendText(data.ToDetails());


                tbxLogDetails.Select(0, 0);
                tbxLogDetails.ScrollToCaret();
            }
        }

        public void UpdateData(DateTime startTime, String deviceName)
        {
            try
            {
                this.Enabled = false;
                this.LogDeviceName = deviceName;
                this.LogStartTime = startTime;

                StringBuilder sb = new StringBuilder();
                DateTime startOfTime = startTime.AddSeconds(Convert.ToInt32(startTimeOffset.Value));
                DateTime endOfTime = startTime.AddSeconds(Convert.ToInt32(endTimeOffset.Value));
                sb.AppendLine("select * from ReceivedDataLogs where createdAt between @startTime and @endTime");

                if (checkBox1.Checked)
                {
                    sb.AppendFormat(" and DeviceName = @deviceName");
                }

                DataTable dtb = new DataTable();
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.DeviceTrackingDataConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        using (SqlCommand cmd = new SqlCommand(sb.ToString(), conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("startTime", startOfTime));
                            cmd.Parameters.Add(new SqlParameter("endTime", endOfTime));

                            if (checkBox1.Checked)
                            {
                                cmd.Parameters.Add(new SqlParameter("deviceName", this.LogDeviceName));
                            }

                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.Fill(dtb);
                            }
                        }

                        trans.Commit();
                    }
                }

                List<DeviceStatusData> list = dtb.Rows.Cast<DataRow>()
                    .Select(x => DeviceDataHelper.Parse(x))
                    .ToList();
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataMember = list.GetType().FullName;
                bindingSource.DataSource = list;
                bindingNavigator1.BindingSource = bindingSource;
                dgvGrid.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志查看器", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData(this.LogStartTime, this.LogDeviceName);
        }

        private void endTimeOffset_ValueChanged(object sender, EventArgs e)
        {
            UpdateData(this.LogStartTime, this.LogDeviceName);
        }
    }
}
