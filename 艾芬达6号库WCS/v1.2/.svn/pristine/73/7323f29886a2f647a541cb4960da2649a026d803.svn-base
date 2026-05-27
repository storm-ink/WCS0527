using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ManualTask
{
    [WcsPluginInfo(typeof(ManualTaskPlugin), "手工任务", "Sineva", "2022年6月", "提供一个测试时使用的手工任务添加界面", false, "任务管理", "手工任务", 1, 2, 0)]
    public class ManualTaskPlugin : WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "手工任务";
            barButtonItem.Id = 0;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmAddTask frmAddTask;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\手工任务\\新增")]
        void tsmi_Click(object sender, EventArgs e)
        {
            //string pwd = "";
            //if (InputBox("请输入密码", "密码：", ref pwd) != DialogResult.OK)
            //{
            //    return;
            //}

            //var password = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("手工任务密码", "1235");

            //if (pwd != password)
            //{
            //    return;
            //}

            if (frmAddTask != null && !frmAddTask.IsDisposed && !frmAddTask.Disposing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form == frmAddTask)
                    {
                        frmAddTask.Focus();
                        frmAddTask.Activate();
                        frmAddTask.TopMost = true;
                        return;
                    }
                }
            }

            frmAddTask = new frmAddTask();
            frmAddTask.Show();
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

    }
}
