using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatedataServer.App
{
    public partial class frmMain : Form
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                dgvGrid.AutoGenerateColumns = false;
                dgvGrid.DataSource = MatedataServicesManager.Hosts;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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

        private void dgvGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var host = dgvGrid.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == colName.Index)
            {
                e.Value = GetProperty<String>(host,"Name");
            }
            else if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = GetProperty<Boolean>(host, "IsRunning") ? "运行" : "停止";
            }
            else if (e.ColumnIndex == colAddress.Index)
            {
                if (GetProperty<String[]>(host,"BaseAddresses") != null)
                {
                    e.Value = String.Join(",", GetProperty<String[]>(host, "BaseAddresses"));
                }
                else
                {
                    e.Value = "";
                }
            }

            if (GetProperty<Boolean>(host, "IsRunning"))
            {
                dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
            }
            else
            {
                dgvGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Control.DefaultForeColor;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                dgvGrid.Update();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.SelectedRows.Count != 1)
            {
                e.Cancel = true;
                return;
            }

            var obj = dgvGrid.CurrentRow.DataBoundItem;

            tsmiStart.Enabled = !GetProperty<Boolean>(obj, "IsRunning");
            tsmiSuppend.Enabled = GetProperty<Boolean>(obj, "IsRunning");
        }

        private void tsmiSuppend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.SelectedRows.Count != 1)
                {
                    return;
                }

                var obj = dgvGrid.CurrentRow.DataBoundItem;
                bool isRunning = GetProperty<Boolean>(obj, "IsRunning");
                if (isRunning != true)
                {
                    return;
                }

                Call(obj, "Shutdown");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show("服务暂停失败：\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.SelectedRows.Count != 1)
                {
                    return;
                }

                var obj = dgvGrid.CurrentRow.DataBoundItem;
                bool isRunning = GetProperty<Boolean>(obj,"IsRunning");
                if (isRunning == true)
                {
                    return;
                }

                Call(obj, "Launch");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show("服务启动失败：\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void 复制服务地址到切换板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.SelectedRows.Count != 1)
                {
                    return;
                }

                var obj = dgvGrid.CurrentRow.DataBoundItem;
                string[] addresses = GetProperty<String[]>(obj,"BaseAddresses");
                if (addresses != null && addresses.Length > 0)
                {
                    string value = string.Join("\r\n", addresses);
                    Clipboard.SetText(value);
                    MessageBox.Show("已将服务地址复制到剪切板", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("复制服务地址失败：\r\n未找到任何服务地址", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                MessageBox.Show("复制服务地址失败：\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var item in MatedataServicesManager.Hosts)
            {
                Call(item, "Shutdown");
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                this.ShowInTaskbar = true;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            显示ToolStripMenuItem_Click(null, null);
        }


        T GetProperty<T>(Object obj, String propertyName)
        {
            var p = obj.GetType().GetProperty(propertyName);

            return (T)p.GetValue(obj, null);
        }

        void Call(Object obj, String methodName)
        {
            var m = obj.GetType().GetMethod(methodName);

            m.Invoke(obj, null);
        }
    }
}
