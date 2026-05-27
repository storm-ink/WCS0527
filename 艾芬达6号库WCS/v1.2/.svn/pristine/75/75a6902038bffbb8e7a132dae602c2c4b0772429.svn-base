using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ManualTask
{
    public partial class frmNextLinks : Form
    {
        private List<string> _links;
        private string _end;
        public frmNextLinks(string end,IEnumerable<string> links, AutoCompleteStringCollection acsc)
        {
            InitializeComponent();

            _links = new List<string>(links);
            _end = end;
            tbxTo.AutoCompleteCustomSource = acsc;


            tbxFrom.Text = _links.LastOrDefault()?? _end;

            for (var i = 0; i < _links.Count; i++)
            {
                if (i == 0)
                {
                    var item = listView1.Items.Add(_end);
                    item.SubItems.Add(_links[i]);
                }
                else
                {
                    var item = listView1.Items.Add(_links[i-1]);
                    item.SubItems.Add(_links[i]);
                }
            }
        }

        public String[] Links
        {
            get { return _links.ToArray(); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void AddLink(String newTo)
        {
            var from = _links.LastOrDefault() ?? _end;

            if (string.IsNullOrWhiteSpace(newTo))
            {
                MessageBox.Show("请输入新结束点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxTo.Focus();
                return;
            }

            if (string.Equals(from, newTo, StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("新的结束点和上一个位置的结束点一样", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxTo.Focus();
                return;
            }

            var loc1 = Wcs.Framework.LocationConverter.UserCodeToLcation(from);
            var loc2= Wcs.Framework.LocationConverter.UserCodeToLcation(newTo);
            var task = new Wcs.Framework.Task("CK001",
                new Framework.LocationInfo(loc1.Device.Name, loc1.DeviceCode, loc1.UserCode, loc1.UnifiedCode),
                new Framework.LocationInfo(loc2.Device.Name, loc2.DeviceCode, loc2.UserCode, loc2.UnifiedCode))
            {
                BizType = Framework.TaskBizType.Normal
            };

            if (Wcs.Framework.PathHelper.FindNextPath(task, loc1, loc2, null, null, Framework.RouteType.Normal).Count ==
                0)
            {
                MessageBox.Show(string.Format("从{0}到{1}无法连通。",from,newTo), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbxTo.Focus();
                return;
            }

            _links.Add(newTo);

            AddRow(from, newTo);

            tbxFrom.Text = newTo;

            tbxTo.Text = "";
        }

        void AddRow(string from, string to)
        {
            var item = listView1.Items.Add(from);
            item.SubItems.Add(to);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var to = tbxTo.Text;

            AddLink(to);
        }
    }
}
