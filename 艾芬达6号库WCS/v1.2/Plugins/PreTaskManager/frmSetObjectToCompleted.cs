using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework.Exceptions;
using NHibernate.Linq;
using Wcs.Framework;
using NLog;

namespace Wcs.App.Plugins.PreTaskManager
{
    public partial class frmSetObjectToCompleted : Form
    {
        Int32 _taskId;
        Logger _logger;
        public frmSetObjectToCompleted(Int32 taskId)
        {
            InitializeComponent();

            _taskId = taskId;
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }

        List<dynamic> getObjects()
        {
            Task task;
            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                task = unitOfWork
                    .session.Get<Task>(_taskId);

                unitOfWork.Commit();
            }

            if (task == null)
            {
                throw new TaskNotFoundException(_taskId);
            }

            List<dynamic> objs = new List<dynamic>();
            objs.Add(task);

            var suspendMovement = task.Movements.LastOrDefault(x => x.Status == LogicMovementStatus.Error || x.Status == LogicMovementStatus.Suspend);
            if (suspendMovement != null)
            {
                objs.Add(suspendMovement);
                var suspendAction = suspendMovement.EquipmentActions.LastOrDefault(x => x.Status ==EquipmentActionStatus.Suspend || x.Status==EquipmentActionStatus.Error);
                if (suspendAction != null)
                {
                    objs.Add(suspendAction);
                }
            }

            return objs;
        }

        private void frmSetObjectToCompleted_Load(object sender, EventArgs e)
        {
            try
            {
                var objs = getObjects();
                for (int i = 0; i < objs.Count(); i++)
                {
                    //30 20
                    RadioButton rb = new RadioButton();
                    rb.AutoSize = true;
                    rb.Text = Convert.ToString(objs.ElementAt(i));
                    rb.Tag = objs.ElementAt(i);
                    rb.Left = 30;
                    rb.Top = 20 + 20 * i;
                    rb.CheckedChanged += (ctl, args) =>
                    {
                        btnOK.Enabled = SelectedObject != null;
                    };
                    this.Controls.Add(rb);
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex,this);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public dynamic SelectedObject
        {
            get
            {
                return this.Controls
                    .Cast<Control>()
                    .Where(x => x is RadioButton && (x as RadioButton).Checked)
                    .Select(x => x.Tag)
                    .SingleOrDefault();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SelectedObject == null)
            {
                MessageBox.Show("请先您要取消的任务类型", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
