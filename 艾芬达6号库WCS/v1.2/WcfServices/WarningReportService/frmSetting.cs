using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.Services.Wcf
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        void loadSettings()
        {
            mtbDefaultWorkStartTime.Text = WarningReportService.GetDefaultWorkStartTime().ToString();
            mtbDefaultOffDutyTime.Text = WarningReportService.GetDefaultOffDutyTime().ToString();
            dtpProjectUsingTime.Value = WarningReportService.GetProjectStartUsingTime();

            var devices=Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection.ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                .Select(x => x.Device)
                .ToArray();

            DgvGrid.RowCount = devices.Length;
            for (int i = 0; i < devices.Length; i++)
            {
                var row = DgvGrid.Rows[i];
                var device = devices[i];

                row.Cells["colDeviceType"].Value = device.GetDeviceType();
                row.Cells["colDeviceName"].Value = device.Name;

                var workStartTime = WarningReportService.GetWorkStartTime(device.GetDeviceType(), device.Name);
                var offDutyTime = WarningReportService.GetOffDutyTime(device.GetDeviceType(), device.Name);

                if (mtbDefaultWorkStartTime.Text != workStartTime.ToString())
                {
                    row.Cells["colWorkStartTime"].Value = workStartTime;
                }
                else
                {
                    row.Cells["colWorkStartTime"].Value = TimeSpan.Zero;
                }
                if (mtbDefaultOffDutyTime.Text != offDutyTime.ToString())
                {
                    row.Cells["colOffDutyTime"].Value = offDutyTime;
                }
                else
                {
                    row.Cells["colOffDutyTime"].Value = TimeSpan.Zero;
                }
            }
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void DgvGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
        }

        private void DgvGrid_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            String v = Convert.ToString(e.Value);
            TimeSpan ts;
            if (TimeSpan.TryParse(v, out ts))
            {
                e.Value = ts;
            }
            else
            {
                e.Value = TimeSpan.Zero;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan defaultWorkStartTime, defaultOffDutyTime;
                DateTime projectUsingTime = dtpProjectUsingTime.Value.Date ;
                if (!TimeSpan.TryParse(mtbDefaultWorkStartTime.Text, out defaultWorkStartTime))
                {
                    MessageBox.Show("请输入有效的设置上班时间", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbDefaultWorkStartTime.Focus();
                    return;
                }

                if (!TimeSpan.TryParse(mtbDefaultOffDutyTime.Text, out defaultOffDutyTime))
                {
                    MessageBox.Show("请输入有效的设置下班时间", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mtbDefaultOffDutyTime.Focus();
                    return;
                }

                WarningReportService.SetDefaultWorkStartTime(defaultWorkStartTime);
                WarningReportService.SetDefaultOffDutyTime(defaultOffDutyTime);
                WarningReportService.SetProjectStartUsingTime(projectUsingTime);

                foreach (var row in DgvGrid.Rows.Cast<DataGridViewRow>())
                {
                    var deviceType = Convert.ToString(row.Cells["colDeviceType"].Value);
                    var deviceName = Convert.ToString(row.Cells["colDeviceName"].Value);
                    var workStartTimeValue = Convert.ToString(row.Cells["colWorkStartTime"].Value);
                    var offDutyTimeValue = Convert.ToString(row.Cells["colOffDutyTime"].Value);
                    TimeSpan workStartTime, offDutyTime;

                    if (TimeSpan.TryParse(workStartTimeValue, out workStartTime) && workStartTime != TimeSpan.Zero)
                    {
                        WarningReportService.SetWorkStartTime(deviceType, deviceName, workStartTime);
                    }

                    if (TimeSpan.TryParse(offDutyTimeValue, out offDutyTime) && offDutyTime != TimeSpan.Zero)
                    {
                        WarningReportService.SetOffDutyTime(deviceType, deviceName, offDutyTime);
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
