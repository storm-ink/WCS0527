using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;

namespace OnlineLogDiagnosis
{
    public partial class StartUpThreedOnlineLogDiagnosis : DevExpress.XtraEditors.XtraForm
    {
        protected override CreateParams CreateParams//双缓存
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public StartUpThreedOnlineLogDiagnosis()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void StartUpThreedOnlineLogDiagnosis_Load(object sender, EventArgs e)
        {
            try
            {
                if (LogWriteToFileHelper.paths != null)
                {
                    var paths = LogWriteToFileHelper.paths.ToArray();
                    if (paths.Count() > 0)
                    {
                        var items = paths.OrderBy(x => x.Length).ToArray();
                        checkedListBox1.Items.AddRange(items);
                        comboBoxEdit1.Properties.Items.AddRange(items);

                        var settings = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("LogWriteToFile", "");
                        for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        {
                            if (settings.Contains(checkedListBox1.Items[i].ToString()))
                                checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
                comboBoxEdit1.Text = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"载入失败，异常消息：{ex}");
            }
        }

        List<string> list = new List<string>();
        private void LogWriteToFileHelper_FileLogWrite(object sender, FileLogWriteEventArgs e)
        {
            if (e.Path != comboBoxEdit1.Text)
                return;
            Action action = () =>
            {
                try
                {
                    lock (list)
                    {
                        if (list.Count > 10)
                            list.RemoveAt(0);
                        list.Add(e.Message);
                        update();
                    }
                }
                catch
                {
                }
            };
            action.Invoke();

            //System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            //{

            //}, e);
        }

        private void update()
        {
            try
            {
                var array = list.ToArray();
                List<string> lst = new List<string>();
                foreach (var item in array)
                {
                    if (item.Length > 5000)
                        lst.Add(item.Substring(0, 500) + "...\r\n数据量过大请输入到文本中打开");
                    else
                        lst.Add(item);
                }
                var showTest = string.Join("\r\n", lst.ToArray().Reverse());
                richTextBoxEx1.Text = showTest;
            }
            catch
            {
            }
        }

        private void StartUpThreedOnlineLogDiagnosis_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkEdit1.Checked)
                LogWriteToFileHelper.FileLogWrite -= LogWriteToFileHelper_FileLogWrite;
            this.Close();
            this.Dispose();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string setting = "";
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(setting))
                        setting += $"{checkedListBox1.CheckedItems[i]}";
                    else
                        setting += $",{checkedListBox1.CheckedItems[i]}";
                }
                Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<string>("LogWriteToFile", setting);

                XtraMessageBox.Show($"设置成功");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"设置失败，异常消息{ex}");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                    return;

                checkedListBox1.Items.Clear();
                if (LogWriteToFileHelper.paths != null)
                {
                    var paths = LogWriteToFileHelper.paths.ToArray();
                    if (paths.Count() > 0)
                    {
                        var _path = textBox1.Text;
                        var items = paths.Where(x => x.Contains(_path)).OrderBy(x => x.Length).ToArray();
                        checkedListBox1.Items.AddRange(items);
                        comboBoxEdit1.Properties.Items.AddRange(items);

                        var settings = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<string>("LogWriteToFile", "");
                        for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        {
                            if (settings.Contains(checkedListBox1.Items[i].ToString()))
                                checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void checkEdit1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                LogWriteToFileHelper.FileLogWrite += LogWriteToFileHelper_FileLogWrite;
                comboBoxEdit1.Enabled = false;
            }
            else
            {
                LogWriteToFileHelper.FileLogWrite -= LogWriteToFileHelper_FileLogWrite;
                comboBoxEdit1.Enabled = true;
            }
        }

        private void comboBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                comboBoxEdit1.Properties.Items.Clear();
                if (string.IsNullOrWhiteSpace(comboBoxEdit1.Text))
                    comboBoxEdit1.Properties.Items.AddRange(LogWriteToFileHelper.paths.ToArray());
                else
                {
                    var paths = LogWriteToFileHelper.paths.ToArray();
                    if (paths.Count() > 0)
                    {
                        var items = paths.OrderBy(x => x.Length).Where(x => x.ToUpper().Contains(comboBoxEdit1.Text.ToUpper().Trim())).ToArray();
                        comboBoxEdit1.Properties.Items.AddRange(items);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

        }
    }
}