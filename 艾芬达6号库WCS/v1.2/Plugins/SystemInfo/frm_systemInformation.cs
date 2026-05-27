using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Wcs.App.Plugins.SystemInfo
{
    partial class frm_systemInformation : Form
    {
        WcsContext _context;
        public frm_systemInformation(WcsContext context)
        {
            _context = context;

            InitializeComponent();

            //   设置行高   20   
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 20);//分别是宽和高

            listView1.SmallImageList = imgList;

            lblVersion.Text = "版本号:v" + context.Application.GetType().Assembly
                .GetName().Version.ToString();

            Binds();
        }

        private void Binds()
        {
            var plugins = _context.FindPlugins(Application.StartupPath)
                .OrderBy(x => x.Value.IsCore ? 0 : 1)
                .ThenBy(x => x.Value.Priority);
            foreach (var item in plugins)
            {
                ListViewItem lvi = new ListViewItem(new string[]
                {
                    string.IsNullOrEmpty(item.Value.Name) || item.Value.Name=="<未配置>"?item.Key.FullName:item.Value.Name,
                    _context.LoadedPlugins.Any(x=>x.Id==item.Value.PluginId)?"是":"否"
                });
                lvi.Tag=item.Value;
                if (item.Value.IsCore)
                {
                    lvi.ForeColor = Color.Purple;
                }
                listView1.Items.Add(lvi);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0) return;
            if (listView1.SelectedItems.Count == 0) return;

            WcsPluginInfoAttribute pia = listView1.SelectedItems[0].Tag as WcsPluginInfoAttribute;

            tbxId.Text = pia.PluginId.ToString();
            tbxPriority.Text = pia.Priority.ToString();
            tbxName.Text = string.IsNullOrEmpty(pia.Name) || pia.Name == "<未配置>" ? pia.PluginType.FullName : pia.Name;
            tbxCreatedBy.Text = pia.CreatedBy;
            tbxCreatedAt.Text = pia.CreatedAt;
            tbxVersion.Text = pia.Version.ToString();
            tbxDescription.Text = pia.Description;
        }

    }
}
