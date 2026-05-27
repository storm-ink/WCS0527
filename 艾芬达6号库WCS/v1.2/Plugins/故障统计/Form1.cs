using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Xml;

namespace Wcs.App.Plugins.堆垛机任务故障查询
{
    public partial class Form1 : Form
    {

      

        public Form1()
        {
            InitializeComponent(); 
            dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpEndDate.Value = new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
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
            }
        }




        void load()
        {


            StringBuilder sb = new StringBuilder();
            sb.Append("select * from [EquipmentActionWarnings] ");
            sb.Append(" where 1=1 ");

            List<SqlParameter> param = new List<SqlParameter>();
            if (dtpStartDate.Checked)
            {
                sb.Append(" and CreatedAt>=@startDate");
                param.Add(new SqlParameter("startDate", dtpStartDate.Value));

            }
            if (dtpEndDate.Checked)
            {
                sb.Append(" and CreatedAt<=@endDate");
                param.Add(new SqlParameter("endDate", dtpEndDate.Value));
            }
            if (!string.IsNullOrWhiteSpace(tbxDeviceName.Text))
            {
                sb.Append(" and DeviceName like '%'+@DeviceName+'%'");
                param.Add(new SqlParameter("DeviceName", tbxDeviceName.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(tbxCode.Text))
            {
                sb.Append(" and Code like '%'+@Code+'%'");
                param.Add(new SqlParameter("Code", tbxCode.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(tbxDescription.Text.Trim()))
            {
                sb.Append(" and Description like '%'+@Description+'%'");
                param.Add(new SqlParameter("Description", tbxDescription.Text.Trim()));
            }


            var key = tbxKey.Text.Trim();
            Int32 keyIntValue;
            Int32.TryParse(key, out keyIntValue);
            if (!string.IsNullOrWhiteSpace(key))
            {

                sb.Append(@" and (equipmentactionid=@equipmentactionid 
                                or code like '%'+@code+'%'
                                or [description] like '%'+@description+'%'
                                or devicename like '%'+@devicename+'%')");
            }
            
            String connectionString;
            if (checkBox1.Checked)
            {
                connectionString = WcsConfigurationConnectionStrings.WcsBakConnectionString;
            }
            else
            {
                connectionString = WcsConfigurationConnectionStrings.WcsConnectionString;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sb.ToString(), connection))
                {
                    cmd.Parameters.AddRange(param.ToArray());
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        DataTable dtb = new DataTable();
                        adp.Fill(dtb);

                        dgvGrid.AutoGenerateColumns = false;
                        dgvGrid.DataSource = dtb;
                    }
                }
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

        private void btn导出_Click(object sender, EventArgs e)
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