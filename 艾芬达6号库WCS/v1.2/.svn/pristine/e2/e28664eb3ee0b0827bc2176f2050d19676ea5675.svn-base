using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHibernate.Linq;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace Wcs.App.Plugins.LogsViewer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((stat) =>
            {
                getTodayErrorsCount();
            });
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
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
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            SqlCommand cmd = new SqlCommand();
            sb.AppendLine(@"SELECT top " + nudTakeCount.Value + @" *
                                  FROM [Logs] where 1=1");
            int idFrom;
            if (int.TryParse(tbxIdFrom.Text, out idFrom) && idFrom > 0)
            {
                sb.AppendLine(" and id>=" + idFrom);

                int idTakeCount;
                if (int.TryParse(tbxIdTakeCount.Text, out idTakeCount) && idTakeCount > 0)
                {
                    sb.AppendLine(" and id<=" + (idFrom + idTakeCount));
                }
            }

            int secondTakeCount;
            DateTime dateTime = dtpDateFrom.Value;
            sb.AppendLine(" and longdate>='" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            if (int.TryParse(tbxDateTakeCount.Text, out secondTakeCount) && secondTakeCount > 0)
            {
                sb.AppendLine(" and longdate<='" + dateTime.AddSeconds(secondTakeCount).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            }

            if (!string.IsNullOrWhiteSpace(tbxLogger.Text))
            {
                sb.AppendLine(" and logger like @logger");
                cmd.Parameters.Add(new SqlParameter("logger", "%"+tbxLogger.Text.Trim()+"%"));
            }

            if (!string.IsNullOrWhiteSpace(tbxSender.Text))
            {
                sb.AppendLine(" and sender like @sender");
                cmd.Parameters.Add(new SqlParameter("sender", tbxSender.Text.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(tbxMessage.Text))
            {
                sb.AppendLine(" and ([message] = @message or [message] like '%'+@message+'%')");
                cmd.Parameters.Add(new SqlParameter("message", tbxMessage.Text.Trim()));
            }

            int threadId;
            if (int.TryParse(tbxThreadId.Text, out threadId) && threadId > 0)
            {
                sb.AppendLine(" and ThreadId=" + threadId);
            }

            if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
            {
                sb.AppendLine(" and [TaskCode] = @taskCode");
                cmd.Parameters.Add(new SqlParameter("taskCode", tbxTaskCode.Text.Trim()));
            }

            int equipmentTaskId;
            if (int.TryParse(tbxEquipmentTaskId.Text, out equipmentTaskId) && equipmentTaskId > 0)
            {
                sb.AppendLine(" and EquipmentTaskId=" + equipmentTaskId);
            }

            if (tbxArea.Visible && !string.IsNullOrWhiteSpace(tbxArea.Text))
            {
                sb.AppendLine(" and [Area] = @area");
                cmd.Parameters.Add(new SqlParameter("area", tbxArea.Text.Trim()));
            }

            var levels = groupBox1.Controls.Cast<Control>()
                .Where(x => x is CheckBox)
                .Select(x => x as CheckBox)
                .Where(x => x.Checked)
                .Select(x => " [level] = '" + x.Text + "'")
                .ToArray();

            if (levels.Length == 0)
            {
                MessageBox.Show("请至少选择一个日志等级", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var levelsConditions = string.Join(" or ", levels);
            sb.AppendLine(" and (" + levelsConditions + ")");

            cmd.CommandText = sb.ToString();

            ThreadPool.QueueUserWorkItem((stat) =>
            {
                xx(cmd);
            });
        }

        void xx(SqlCommand cmd)
        {
            try
            {

                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = false;
                }));

                DataTable dtb = new DataTable();
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsLogsConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtb);
                        }

                        trans.Commit();
                    }
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    if (dtb.Columns.Contains("Area"))
                    {
                        colArea.Visible =
                        lblArea.Visible =
                        tbxArea.Visible = true;
                    }
                    else
                    {
                        colArea.Visible =
                        lblArea.Visible =
                        tbxArea.Visible = false;
                    }
                    BindingSource bindingSource = new BindingSource(dtb, "");
                    dgvGrid.DataSource = bindingSource;
                    bindingNavigator1.BindingSource = bindingSource;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志查看器", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = true;
                }));
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.CurrentRow == null || dgvGrid.SelectedRows.Count!=1)
            {
                e.Cancel = true;
            }
        }

        private void tbxLogException_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvGrid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataRow dtr;
                if(dgvGrid.CurrentRow==null)
                {
                    dtr = null;
                }else
                {
                    dtr = (dgvGrid.CurrentRow.DataBoundItem as DataRowView).Row;
                }
                updateReceivedDataForm(dtr);
                bindDetails(dtr);

            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, "日志查看器", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void bindDetails(DataRow row)
        {
            if (row == null)
            {
                tbxLogDetails.Clear();
            }
            else
            {
                tbxLogDetails.Clear();
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "Id  ：", row["Id"]));
                if (row.Table.Columns.Contains("Area"))
                {
                    tbxLogDetails.AppendText(string.Format("{0}{1}\n", "区域：", row["Area"]));
                }
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "时间：", Convert.ToDateTime(row["LongDate"]).ToString("yyyy-MM-dd HH:mm:ss.ffff")));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "级别：", row["Level"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "线程：", row["ThreadId"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "用户：", row["UserName"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "设备：", row["Sender"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "日志：", row["Logger"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "进程：", row["ProcessName"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "主任务号：", row["TaskCode"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n", "设备任务：", row["EquipmentTaskId"]));
                tbxLogDetails.AppendText(string.Format("{0}{1}\n\n", "消息：", row["Message"]));
                if (!string.IsNullOrWhiteSpace(Convert.ToString(row["Exception"])))
                {
                    tbxLogDetails.AppendText(string.Format("{0}:{1}\n", "异常：", row["Exception"]));
                }

                tbxLogDetails.Select(0, 0);
                tbxLogDetails.ScrollToCaret();
            }
        }

        frmReceivedDatas _frmReceivedData;
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(_frmReceivedData==null){
                _frmReceivedData = new frmReceivedDatas();
    
                _frmReceivedData.FormClosed += _frmReceivedData_FormClosed;
            }

            if (dgvGrid.CurrentRow != null)
            {
                updateReceivedDataForm((dgvGrid.CurrentRow.DataBoundItem as DataRowView).Row);
            }
        }

        void updateReceivedDataForm(DataRow row)
        {
            if(row==null)
            {
                return ;
            }

            DateTime longDate=Convert.ToDateTime(row["LongDate"]);
            String deviceName=Convert.ToString(row["Sender"]);
            if (_frmReceivedData == null || _frmReceivedData.IsDisposed || _frmReceivedData.Disposing)
            {
                return;
            }

            if (!_frmReceivedData.Visible)
            {


                _frmReceivedData.Left = this.Left;
                _frmReceivedData.Top = this.Top + groupBox1.Height;

                _frmReceivedData.Show(this);
            }

            _frmReceivedData.UpdateData(longDate, deviceName);
        }

        void _frmReceivedData_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frmReceivedData = null;
        }

        void getTodayErrorsCount()
        {
            String errors = "-", falats = "-";
            try
            {
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsLogsConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT count(0) FROM [Logs] where [level] = 'Error' and longDate>=@today",conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("today", DateTime.Now.ToString("yyyy-MM-dd")));

                            errors = Convert.ToString(cmd.ExecuteScalar());

                            cmd.CommandText = @"SELECT count(0) FROM [Logs] where [level] = 'Falat' and longDate>=@today";

                            falats = Convert.ToString(cmd.ExecuteScalar());
                        }

                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志查看器", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {
                groupBox1.Enabled = true;
            }


            Action<string, string> setText = (err, falat) =>
            {
                lblErrors.Text = err;
                lblErrors.ForeColor = Color.Red;
                lblFalats.Text = falat;
                lblFalats.ForeColor = Color.Red;
            };

            this.Invoke(setText, errors, falats);
        }

        public void Search(DateTime startDate,Int32 secondTakeCount,String message,params String[] levels)
        {
            tbxIdFrom.Text = "";
            tbxIdTakeCount.Text = "";
            dtpDateFrom.Value = startDate;
            tbxDateTakeCount.Text = secondTakeCount.ToString();
            tbxThreadId.Text = "";
            tbxLogger.Text = "";
            tbxSender.Text = "";
            tbxMessage.Text = message;
            foreach (var item in new CheckBox[]{cbxDebug,cbxError,cbxFatal,cbxInfo,cbxTrace,cbxWarn})
            {
                if (levels.Any(x => x == Convert.ToString(item.Text)))
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }

            nudTakeCount.Value = nudTakeCount.Maximum;

            btnSearch_Click(null, null);
        }

        frmErrors _frmErrors;
        private void toolStripStatusLabel9_Click(object sender, EventArgs e)
        {
            if (_frmErrors != null && !_frmErrors.IsDisposed && !_frmErrors.Disposing && _frmErrors.Visible)
            {
                return;
            }

            _frmErrors = new frmErrors(this);
            _frmErrors.Show(this);
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
                        return ;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tslArchiveLogs_Click(object sender, EventArgs e)
        {
            using (frmArchive frm = new frmArchive())
            {
                frm.ShowDialog();
            }
        }
        
    }
}
