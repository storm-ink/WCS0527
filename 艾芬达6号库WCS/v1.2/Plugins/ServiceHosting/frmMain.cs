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

namespace Wcs.App.Plugins.ServiceHosting
{
    public partial class frmMain : Form
    {
        List<dynamic> _hosts;
        BindingSource _bindingSource;
        Logger _logger;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "Wcf服务宿主\\查看")]
        public frmMain(List<dynamic> hosts)
        {
            _hosts = hosts;
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<dynamic>();
            dgvGrid.DataSource = _bindingSource;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in _hosts)
                {
                    _bindingSource.Add(item);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
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
            var host = (dynamic)dgvGrid.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == colName.Index)
            {
                e.Value = host.Name;
            }
            else if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = host.IsRunning ? "运行" : "停止";
            }
            else if (e.ColumnIndex == colAddress.Index)
            {
                if (host.BaseAddresses != null)
                {
                    e.Value = String.Join(",", host.BaseAddresses);
                }
                else
                {
                    e.Value = "";
                }
            }

            if (host.IsRunning)
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
                _bindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.SelectedRows.Count != 1)
            {
                e.Cancel = true;
                return;
            }

            var obj = (dynamic)dgvGrid.CurrentRow.DataBoundItem;

            tsmiStart.Enabled = !obj.IsRunning;
            tsmiSuppend.Enabled = obj.IsRunning;
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "Wcf服务宿主\\停止服务")]
        private void tsmiSuppend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.SelectedRows.Count != 1)
                {
                    return;
                }

                var obj = (dynamic)dgvGrid.CurrentRow.DataBoundItem;
                bool isRunning = obj.IsRunning;
                if (isRunning != true)
                {
                    return;
                }

                obj.Shutdown();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show("服务暂停失败：\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "Wcf服务宿主\\启动服务")]
        private void tsmiStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.SelectedRows.Count != 1)
                {
                    return;
                }

                var obj = (dynamic)dgvGrid.CurrentRow.DataBoundItem;
                bool isRunning = obj.IsRunning;
                if (isRunning == true)
                {
                    return;
                }

                obj.Launch();
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
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

                var obj = (dynamic)dgvGrid.CurrentRow.DataBoundItem;
                var isRunningProperty = obj.GetType().GetProperty("BaseAddresses");
                string[] addresses = obj.BaseAddresses;
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
                _logger.Error1(ex, this);
                MessageBox.Show("复制服务地址失败：\r\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
