using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NHibernate.Linq;
using Wcs.Framework.Cfg;
using Wcs.Framework.EventBus;
using Wcs;
using NLog;
using Wcs.FrameworkExtend;
using Newtonsoft.Json;
using System.Collections;

namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    public partial class frmMain : Form
    {
        //WcsContext WcsContext;
        BindingSource _bindingSource;

        static Logger _logger = LogManager.GetCurrentClassLogger();
        public frmMain()
        {
            InitializeComponent();

            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<PreTask>();
            dgvGrid.DataSource = _bindingSource;
        }

        public frmMain(WcsContext context)
        {
            InitializeComponent();

            //WcsContext = context;
            dgvGrid.AutoGenerateColumns = false;
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<PreTask>();
            dgvGrid.DataSource = _bindingSource;

            var status = Wcs.EnumExtentions.ToKeyValueList<Framework.TaskStatus>();
            status.Insert(0, new KeyValuePair<string, string>("", ""));

        }

        private void paging1_EventPaging(EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //WcsContext.Application.Logger.Error1(ex, this);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //load();
                //paging1.Bind();
                //this.paging1.EventPaging += new EventPagingHandler(paging1_EventPaging);//初始化自定义事件

                System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
                {
                    loadTaskTypes();
                }, null);
            }
            catch (Exception ex)
            {
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("任务数据加载失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        void load()
        {
            List<PreTask> list;
            using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                paging1.TotalCount = unitOfWork.session.Query<PreTask>().Count();

                var q = unitOfWork.session.Query<PreTask>();
                if (!string.IsNullOrWhiteSpace(tbxTaskCode.Text))
                {
                    q = q.Where(x => x.TaskCode.Contains(tbxTaskCode.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxContainerCode.Text))
                {
                    q = q.Where(x => x.ContainerCodes.Contains(tbxContainerCode.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxStartLocation.Text))
                {
                    q = q.Where(x => x.StartLocation.UserCode.Contains(tbxStartLocation.Text.Trim())
                        || x.StartLocation.DeviceCode.Contains(tbxStartLocation.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(tbxEndLocation.Text))
                {
                    q = q.Where(x => x.EndLocation.UserCode.Contains(tbxEndLocation.Text.Trim())
                        || x.EndLocation.DeviceCode.Contains(tbxEndLocation.Text.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(cbxTaskTypes.Text))
                {
                    if (cbxTaskTypes.Text == "【空字符串】")
                    {
                        q = q.Where(x => string.IsNullOrWhiteSpace(x.TaskType));
                    }
                    else
                    {
                        q = q.Where(x => x.TaskType == cbxTaskTypes.Text.Trim());
                    }
                }
                switch (cbxFromWms.CheckState)
                {
                    case CheckState.Unchecked:
                        q = q.Where(x => x.Source != Framework.TaskSource.Wms);
                        break;
                    case CheckState.Checked:
                        q = q.Where(x => x.Source == Framework.TaskSource.Wms);
                        break;
                    case CheckState.Indeterminate:
                    default:
                        break;
                }

                list = q.ToList();
                unitOfWork.Commit();
            }

            paging1.QueryCount = list.Count();

            if (paging1.PageSize >= 0)
                list = list.Skip((paging1.CurrentPage - 1) * paging1.PageSize).Take(paging1.PageSize).ToList();

            _bindingSource.Clear();

            foreach (var item in list)
            {
                _bindingSource.Add((PreTask)item);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                load();
                paging1.Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("数据加载失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //WcsContext.Application.Logger.Error1(ex, this);
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

        private void tbxTaskCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        void loadTaskTypes()
        {
            try
            {
                using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    var taskTypes = unitOfWork.session.Query<PreTask>()
                        .GroupBy(x => x.TaskType)
                        .Select(x => x.Key)
                        .ToArray();

                    unitOfWork.Commit();

                    this.Invoke(new MethodInvoker(() =>
                    {
                        try
                        {
                            cbxTaskTypes.Items.Clear();
                            cbxTaskTypes.Items.Add("");
                            foreach (var item in taskTypes)
                            {
                                if (String.IsNullOrWhiteSpace(item))
                                {
                                    var key = "【空字符串】";
                                    cbxTaskTypes.Items.Add(key);
                                }
                                else
                                {
                                    cbxTaskTypes.Items.Add(item);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error1(ex, this);
                            //WcsContext.Application.Logger.Error1(ex, this);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                //WcsContext.Application.Logger.Error1(ex, this);
            }
        }
        private void dgvGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            tsmiView_Click(null, null);
        }

        private void tsmiView_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvGrid.CurrentRow == null)
                {
                    return;
                }

                var task = dgvGrid.CurrentRow.DataBoundItem as PreTask;

                using (frmTaskViewer frm = new frmTaskViewer(task.Id))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                //WcsContext.Application.Logger.Error1(ex, this);
                MessageBox.Show(string.Format("打开任务详情失败：{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
