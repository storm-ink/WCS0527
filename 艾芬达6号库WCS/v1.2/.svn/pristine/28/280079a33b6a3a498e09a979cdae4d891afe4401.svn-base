using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.MessageBoard
{
    public partial class frmMessage : Form
    {
        public frmMessage(Wcs.Framework.MessageBoard.AbstractMessage msg)
        {
            InitializeComponent();

            try
            {
                xmlViewer1.Text = msg.GetData();
                xmlViewer1.Process(true);
            }
            catch (Exception ex)
            {
                xmlViewer1.Text = msg.GetData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
