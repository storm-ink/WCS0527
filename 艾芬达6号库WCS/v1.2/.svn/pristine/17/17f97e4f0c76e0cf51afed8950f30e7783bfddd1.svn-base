using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;

namespace Wcs.App.Plugins.LogsViewer
{
    public partial class frmErrors : Form
    {
        frmMain _mainForm;
        public frmErrors(frmMain mainForm)
        {
            InitializeComponent();

            _mainForm = mainForm;
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }

        Thread thread;
        private void count(object state)
        {
            dynamic param = (dynamic)state;
            string level = param.level;
            DateTime startDate = param.date;
            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = false;
                    dgvGrid.Enabled = false;
                    timer1.Interval = 100;
                    timer1.Start();
                }));

                DataTable dtb = new DataTable();
                using (SqlConnection conn = new SqlConnection(WcsConfigurationConnectionStrings.WcsLogsConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"SELECT [message],count(0) as Totals FROM [Logs] where [level] = @level and [message]<>'' and longDate between @startDate and @endDate group by message order by Totals desc", conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("startDate", startDate.ToString("yyyy-MM-dd")));
                            cmd.Parameters.Add(new SqlParameter("endDate", startDate.AddDays(1).ToString("yyyy-MM-dd")));
                            cmd.Parameters.Add(new SqlParameter("level", level));

                            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                            {
                                adp.Fill(dtb);
                            }
                        }

                        trans.Commit();
                    }
                }

                this.Invoke(new MethodInvoker(() =>
                {
                    dgvGrid.DataSource = dtb;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {

                thread = null;


                this.Invoke(new MethodInvoker(() =>
                {
                    groupBox1.Enabled = true;
                    dgvGrid.Enabled = true;
                    processer.Value = processer.Minimum;
                    timer1.Stop();

                    dgvGrid_SelectionChanged(null, null);
                }));
            }
        }

        private void run()
        {
            if (groupBox1.Enabled == false)
            {
                return;
            }

            if (thread != null)
            {
                //MessageBox.Show("有未完成的查询，请稍候.");
                return;
            }

            string level = "Error";
            if (rbError.Checked)
            {
                level = "Error";
            }
            else if (rbWarn.Checked)
            {
                level = "Warn";
            }
            else if (rbFault.Checked)
            {
                level = "Falat";
            }
            else
            {
                level = "Error";
            }
            thread = new Thread(new ParameterizedThreadStart(count));
            thread.Start(new
            {
                date = dateTimePicker1.Value,
                level = level
            });
        }

        private void rbError_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as RadioButton).Checked || groupBox1.Enabled==false || !this.Visible)
            {
                return;
            }
            run();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (processer.Value >= processer.Maximum)
            {
                processer.Value = processer.Minimum;
            }

            processer.Value++;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (groupBox1.Enabled == false || !this.Visible)
            {
                return;
            }

            run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
        }
        
        private void frmErrors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                //thread.Abort();
                thread = null;
            }
        }

        private void dgvGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGrid.Enabled == false)
            {
                return;
            }

            if (dgvGrid.CurrentRow == null)
            {
                return;
            }

            if (thread != null)
            {
                MessageBox.Show("有未完成的查询，请稍候.");
                return;
            }

            string level = "Error";
            if (rbError.Checked)
            {
                level = "Error";
            }
            else if (rbWarn.Checked)
            {
                level = "Warn";
            }
            else if (rbFault.Checked)
            {
                level = "Fault";
            }
            else
            {
                level = "Error";
            }

            var row = (dgvGrid.CurrentRow.DataBoundItem as DataRowView).Row;

            _mainForm.Search(dateTimePicker1.Value, 24 * 60 * 60, Convert.ToString(row["message"]), level);
        }

        private void dgvGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvGrid_SelectionChanged(null, null);
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
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
                    saveFileDialog.FileName = "错误统计";
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.Rows.Count == 0)
            {
                e.Cancel = true;
                return;
            }
        }

    }
}
