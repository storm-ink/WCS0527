using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;

namespace Wcs.App.Plugins.TaskManager
{
    public partial class frmChooseNewEndLocation : Form
    {
        Task _task;
        public frmChooseNewEndLocation(Task tsk)
        {
            InitializeComponent();

            _task = tsk;

            lblTaskNo.Text = tsk.TaskCode;
            lblContainerCodes.Text = String.Join(",", tsk.ContainerCodes);
            lblTaskType.Text = tsk.TaskType;
            lblAdditationInfo.Text = string.Join("；", tsk.AdditionalInfo.Select(x => String.Format("{0}={1}", x.Key, x.Value)));
        }

        /// <summary>
        /// 新任务终点
        /// </summary>
        public LocationInfo NewEndLocation
        {
            get
            {
                return cbxEndLocation.SelectedItem as LocationInfo;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmChooseNewEndLocation_Load(object sender, EventArgs e)
        {
            cbxEndLocation.DisplayMember = "UserCode";
            cbxEndLocation.DataSource = Wcs.Framework.Cfg.WcsConfiguration.Instance.LocationCollection.Locations.Where(x => !(x is ILocationWildcard)).ToList();
            foreach (Location item in cbxEndLocation.Items)
            {
                if (item.UserCode == _task.EndLocation.UserCode)
                {
                    cbxEndLocation.SelectedItem = item;
                }
            }
        }
    }
}
