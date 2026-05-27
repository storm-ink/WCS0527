
/*
 * 开 发 者: 朱庆丰
 * 开发时间: 2011/06/17
 * 模块名称: frmInput
 * 模块说明: 数字输入界面
 * 备    注: 
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    public partial class frmInput1 : Form
    {
        bool bLoad = true;

        /// <summary></summary>
        public frmInput1(String[] values)
        {
            InitializeComponent();

            if (values != null && values.Length > 0)
            {
                lblInpuNum.Items.AddRange(values);
            }
        }

        /// <summary>显示 数字输入界面</summary>
        /// <param name="sNum">初始值</param>
        /// <param name="iX">显示位置, X 坐标</param>
        /// <param name="iY">显示位置, Y 坐标</param>
        public static DialogResult Show(string sNum, int iX, int iY,String[] values,out string value)
        {
            frmInput1 frm = new frmInput1(values);
            frm.Left = iX; frm.Top = iY;
            frm.lblInpuNum.Text = sNum;
            frm.lblInpuNum.Tag = sNum;

            var result = frm.ShowDialog();

            value =  frm.lblInpuNum.Text.Trim();

            return result;
        }

        // 取消, 清除, 后退, 确认, 数字录入
        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblInpuNum.Text = Convert.ToString(lblInpuNum.Tag);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            lblInpuNum.Text = "";
        }
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (lblInpuNum.Text.Length == 0) return;

            lblInpuNum.Text = lblInpuNum.Text.Remove(lblInpuNum.Text.Length - 1, 1);
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                lblInpuNum.Text = ((Button)sender).Text; bLoad = false;
            }
            else
            { 
                // 控制输入长度不超长, 临时用 TabIndex 属性记录最大输入长度
                if (lblInpuNum.Text.Length == lblInpuNum.TabIndex) return;

                lblInpuNum.Text += ((Button)sender).Text;
            }
        }
    }
}
