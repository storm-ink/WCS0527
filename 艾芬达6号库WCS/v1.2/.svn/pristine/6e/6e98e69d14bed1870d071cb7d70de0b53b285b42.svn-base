using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.DataAnalysis
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            label1.Parent = this;

            treeView1.ExpandAll();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        void show<TChart>()
            where TChart : UserControl,IChart, new()
        {
            TChart chart = new TChart();
            chart.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(chart);

            chart.Refresh(null, DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            label1.Visible = false;
            if (e.Node.Name == "堆垛机_状态数据_24小时")
            {
                show<uc堆垛机_状态数据_24小时>();
            }
            else if (e.Node.Name == "堆垛机_状态数据_天")
            {
                show<uc堆垛机_状态数据_天>();
            }
            else if (e.Node.Name == "堆垛机_故障数据_24小时")
            {
                show<uc堆垛机_故障数据_24小时>();
            }
            else if (e.Node.Name == "堆垛机_故障数据_月")
            {
                show<uc堆垛机_故障数据_月>();
            }
            else
            {
                label1.Visible = true;
                splitContainer1.Panel2.Controls.Clear();
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            label1.Left = splitContainer1.Panel2.Left + splitContainer1.Panel2.Width / 2 - label1.Width / 2;
            label1.Top = splitContainer1.Panel2.Top + splitContainer1.Panel2.Height / 2 - label1.Height / 2;
            label1.BringToFront();
        }

        堆垛机故障统计表 frm;
        private void button1_Click(object sender, EventArgs e)
        {
            frm = new 堆垛机故障统计表();

            frm.Show();
        }
    }
}
