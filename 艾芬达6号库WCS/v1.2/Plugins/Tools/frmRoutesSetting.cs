using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wcs.Framework;

namespace Wcs.App.Plugins.Tools
{
    public partial class frmRoutesSetting : Form
    {
        public frmRoutesSetting()
        {
            InitializeComponent();
            this.paging1.EventPaging += new EventPagingHandler(paging1_EventPaging);//初始化自定义事件
            /// 新添加这一句调用就行了，如果有ListViews也是这样添加，
            /// 但要注意方法里改为有关ListViews的声明即可
            dataGridView1.DoubleBufferedDataGirdView(true);
        }

        private void paging1_EventPaging(EventArgs e)
        {
            GvDataBind(); //DataGridView数据绑定
        }

        private void GvDataBind()
        {
            int SumCount = 0;
            DataTable dt;
            //获取分页
            if (paging1.PageSize == -1)//为显示全部时的情况
            {
                LoadRoute(paging1.PageSize, paging1.CurrentPage, out SumCount);
                paging1.PageSize = SumCount;
            }
            else
                LoadRoute(paging1.PageSize, paging1.CurrentPage, out SumCount);
            dataGridView1.AutoGenerateColumns = false;
            paging1.TotalCount = SumCount;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GvDataBind();
            paging1.Bind();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GvDataBind();
            paging1.Bind();
        }

        private void LoadRoute(int pageSize, int currentPage, out int sumCount)
        {
            dataGridView1.CurrentCellDirtyStateChanged -= dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged -= dataGridView1_CellValueChanged;

            var list = Wcs.Framework.RouteHelper.RouteHeads.ToArray();
            if (int.TryParse(tbx_routeId.Text, out Int32 selectRouteId))
                list = list.Where(x => x.HeadID == selectRouteId).ToArray();

            if (!string.IsNullOrWhiteSpace(tbx_path.Text.Trim()))
                list = list.Where(x => string.Join(",", x.Details.Select(y => y.Path).ToArray()).Contains(tbx_path.Text)).ToArray();

            if (radioBtn_enable.Checked)
                list = list.Where(x => !Wcs.Framework.RouteHelper.DisableRouteIds.Contains(x.HeadID)).ToArray();
            if (radioBtn_unable.Checked)
                list = list.Where(x => Wcs.Framework.RouteHelper.DisableRouteIds.Contains(x.HeadID)).ToArray();

            //dataGridView1.DataSource = list;

            //计算分页显示
            sumCount = list.Length;
            var dt = GetDateShowTable(pageSize, currentPage, list);
            this.dataGridView1.DataSource = dt;


            if (!dataGridView1.Columns.Contains("Unable"))
            {
                DataGridViewCheckBoxColumn checkbox = new DataGridViewCheckBoxColumn(false);
                checkbox.Name = "Unable";
                checkbox.HeaderText = "Unable";
                dataGridView1.Columns.Add(checkbox);
            }

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (Int32.TryParse(item.Cells["HeadId"].Value.ToString(), out Int32 routeId))
                {
                    var routeHead = Wcs.Framework.RouteHelper.RouteHeads.FirstOrDefault(x => x.HeadID == routeId);

                    if (Wcs.Framework.RouteHelper.DisableRouteIds.Contains(routeId))
                    {
                        item.Cells["Unable"].Value = true;
                        item.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                else
                    break;
            }

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.Name != "Unable")
                    item.ReadOnly = true;
            }

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }

        private DataTable GetDateShowTable(int pageSize, int currentPage, RouteHead[] list)
        {
            DataTable dt = new DataTable();
            foreach (PropertyInfo propertyInfo in typeof(RouteHead).GetProperties())
            {
                if (propertyInfo.PropertyType.Name == "ISet`1")
                    continue;
                dt.Columns.Add(new DataColumn(propertyInfo.Name));
            }
            dt.Columns.Add(new DataColumn("Path"));
            dt.Columns.Add(new DataColumn("AdJoins"));
            if (pageSize == -1)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    var item = list[i];
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        var name = column.ColumnName;
                        switch (name)
                        {
                            case "Path":
                                dr[name] = string.Join(",", item.Details.Select(x => x.Path));
                                break;
                            case "AdJoins":
                                dr[name] = string.Join(",", item.Relations.Select(x => x.Adjoins));
                                break;
                            case "Unable":
                                dr[name] = false;
                                break;
                            default:
                                dr[name] = item.GetType().GetProperty(name).GetValue(item, null);
                                break;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                int i;
                if (list.Length <= pageSize)
                    i = 0;
                else
                    i = pageSize * (currentPage - 1);

                for (; i < list.Length && i < pageSize * currentPage; i++)
                {
                    var item = list[i];
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        var name = column.ColumnName;
                        switch (name)
                        {
                            case "Path":
                                dr[name] = string.Join(",", item.Details.Select(x => x.Path));
                                break;
                            case "AdJoins":
                                dr[name] = string.Join(",", item.Relations.Select(x => x.Adjoins));
                                break;
                            case "Unable":
                                dr[name] = false;
                                break;
                            default:
                                dr[name] = item.GetType().GetProperty(name).GetValue(item, null);
                                break;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex != -1 && !dataGridView1.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["HeadId"].Value.ToString(), out int routeId))
                {
                    var unable = Boolean.Parse(row.Cells["Unable"].Value.ToString());
                    Wcs.Framework.RouteHelper.SetDisableRoute(routeId, unable);
                    if (unable)
                        row.DefaultCellStyle.BackColor = Color.Red;
                    else
                        row.DefaultCellStyle.BackColor = this.dataGridView1.BackgroundColor;
                }
            }
        }

        private void paging1_Load(object sender, EventArgs e)
        {

        }
    }
    public static class DoubleBufferDataGridView
    {
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        public static void DoubleBufferedDataGirdView(this DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }
    }
}
