using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCSValidity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbx_start.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var _str = ValidityHelper.WeiYiJiaMiGuid(tbx_start.Text.Trim() + "_" + tbx_dateCount.Text.Trim() + "_0");
            var _jmstr = ValidityHelper.WeiYiJieMiGuid(_str);
            tbx_str.Text = _str;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = ValidityHelper.IsOverTime(tbx_str.Text.Trim());
        }
    }
}
