using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.Reports
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (frmCraneWarningReport frm = new frmCraneWarningReport())
            {
                frm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (frmTaskReport frm = new frmTaskReport())
            {
                frm.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (frmEquipmentActionsReport frm = new frmEquipmentActionsReport())
            {
                frm.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (frmConveyorWarningReport frm = new frmConveyorWarningReport())
            {
                frm.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
