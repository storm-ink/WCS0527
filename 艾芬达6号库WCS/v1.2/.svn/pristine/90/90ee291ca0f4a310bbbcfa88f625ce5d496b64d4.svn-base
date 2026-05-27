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

namespace Wcs.App.Plugins.RequestManager
{
    public partial class frmMain : Form
    {
        Logger _logger;
        BindingSource _bindingSource;

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "请求管理\\查看")]
        public frmMain()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
            
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            dgvGrid.DataSource = _bindingSource;

            var status = Wcs.EnumExtentions.ToKeyValueList<RequestStatus>();
            status.Insert(0, new KeyValuePair<string, string>("", ""));
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DataSource = status;

            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<RequestAddedEvent>(onRequestAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<RequestArchivedEvent>(onRequestArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Subscribe<RequestStatusChangedEvent>(onRequestStatusChanged);
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
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<RequestAddedEvent>(onRequestAdded);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<RequestArchivedEvent>(onRequestArchived);
            Wcs.Framework.EventBus.EventBus.Instance.Unsubscribe<RequestStatusChangedEvent>(onRequestStatusChanged);
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
            var request = (Request)dgvGrid.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == colStatus.Index)
            {
                e.Value = request.Status.GetDescription();
            }
            else if (e.ColumnIndex == colContainerCode.Index)
            {
                e.Value = String.Join(",", request.ContainerCodes.ToArray());
            }
            else if (e.ColumnIndex == colAdditionalInfo.Index)
            {
                e.Value = String.Join(",", request.AdditionalInfo.Select(x=>string.Format("{0}：{1}",x.Key,x.Value)).ToArray());
            }
        }

        void load()
        {
            List<Request> list;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                var q = unitOfWork.session.Query<Request>();
                if (!string.IsNullOrWhiteSpace(tbxSource.Text))
                {
                    q = q.Where(x => x.Source.UserCode.Contains(tbxSource.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxContainerCode.Text))
                {
                    q = q.Where(x => x.ContainerCodes.Any(containerCode => containerCode.Contains(tbxContainerCode.Text.Trim())));
                }


                if (!String.IsNullOrWhiteSpace(Convert.ToString(cmbStatus.SelectedValue)))
                {
                    RequestStatus requestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), Convert.ToString(cmbStatus.SelectedValue));
                    q = q.Where(x => x.Status == requestStatus);
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

        void onRequestAdded(RequestAddedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<RequestAddedEvent> act = (_args) =>
                {
                    onRequestAdded(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<Request> requests = _bindingSource.Cast<Request>().ToList();
                if (requests != null)
                {
                    var request = requests.FirstOrDefault(x => x.Id == args.Request.Id);
                    if (request != null)
                    {
                        request.Id = args.Request.Id;
                        request.Status = args.Request.Status;

                        _bindingSource.ResetBindings(false);
                        return;
                    }
                }

                _bindingSource.Add(args.Request);
            }
        }

        void onRequestArchived(RequestArchivedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<RequestArchivedEvent> act = (_args) =>
                {
                    onRequestArchived(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<Request> requests = _bindingSource.Cast<Request>().ToList();
                if (requests != null)
                {

                    var request = requests.FirstOrDefault(x => x.Id == args.Id);
                    if (request == null)
                    {
                        return;
                    }

                    _bindingSource.Remove(request);
                }
            }
        }

        void onRequestStatusChanged(RequestStatusChangedEvent args)
        {
            if (this.InvokeRequired)
            {
                Action<RequestStatusChangedEvent> act = (_args) =>
                {
                    onRequestStatusChanged(_args);
                };

                this.Invoke(act, args);
            }
            else
            {
                List<Request> requests = _bindingSource.Cast<Request>().ToList();
                if (requests == null)
                {
                    return;
                }
                var request = requests.FirstOrDefault(x => x.Id == args.Id);
                if (request == null)
                {
                    return;
                }

                request.Status = args.Status;

                _bindingSource.ResetBindings(false);
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

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "请求管理\\归档")]
        private void 归档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var request = dgvGrid.CurrentRow.DataBoundItem as Request;
                
                String msg;
                if (request.Status == RequestStatus.New)
                {
                    msg = string.Format("该请求还未处理，人工归档可能会丢失一些业务触发信号。\n\n是否继续？", request);
                }
                else
                {
                    msg = string.Format("确定要归档 {0} 吗？", request);
                }
                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                _logger.Trace1(string.Format("准备人工删除 {0}", request), this, request);
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    var deleteObj = unitOfWork.session.Get<Request>(request.Id);
                    if (deleteObj == null)
                    {
                        throw new ApplicationException(string.Format("未找到 id 为 {0} 的 Request 对象",request.Id));
                    }

                    unitOfWork.session.Delete(deleteObj);

                    unitOfWork.Commit();
                }

                _logger.Info1(string.Format("人工 {0} 删除成功", request), this, request);

                EventBus.Instance.Publish(new Wcs.Framework.Events.RequestArchivedEvent(request.Id, request.Source, request.Status));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(string.Format("请求删除失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
