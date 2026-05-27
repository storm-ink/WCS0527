using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs.Framework;

namespace Wcs.App.Plugins.EquipmentActionSchedulerManager
{
    public partial class frmMethodTrace : Form
    {
        EquipmentActionScheduler _scheduler;
        public frmMethodTrace(EquipmentActionScheduler scheduler)
        {
            InitializeComponent();
            _scheduler = scheduler;
            this.Text = string.Format("{0} 运行状态监视", scheduler);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(bmp);
            g.Clear(Control.DefaultBackColor);
            _scheduler._methodDescriptorTree.Paint(g);
            g.Dispose();
            this.CreateGraphics().DrawImage(bmp, 0, 0);

            if (checkBox1.Checked)
            {
                var node = _scheduler._methodDescriptorTree.AllMethodDescriptors.FirstOrDefault(x => x.AccessResult == false);
                if (node != null)
                {
                    string msg = string.Format(@"节点名称：{0}
说   明：{1}
访问时间：{2}
访问结果：{3}
访问说明：{4}", node.Name, node.Description, node.AccessAt, node.AccessResult, node.AccessDescription);
                    richTextBox1.Text = msg;
                }
                else
                {
                    richTextBox1.Clear();
                }
            }
            
        }

        private void frmMethodTrace_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_scheduler._methodDescriptorTree == null)
            {
                return;
            }

            var node = _scheduler._methodDescriptorTree.AllMethodDescriptors.FirstOrDefault(x => x.TestHit(e.X, e.Y));
            if (node != null)
            {
                this.Cursor = Cursors.Hand;
                string msg = string.Format(@"节点名称：{0}
说   明：{1}
访问时间：{2}
访问结果：{3}
访问说明：{4}", node.Name, node.Description, node.AccessAt, node.AccessResult, node.AccessDescription);
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_scheduler._methodDescriptorTree == null)
            {
                return;
            }

            var node = _scheduler._methodDescriptorTree.AllMethodDescriptors.FirstOrDefault(x => x.TestHit(e.X, e.Y));
            if (node != null)
            {
                string msg = string.Format(@"节点名称：{0}
说   明：{1}
访问时间：{2}
访问结果：{3}
访问说明：{4}", node.Name, node.Description, node.AccessAt, node.AccessResult, node.AccessDescription);
                richTextBox1.Text = msg;
            }
            else
            {
                richTextBox1.Clear();
            }
        }
    }
}
