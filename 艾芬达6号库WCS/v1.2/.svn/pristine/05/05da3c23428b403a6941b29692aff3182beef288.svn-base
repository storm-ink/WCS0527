using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.RackLocation
{
    public partial class frmMain : Form
    {
       
        public frmMain()
        {
            InitializeComponent();
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {

                LocationInfo startColumn;
                LocationInfo endColumn;
                if (tbxStartUserColumn.Text.Trim() != ""
                && tbxStartColumn.Text.Trim() != ""
                && tbxEndUserColumn.Text.Trim() != ""
                )
                {
                    startColumn = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxStartUserColumn.Text.Trim()),
                        deviceValue = Convert.ToInt32(tbxStartColumn.Text.Trim()),
                    };

                    endColumn = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxEndUserColumn.Text)
                    };
                }
                else if (tbxStartUserColumn.Text.Trim() == ""
                && tbxStartColumn.Text.Trim() == ""
                && tbxEndUserColumn.Text.Trim() == ""
                    )
                {
                    tbx_ShowResult.Text = "未填写列数据，因此没有生成货架！";
                    return;
                }
                else
                {
                    MessageBox.Show("必要的列数据不足，请补充完整后再尝试生成货架！");
                    return;
                }

                List<LocationItem> result = new List<LocationItem>();

                LocationInfo startLevel;
                LocationInfo endLevel;
                //层的第一种对应关系
                if (tbxStartUserLevel0.Text.Trim() != ""
                    && tbxStartLevel0.Text.Trim() != ""
                    && tbxEndUserLevel0.Text.Trim() != ""
                    )
                {
                    startLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxStartUserLevel0.Text),
                        deviceValue = Convert.ToInt32(tbxStartLevel0.Text),
                    };
                    endLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxEndUserLevel0.Text)
                    };
                    int height = 0;
                    if (!string.IsNullOrWhiteSpace(tbxLevelHeight0.Text))
                        height = Convert.ToInt32(tbxLevelHeight0.Text);
                    string linePre = "";
                    if (!string.IsNullOrWhiteSpace(tbxLinepre.Text))
                        linePre = tbxLinepre.Text;

                    RacklocationBuilder builder = new RacklocationBuilder(startColumn, endColumn, startLevel, endLevel, Convert.ToInt32(tbxLine.Text), comboBox2.Text, checkBox1.Checked, height, linePre);
                    result.AddRange(builder.Result);

                    //textBox11.Text = "";
                    //textBox11.Text += builder.ToString();
                }


                //层的第二种对应关系
                if (tbxStartUserLevel1.Text.Trim() != ""
                    && tbxStartLevel1.Text.Trim() != ""
                    && tbxEndUserLevel1.Text.Trim() != ""
                    )
                {
                    startLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxStartUserLevel1.Text),
                        deviceValue = Convert.ToInt32(tbxStartLevel1.Text),
                    };
                    endLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxEndUserLevel1.Text)
                    };
                    int height = 0;
                    if (!string.IsNullOrWhiteSpace(tbxLevelHeight1.Text))
                        height = Convert.ToInt32(tbxLevelHeight1.Text);
                    string linePre = "";
                    if (!string.IsNullOrWhiteSpace(tbxLinepre.Text))
                        linePre = tbxLinepre.Text;
                    RacklocationBuilder builder = new RacklocationBuilder(startColumn, endColumn, startLevel, endLevel, Convert.ToInt32(tbxLine.Text), comboBox2.Text, checkBox1.Checked, height, linePre);

                    result.AddRange(builder.Result);

                    //textBox11.Text += builder.ToString();
                }


                //层的第三种对应关系
                if (tbxStartUserLevel2.Text.Trim() != ""
                    && tbxStartLevel2.Text.Trim() != ""
                    && tbxEndUserLevel2.Text.Trim() != ""
                    )
                {
                    startLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxStartUserLevel2.Text),
                        deviceValue = Convert.ToInt32(tbxStartLevel2.Text),
                    };
                    endLevel = new LocationInfo
                    {
                        userValue = Convert.ToInt32(tbxEndUserLevel2.Text)
                    };
                    int height = 0;
                    if (!string.IsNullOrWhiteSpace(tbxLevelHeight2.Text))
                        height = Convert.ToInt32(tbxLevelHeight2.Text);
                    string linePre = "";
                    if (!string.IsNullOrWhiteSpace(tbxLinepre.Text))
                        linePre = tbxLinepre.Text;
                    RacklocationBuilder builder = new RacklocationBuilder(startColumn, endColumn, startLevel, endLevel, Convert.ToInt32(tbxLine.Text), comboBox2.Text, checkBox1.Checked, height, linePre);
                    //textBox11.Text += builder.ToString();
                    result.AddRange(builder.Result);
                }

                显示(result);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void 显示(List<LocationItem> items)
        {
            tbx_ShowResult.Clear();

            tbx_ShowResult.Text = string.Join("\r", items.OrderBy(x => x.UserColumn).Select(x => x.ElementOuterXml));
        }
       
    }
}
