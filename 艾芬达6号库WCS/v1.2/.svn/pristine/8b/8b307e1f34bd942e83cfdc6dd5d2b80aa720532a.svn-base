using DevExpress.XtraBars;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.CraneTest
{
    [WcsPluginInfo(typeof(CraneTestPlugin), "堆垛机测试工具", "Sineva", "2022年6月", "仅供相关开发人员使用。", false, "工具箱", "测试工具", 3, 1, 0)]
    public class CraneTestPlugin : Wcs.WcsPlugin
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "堆垛机测试";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmiCraneTestPlugin_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        frmMain frmCraneTestPlugin;

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "工具箱\\测试工具\\堆垛机测试")]
        void tsmiCraneTestPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                string pwd = "";
                if (InputBox("请输入密码", "密码：", ref pwd) != DialogResult.OK)
                {
                    return;
                }

                if (pwd != "Sineva@123")
                {
                    MessageBox.Show("密码错误，请重试！");
                    return;
                }

                if (frmCraneTestPlugin != null && !frmCraneTestPlugin.IsDisposed && !frmCraneTestPlugin.Disposing)
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form == frmCraneTestPlugin)
                        {
                            frmCraneTestPlugin.Focus();
                            frmCraneTestPlugin.Activate();
                            frmCraneTestPlugin.TopMost = true;
                            return;
                        }
                    }
                }

                frmCraneTestPlugin = new frmMain();
                frmCraneTestPlugin.Show();
                frmCraneTestPlugin.WindowState = FormWindowState.Maximized;
                frmCraneTestPlugin.Activate();
            }
            catch (System.Security.SecurityException securityException)
            {
                _logger.Error1(securityException, this);
                MessageBox.Show(securityException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is System.Security.SecurityException)
                {
                    MessageBox.Show(ex.InnerException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _logger.Error1(ex, this);
            }
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
