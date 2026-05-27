using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Wcs.App.Plugins.LogsViewer
{
    public partial class frmArchive : Form
    {
        LogArchiver _logArchiver = new LogArchiver();
        Thread _thread;
        public frmArchive()
        {
            InitializeComponent();

            _logArchiver.Processed += _logArchiver_Processed;

            try
            {
                var dateRange = getLogDateRange();
                if (dateRange == null)
                {
                    MessageBox.Show("没有需要归档的日志", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    dtpStartDate.Value = dateRange.startDate;
                    dtpStartDate.MinDate = dateRange.startDate;
                    dtpStartDate.MaxDate = dateRange.endDate;

                    dtpEndDate.MinDate = dateRange.startDate.AddDays(1);
                    dtpEndDate.MaxDate = dateRange.endDate;
                    dtpEndDate.Value = dtpEndDate.MinDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void _logArchiver_Processed(int recordTotals, int processedTotals, int batchSize, TimeSpan previousUsedTime)
        {
            Action<int, int, int, TimeSpan> act = (p1, p2, p3, p4) =>
            {
                _logArchiver_Processed(p1, p2, p3, p4);
            };

            if (this.InvokeRequired)
            {
                this.Invoke(act, recordTotals, processedTotals, batchSize, previousUsedTime);
            }
            else
            {
                progressBar1.Maximum = recordTotals;
                if (processedTotals > recordTotals)
                {
                    progressBar1.Value = recordTotals;
                }
                else
                {
                    progressBar1.Value = processedTotals;
                }
                var finishedAt = DateTime.Now.AddMilliseconds(((recordTotals - processedTotals) / batchSize) * previousUsedTime.TotalMilliseconds);
                lblInfo.Text = string.Format("共 {0} 条日志记录，已归档 {1}。预计可以在 {2} 完成归档任务",
                    recordTotals, processedTotals, finishedAt);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _logArchiver.SourceConnectionString = WcsConfigurationConnectionStrings.WcsLogsConnectionString;

            string sspi = "";
            if (cbxEnlist.Checked)
            {
                sspi = ";integrated security=sspi";
            }

            if (tbxPort.Text != "1433")
            {
                _logArchiver.DestinationConnectionString = string.Format("Data Source={0}:{5};Initial Catalog={1};User Id={2};Password={3};Enlist=false{6}", tbxServerName.Text, tbxDatabase.Text, tbxUserName.Text, tbxPassword.Text,sspi, tbxPort.Text);
            }
            else
            {
                _logArchiver.DestinationConnectionString = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Enlist=false{4}", tbxServerName.Text, tbxDatabase.Text, tbxUserName.Text, tbxPassword.Text, sspi);
            }
            _logArchiver.BatchSize = Convert.ToInt32(nudBatchSize.Value);
            _logArchiver.FromDate = dtpStartDate.Value.Date;
            _logArchiver.ToDate = dtpEndDate.Value.Date;

            _thread = new Thread(proc);
            _thread.Start();
        }


        void proc(object stat)
        {
            try
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                }));

                _logArchiver.Archive();
                MessageBox.Show("归档任务已结束。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Invoke(new MethodInvoker(() =>
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = true;
                }));

            }
            catch (Exception ex)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = true;
                }));
                MessageBox.Show(ex.ToString(),Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        dynamic getLogDateRange()
        {
            using (SqlConnection connection = new SqlConnection(WcsConfigurationConnectionStrings.WcsLogsConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("select min(longdate) as startDate,max(longdate) as endDate from [logs]", connection))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dtb = new DataTable();
                        adp.Fill(dtb);
                        if (dtb.Rows.Count == 0)
                        {
                            return new
                            {
                                startDate = (DateTime?)null,
                                endDate = (DateTime?)null
                            };
                        }

                        DateTime? startDate, endDate;
                        if (Convert.IsDBNull(dtb.Rows[0]["startDate"]))
                        {
                            startDate = null;
                        }
                        else
                        {
                            startDate = Convert.ToDateTime(dtb.Rows[0]["startDate"]);
                        }

                        if (Convert.IsDBNull(dtb.Rows[0]["endDate"]))
                        {
                            endDate = null;
                        }
                        else
                        {
                            endDate = Convert.ToDateTime(dtb.Rows[0]["endDate"]);

                            if (endDate.Value.Date == startDate.Value.Date)
                            {
                                endDate = endDate.Value.AddDays(1);
                            }
                        }

                        return new
                        {
                            startDate = startDate,
                            endDate = endDate
                        };
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _thread.Abort();
            _thread = null;

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void cbxEnlist_CheckedChanged(object sender, EventArgs e)
        {
            tbxUserName.Enabled =
                tbxPassword.Enabled = !cbxEnlist.Checked;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label7_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(@"exec sp_configure 'show advanced options', 1;
RECONFIGURE;
exec sp_configure 'Ad Hoc Distributed Queries', 1;
RECONFIGURE;
GO");
            }
            catch (Exception ex)
            {
                
            }
        }
    }

    public class LogArchiver
    {
        public delegate void ProcessedEventDelegate(Int32 recordTotals, Int32 processedTotals, Int32 batchSize, TimeSpan previousUsedTime);

        public String SourceConnectionString { get; set; }
        public String DestinationConnectionString { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int32 BatchSize { get; set; }
        public Int32 RecordCount { get; private set; }
        public Int32 ProcessedCount { get; private set; }
        public event ProcessedEventDelegate Processed;

        String getOrCreateArchiveTable(SqlConnection destinationConnection)
        {
            String tableName = String.Format("{0:yyyy_MM_dd}至{1:yyyy_MM_dd}", FromDate, ToDate);
            String existsTableCommandText = "SELECT Count(0) FROM dbo.SysObjects WHERE ID = object_id(N'[" + tableName + "]') AND OBJECTPROPERTY(ID, 'IsTable') = 1";
            using (SqlCommand cmd = new SqlCommand(existsTableCommandText, destinationConnection))
            {
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    String createTableCommandText = "select * into [" + tableName + "] from OPENDATASOURCE('SQLNCLI','" + SourceConnectionString + "').wcs_logs.dbo.[logs] where 1<>1";
                    cmd.CommandText = createTableCommandText;

                    cmd.ExecuteNonQuery();
                }
            }
            return tableName;
        }

        public void Archive()
        {
            using (SqlConnection sourceConnection = new SqlConnection(SourceConnectionString))
            {
                sourceConnection.Open();
                using (SqlCommand cmd = new SqlCommand("select count(0) from [logs] where [longdate]>=@startDate and [longdate]<@endDate", sourceConnection))
                {
                    cmd.Parameters.Add(new SqlParameter("startDate", this.FromDate));
                    cmd.Parameters.Add(new SqlParameter("endDate", this.ToDate));
                    Int32 recordCount = Convert.ToInt32(cmd.ExecuteScalar());
                    this.RecordCount = recordCount;
                }

                using (SqlConnection destinationConnection = new SqlConnection(DestinationConnectionString))
                {
                    destinationConnection.Open();
                    var tableName = getOrCreateArchiveTable(destinationConnection);
                    while (this.ProcessedCount < this.RecordCount)
                    {
                        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        using (System.Transactions.TransactionScope tranScope = new System.Transactions.TransactionScope())
                        {
                            String copyCommandText = String.Format(@"
    INSERT INTO [{0}] SELECT top {1} * FROM OPENDATASOURCE('SQLNCLI','{2}').wcs_logs.dbo.[logs] 
    where [longdate]>=@startDate and [longdate]<@endDate;
    ", tableName, BatchSize, SourceConnectionString);
                            using (SqlTransaction trans = destinationConnection.BeginTransaction())
                            {
                                using (SqlCommand copyeCmd = new SqlCommand(copyCommandText, destinationConnection, trans))
                                {
                                    copyeCmd.Parameters.Add(new SqlParameter("startDate", this.FromDate));
                                    copyeCmd.Parameters.Add(new SqlParameter("endDate", this.ToDate));
                                    Int32 result = copyeCmd.ExecuteNonQuery();
                                }

                                trans.Commit();
                            }

                            Int32 deleteCount = 0;
                            String archiveCommandText = String.Format(@"
    DELETE top ({1}) FROM [logs] 
    where [longdate]>=@startDate and [longdate]<@endDate;
    ", tableName, BatchSize, SourceConnectionString);
                            using (SqlTransaction trans = sourceConnection.BeginTransaction())
                            {
                                using (SqlCommand archiveCmd = new SqlCommand(archiveCommandText, sourceConnection, trans))
                                {
                                    archiveCmd.Parameters.Add(new SqlParameter("startDate", this.FromDate));
                                    archiveCmd.Parameters.Add(new SqlParameter("endDate", this.ToDate));
                                    Int32 result = archiveCmd.ExecuteNonQuery();
                                    deleteCount = result;
                                }

                                trans.Commit();
                            }

                            tranScope.Complete();

                            ProcessedCount += deleteCount;
                        }
                        sw.Stop();
                        if (Processed != null)
                        {
                            Processed(this.RecordCount, this.ProcessedCount, this.BatchSize, new TimeSpan(sw.ElapsedTicks));
                        }

                    }
                }
            }
        }
    }
}
