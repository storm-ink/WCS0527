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
using Wcs.Framework;
using Wcs.Framework.Events;
using NHibernate.Linq;
using Wcs.Framework.EventBus;

namespace Wcs.App.Plugins.EventsManager
{
    public partial class frmMain : Form
    {
        Logger _logger;
        BindingSource _bindingSource;

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "延迟事件管理\\查看")]
        public frmMain()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
            
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<IEvent>();
            dgvGrid.DataSource = _bindingSource;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error1(ex, this);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            var evt = (IEvent)dgvGrid.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == colType.Index)
            {
                e.Value = evt.GetType().Name;
            }
            else if (e.ColumnIndex == colName.Index)
            {
                e.Value = evt.GetType().GetDisplayName();
            }
            else if (e.ColumnIndex == colDescription.Index)
            {
                e.Value = evt.ToDescription();
            }
        }

        void load()
        {
            List<IEvent> list;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<IEvent>().ToList();
                if (!string.IsNullOrWhiteSpace(tbxTypeOrName.Text))
                {
                    q = q.Where(x =>
                        x.GetType().FullName.Contains(tbxTypeOrName.Text.Trim())
                        || x.GetType().GetDisplayName().Contains(tbxTypeOrName.Text.Trim())
                        )
                        .ToList();
                }

                list = q.ToList();

                unitOfWork.Commit();
            }

            _bindingSource.Clear();

            foreach (var item in list)
            {
                _bindingSource.Add(item);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error1(ex, this);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvGrid.CurrentRow == null || dgvGrid.SelectedRows.Count!=1)
            {
                e.Cancel = true;
            }
        }

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "延迟事件管理\\归档")]
        private void 归档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var evt = dgvGrid.CurrentRow.DataBoundItem as IEvent;

                String msg = string.Format("该事件还未处理，人工归档可能会丢失一些业务触发信号。\n\n是否继续？", evt);
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                _logger.Trace1(string.Format("准备人工删除 {0}", evt), this, evt);

                EventBusEventPublisher.Pop(evt);

                _logger.Info1(string.Format("人工 {0} 删除成功", evt), this, evt);
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("延迟事件删除失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
