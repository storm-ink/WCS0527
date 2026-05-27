using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.TwoForksTaskViewer
{
    public class TwoForksTaskViewerPligin : Wcs.WcsPlugin
    {
        frmTasks frmMain;
        public override bool Initialization(WcsContext context)
        {
            var menu = (ToolStripMenuItem)context.Application.GetMenu(WcsApplicationMenuType.Tools).DropDownItems.Add("双货叉任务");

            //var mi2 = (ToolStripMenuItem)menu.DropDownItems.Add("分配查询");
            //mi2.Click += Mi2_Click;

            var mi1 = (ToolStripMenuItem)menu.DropDownItems.Add("历史查询");
            mi1.Click += Mi1_Click;

            return base.Initialization(context);
        }

        //[Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "双货叉任务\\分配查询")]
        //private void Mi2_Click(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "双货叉任务\\历史查询")]
        private void Mi1_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmMain != null && !frmMain.IsDisposed && !frmMain.Disposing)
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form == frmMain)
                        {
                            frmMain.WindowState = FormWindowState.Maximized;
                            frmMain.Focus();
                            frmMain.Activate();
                            return;
                        }
                    }
                }

                frmMain = new frmTasks();
                frmMain.MdiParent = this.Context.Application.MainForm;
                frmMain.Show();
                frmMain.WindowState = FormWindowState.Maximized;
                frmMain.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Context.Application.Logger.Error1(ex, this);
            }
        }
    }
}
