using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;
using Wcs.Framework.Events;
using NHibernate.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Wcs.FrameworkExtend;
using Newtonsoft.Json;

namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    public partial class frmTaskViewer : Form
    {
        Logger _logger;

        TaskChooseEndLocationHandler taskChooseEndLocationHandler;

        Int32 _taskId;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\计划任务\\历史计划任务详情查看")]
        public frmTaskViewer(Int32 taskId)
        {
            _taskId = taskId;
            _logger = NLog.LogManager.GetCurrentClassLogger();

            InitializeComponent();
        }

        private void frmTaskViewer_Load(object sender, EventArgs e)
        {
            try
            {
                load(_taskId);
                taskChooseEndLocationHandler = Wcs.Framework.Cfg.WcsConfiguration.Instance.TaskChouseEndLocationHandlersElement.TaskChooseEndLocationHandler;
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                this.Close();
                return;
            }
        }
        void load(Int32 taskId)
        {

            PreTask preTask = null;
            new System.Threading.Tasks.TaskFactory().StartNew(() =>
            {
                try
                {
                    using (NHBackupServerUnitOfWork unitOfWork = new NHBackupServerUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                    {
                        preTask = unitOfWork.session.Get<PreTask>(taskId);

                        unitOfWork.Commit();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, this);
                }
            }).Wait();

            this.BeginInvoke(new Action(() =>
            {

                if (preTask == null)
                {
                    throw new Exception(string.Format("未找到 id 为 {0} 的 Task 对象", taskId));
                }

                this.Text = string.Format("查看任务 {0}#{1} 信息", preTask.Id, preTask.TaskCode);

                lblTaskCode.Text = string.Format("{0}#{1}", preTask.Id, preTask.TaskCode);
                lblStartLocation.Text = string.Format("{0}@{1}", preTask.StartLocation.UserCode, preTask.StartLocation.DeviceName);
                lblEndLocation.Text = string.Format("{0}@{1}", preTask.EndLocation.UserCode, preTask.EndLocation.DeviceName);
                lblBzType.Text = preTask.BizType.GetDescription();
                lblSource.Text = preTask.Source.GetDescription();
                lblStatus.Text = preTask.Status.GetDescription();
                lblPriority.Text = preTask.Priority.ToString();
                lblCreatedAt.Text = preTask.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                tbx_description.Text = preTask.Description;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(preTask.AdditionalInfo))
                    tbx_additionalinfo.Text = "-";
                else
                {
                    dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(preTask.AdditionalInfo);
                    tbx_additionalinfo.Text = string.Join(",", dic.Select(x => x.Key + "=" + x.Value));
                }
            }));
        }

        #region 对象事件

        private void frmTaskViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        #endregion

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private void btnEsc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
