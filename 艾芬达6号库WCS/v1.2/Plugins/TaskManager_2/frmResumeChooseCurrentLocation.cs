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
    public partial class frmResumeChooseCurrentLocation : Form
    {
        RouteHead _route;
        Task _task;
        public frmResumeChooseCurrentLocation(Task task,RouteHead route)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            _route = route;
            _task = task;

            InitializeComponent();
        }

        public Location CurrentLocation
        {
            get
            {
                if (dgvPath.CurrentCell == null)
                {
                    return null;
                }

                return dgvPath.CurrentCell.Tag as Location;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var startLocation = this.CurrentLocation;

            if (startLocation == null)
            {
                MessageBox.Show("必须选择一个任务的当前实际停靠位置", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this._route.Details.Last().Path == startLocation.DeviceCode)
            {
                MessageBox.Show("任务的开始位置不能是路径的末位，如果确认货物已经在路径的结束位置，请强制完成对应的任务后再尝试继续执行", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var endLocation = LocationConverter.ToLocation(_task.EndLocation);

            if (startLocation.Equals(endLocation))
            {
                MessageBox.Show("任务的开始位置不能是任务的末位，如果确认货物已经在路径的结束位置，请强制完成对应的任务后再尝试继续执行", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCacnel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmResumeChooseCurrentLocation_Load(object sender, EventArgs e)
        {
            var locations = _route.Details.Select(x => LocationConverter.ConvertibleCodeToLcation(x.Path + "@" + x.Device)).ToArray();

            var destinationIndex = locations.FindIndex(
                x => x.Device.Name == _task.EndLocation.DeviceName
                && x.DeviceCode == _task.EndLocation.DeviceCode);
            if (destinationIndex >= 0)
            {
                locations = locations.Take(destinationIndex+1).ToArray();
            }

            int rowCount = 0;
            if (locations.Length < 5)
            {
                rowCount = 1;
            }
            else
            {
                if (locations.Length % 5 == 0)
                {
                    rowCount = (locations.Length / 5);
                }
                else
                {
                    rowCount = (locations.Length / 5) + 1;
                }
            }

            this.Height += 24 * (rowCount - 1);

            for (int i = 1; i <= 5; i++)
            {
                DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
                dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPath.Columns.Add(dgvc);
            }

            dgvPath.RowCount = rowCount;
            for (int i = 0; i < dgvPath.Rows.Count; i++)
            {
                DataGridViewRow row = dgvPath.Rows[i];
                for (int j = 0; j < dgvPath.ColumnCount; j++)
                {
                    var index = i * 5 + j;
                    DataGridViewCell cell = row.Cells[j];
                    if (index < locations.Length)
                    {
                        cell.Value = locations[index].DeviceCode;
                        cell.Tag = locations[index];
                        cell.ToolTipText = locations[index].UserCode;
                    }
                    else
                    {
                        cell.Tag = null;
                    }
                }
            }

            var currentLocation = dgvPath.Rows.Cast<DataGridViewRow>()
                .SelectMany(row=>row.Cells.Cast<DataGridViewCell>())
                .FirstOrDefault(x => (x.Tag as Location).Equals(LocationConverter.ToLocation(_task.CurrentLocation)));
            
            if (currentLocation != null)
            {
                currentLocation.Selected = true;
                dgvPath.CurrentCell = currentLocation;
            }

            tbxRouteId.Text = _route.Id.ToString();
            tbxRouteNo.Text = _route.No.ToString();
            tbxDevice.Text = _route.Device;
            tbxDestination.Text = _task.EndLocation.UserCode;
        }
    }
}
