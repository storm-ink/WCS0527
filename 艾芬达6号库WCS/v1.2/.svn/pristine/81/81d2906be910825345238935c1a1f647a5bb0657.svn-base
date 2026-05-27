
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

namespace Wcs.App.Plugins.CraneManualClient
{
    public partial class frmInput : Form
    {
        bool bLoad = true;

        /// <summary></summary>
        public frmInput()
        {
            InitializeComponent();
        }

        /// <summary>显示 数字输入界面</summary>
        /// <param name="sNum">初始值</param>
        /// <param name="iX">显示位置, X 坐标</param>
        /// <param name="iY">显示位置, Y 坐标</param>
        /// <param name="iMax">限定输入的最大值</param>
        /// <param name="iMin">限定输入的最小值</param>
        public static string Show(string sNum, int iX, int iY, int iMax, int iMin)
        {
            frmInput frm = new frmInput();
            frm.Left = iX; frm.Top = iY;
            frm.lblMaxNum.Text = Convert.ToString(iMax);
            frm.lblMinNum.Text = Convert.ToString(iMin);
            frm.lblInpuNum.Text = sNum;
            frm.lblInpuNum.Tag = sNum;

            frm.ShowDialog();

            if (frm.lblInpuNum.Text.Length > 0)
            {
                int iInput = Convert.ToInt32(frm.lblInpuNum.Text);

                if (iInput < iMin)
                    return Convert.ToString(iMin);
                else if (iInput > iMax)
                    return Convert.ToString(iMax);
                else
                    return Convert.ToString(iInput);
            }
            else
            {
                return Convert.ToString(iMin);
            }
        }

        // 取消, 清除, 后退, 确认, 数字录入
        private void btnCancel_Click(object sender, EventArgs e)
        {
            lblInpuNum.Text = Convert.ToString(lblInpuNum.Tag);

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
