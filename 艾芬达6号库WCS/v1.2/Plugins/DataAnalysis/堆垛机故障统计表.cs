using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.DataAnalysis
{
    public partial class 堆垛机故障统计表 : Form
    {
        public 堆垛机故障统计表()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception ex)
            {

                MessageBox.Show(string.Format("数据加赞失败。{0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void load()
        {

            string deviceName = null;

            DateTime? from = null;

            DateTime? to = null;



            if (!string.IsNullOrWhiteSpace(tbxDeviceName.Text.Trim()))
            {
                deviceName = tbxDeviceName.Text.ToString();

            }
            if (dateTimeStart.Checked)
            {
                from = dateTimeStart.Value;
            }

            if (dateTimeEnd.Checked)
            {
                to = dateTimeEnd.Value;
            }
            CraneDeviceDataAnalysis.CraneDeviceChartDataAdapter adp = new CraneDeviceDataAnalysis.CraneDeviceChartDataAdapter();


            dgvGrid.AutoGenerateColumns = false;
            dgvGrid.DataSource = adp.FindErrorExecutionTimes(deviceName, from, to).ToList();

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
            if (e.ColumnIndex == _持续时间列.Index)
            {
                var row = dgvGrid.Rows[e.RowIndex];
                var data = row.DataBoundItem as ErrorExecutionTime;
                e.Value = new TimeSpan(0, 0, Convert.ToInt32(data.TotalSeconds));
            }
        }

        private void btnExel_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.CheckFileExists = false;
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.FileName = "日志";
                    saveFileDialog.Filter = "XML 电子表格 2003|*.xml";
                    saveFileDialog.ValidateNames = true;
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                    {
                        return;
                    }

                    fileName = saveFileDialog.FileName;
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return;
                }

                dgvGrid.ExportAsExcel(fileName);

                if (MessageBox.Show("导出成功，是否立即打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}