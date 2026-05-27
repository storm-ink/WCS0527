
/*
 * 开 发 者: 朱庆丰
 * 开发时间: 2011/06/17
 * 模块名称: frmMsgbox
 * 模块说明: 统一界面风格的, 消息显示
 * 备    注: 
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    public partial class frmMsgbox : Form
    {
        /// <summary></summary>
        public frmMsgbox()
        {
            InitializeComponent();

            this.BackColor = Color.Black; this.ForeColor = Color.White;
        }

        /// <summary></summary>
        public static DialogResult Show(string strMsginfo)
        {
            return Show(strMsginfo, Btn.Ok);
        }
        /// <summary></summary>
        public static DialogResult Show(string strMsginfo, Btn btn)
        {
            frmMsgbox frm = new frmMsgbox(); frm.lblMsginfo.Text = strMsginfo;

            if (btn == Btn.Ok) { frm.btnCancel.Visible = false; frm.btnOk.Left = 130; }

            return frm.ShowDialog();
        }
    }
}
