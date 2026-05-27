using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.HomePage
{
    public partial class frmHomePage : DevExpress.XtraEditors.XtraForm
    {
        public frmHomePage()
        {
            InitializeComponent();
        }

        List<DeviceAlarm> list = new List<DeviceAlarm>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var _list = HomePageHelper.last.Where(x => !x.IsOK).ToList();
                if (string.Join("/", _list.Select(x => x.ToString())) == string.Join("/", list.Select(x => x.ToString())))
                    return;

                list = _list;
                dataGridView1.DataSource = list.OrderBy(x => getPiroirty(x)).ToList();
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch
            {
            }
        }

        private object getPiroirty(DeviceAlarm deviceAlarm)
        {
            if (int.TryParse(deviceAlarm.DeviceName, out int result))
                return result;
            else
                return 0;
        }

        private void frmHomePage_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}