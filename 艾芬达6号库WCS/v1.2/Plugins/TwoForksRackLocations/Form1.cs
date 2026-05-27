using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwoForksRackLocations.asdf;

namespace TwoForksRackLocations
{
    /// <summary>
    /// 双货叉堆垛机 配置采用分组形式
    /// 
    /// </summary>
    public partial class frm : Form
    {
        BindingSource _bindingSource;
        public frm()
        {
            InitializeComponent();
        }

        private void frm_Load(object sender, EventArgs e)
        {
            _bindingSource = new BindingSource();
            _bindingSource.DataSource = new List<UserLocationRelationship>();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _bindingSource;

            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.DataSource = LoadAllBuilder().ToArray();
        }


        private void btnAddRelationships_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbxStartUserColumn.Text))
            {
                MessageBox.Show("必要的用户起始列数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }
            if (String.IsNullOrWhiteSpace(tbxStartUserLevel.Text))
            {
                MessageBox.Show("必要的用户起始层数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbxStartColumn.Text))
            {
                MessageBox.Show("必要的设备起始列数据不能为空，请补充完整后再尝试添加对应关系！");
               return; 
            }

            if (String.IsNullOrWhiteSpace(tbxStartLevel.Text))
            {
                MessageBox.Show("必要的设备起始层数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbxEndUserColumn.Text))
            {
                MessageBox.Show("必要的用户结束列数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbxEndUserLevel.Text))
            {
                MessageBox.Show("必要的用户结束层数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            UserLocationRelationship r = new UserLocationRelationship
            {
                 FromColumn=Convert.ToInt32(tbxStartColumn.Text),
                 FromLevel = Convert.ToInt32(tbxStartLevel.Text),
                 FromUserColumn = Convert.ToInt32(tbxStartUserColumn.Text),
                 FromUserLevel = Convert.ToInt32(tbxStartUserLevel.Text),
                 ToUserColumn = Convert.ToInt32(tbxEndUserColumn.Text),
                 ToUserLevel = Convert.ToInt32(tbxEndUserLevel.Text),
                 列递减=cbxUserLevelJ.Checked,
                 组大小 = Convert.ToInt32(tbxGroupSize.Text)
            };

            if (cbxStep1.Checked)
            {
                r.Step1 = new stepInfo();
                r.Step1.叉1能用 = cbxFork1Can.Checked;
                r.Step1.叉2能用 = cbxFork2Can.Checked;
                r.Step1.定位 = cbxDW.Checked;
                r.Step1.每组中隔几列 = Convert.ToInt32(tbxGT.Text);
            }

            if (cbxStep2.Checked)
            {
                r.Step2 = new stepInfo();
                r.Step2.叉1能用 = cbxFork1Can2.Checked;
                r.Step2.叉2能用 = cbxFork2Can2.Checked;
                r.Step1.定位 = cbxDW2.Checked;
                r.Step2.每组中隔几列 = Convert.ToInt32(tbxGT2.Text);
            }

            if (cbxStep3.Checked)
            {
                r.Step3 = new stepInfo();
                r.Step3.叉1能用 = cbxFork1Can3.Checked;
                r.Step3.叉2能用 = cbxFork2Can3.Checked;
                r.Step1.定位 = cbxDW3.Checked;
                r.Step3.每组中隔几列 = Convert.ToInt32(tbxGT3.Text);
            }

            _bindingSource.Add(r);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrWhiteSpace(tbx_Laneway.Text))
            {
                MessageBox.Show("必要的巷道名称数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }
            if (String.IsNullOrWhiteSpace(tbxleftRackNo.Text))
            {
                MessageBox.Show("必要的左排数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            if (String.IsNullOrWhiteSpace(tbxrightRackNo.Text))
            {
                MessageBox.Show("必要的右排数据不能为空，请补充完整后再尝试添加对应关系！");
                return;
            }

            var type = (Type)comboBox1.SelectedValue;



            asdf.IRackLocationBuilder xx = (asdf.IRackLocationBuilder)type.Assembly.CreateInstance(type.FullName,false,System.Reflection.BindingFlags.CreateInstance,null,null,null,null);
            CreateRackLocationSettings settings = new CreateRackLocationSettings();

            settings.LanewayName = tbx_Laneway.Text.ToString();
            settings.LeftRackNo = Convert.ToInt32(tbxleftRackNo.Text);
            settings.RightRackNo = Convert.ToInt32(tbxrightRackNo.Text);
            settings.Left2RackNo = Convert.ToInt32(tbxLeft2.Text);
            settings.Right2RackNo = Convert.ToInt32(tbxRight2.Text);

            foreach (var item in _bindingSource.Cast<UserLocationRelationship>())
            {
                settings.AddUserLocationRelationship(item);
            }

            var text = xx.Create(settings);
            tbx_ShowResult.Clear();
            tbx_ShowResult.Text = text;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            _bindingSource.Remove(dataGridView1.CurrentRow.DataBoundItem);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UserLocationRelationship currentData = null;
            int index = -1;
            if (dataGridView1.CurrentRow != null)
            {
                currentData = (UserLocationRelationship)dataGridView1.CurrentRow.DataBoundItem;
                index = dataGridView1.CurrentRow.Index;
            }

            if (currentData == null)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }

            if (index > 0 && dataGridView1.RowCount > 1)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }

            if (index <dataGridView1.RowCount-1 && index>-1)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserLocationRelationship currentData = null;
            int index = -1;
            if (dataGridView1.CurrentRow != null)
            {
                currentData = (UserLocationRelationship)dataGridView1.CurrentRow.DataBoundItem;
                index = dataGridView1.CurrentRow.Index;
            }

            if (index > 0)
            {
                _bindingSource.Remove(currentData);
                _bindingSource.Insert(index - 1, currentData);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserLocationRelationship currentData = null;
            int index = -1;
            if (dataGridView1.CurrentRow != null)
            {
                currentData = (UserLocationRelationship)dataGridView1.CurrentRow.DataBoundItem;
                index = dataGridView1.CurrentRow.Index;
            }

            if (index < dataGridView1.RowCount-1)
            {
                _bindingSource.Remove(currentData);
                _bindingSource.Insert(index + 1, currentData);
            }

        }


        Dictionary<Type, String> LoadAllBuilder()
        {
            Dictionary<Type, String> r = new Dictionary<Type, string>();
            foreach (var item in System.Reflection.Assembly.GetAssembly(this.GetType()).GetTypes())
            {
                if (item.IsInterface)
                {
                    continue;
                }

                if (item.GetInterfaces().Any(x => x == typeof(asdf.IRackLocationBuilder)))
                {
                    r.Add(item, item.Name);
                }
            }

            return r;
        }

        private void cbxUserLevelJ_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                var currentData = (UserLocationRelationship)item.DataBoundItem;
                currentData.列递减 = cbxUserLevelJ.Checked;
            }
        }

        private void cbxStep1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = cbxStep1.Checked;
        }

        private void cbxStep2_CheckedChanged(object sender, EventArgs e)
        {

            groupBox3.Enabled = cbxStep2.Checked;
        }

        private void cbxStep3_CheckedChanged(object sender, EventArgs e)
        {

            groupBox4.Enabled = cbxStep3.Checked;
        }
        
    }
}
