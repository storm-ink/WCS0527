using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ConveyorLocation
{
    public partial class frmConveyorLocation : Form
    {
        public frmConveyorLocation()
        {
            InitializeComponent();
        }

   
        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                Location deviceCodeStart = new Location
                {
                    deviceCode = Convert.ToInt32(txtdeviceCode.Text),

                };

                Location deviceCodeEnd = new Location
                {
                    deviceCode = Convert.ToInt32(txtdeviceCodeEnd.Text),
                };



                UsercodeBuilder builder = new UsercodeBuilder(deviceCodeStart, deviceCodeEnd);

                textBox11.Text = builder.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
