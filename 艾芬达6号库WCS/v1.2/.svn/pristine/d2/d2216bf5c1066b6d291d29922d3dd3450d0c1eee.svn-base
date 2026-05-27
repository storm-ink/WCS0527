using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;


namespace OnlineLogDiagnosis
{
    [WcsPluginInfo(typeof(OnlineLogDiagnosisPlugin), "文件实时日志", "Sineva", "2023年10月", "文件日志实时输出界面", false, "日志管理", "文件日志", 5, 2, 0)]
    public class OnlineLogDiagnosisPlugin : WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "文件实时日志";
            barButtonItem.Id = 0;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);
        }

        StartUpThreedOnlineLogDiagnosis frm;
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "日志管理\\文件日志\\文件实时日志")]//20231113
        void tsmi_Click(object sender, EventArgs e)
        {
            if (frm != null && !frm.IsDisposed && !frm.Disposing)
            {
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    var _frm = Application.OpenForms[i];
                    if (_frm is DevExpress.XtraEditors.XtraForm form)
                    {
                        if (form == frm)
                        {
                            frm.Activate();
                            frm.TopMost = true;
                            frm.TopMost = false;
                            return;
                        }
                    }
                }
                foreach (DevExpress.XtraEditors.XtraForm form in Application.OpenForms)
                {
                }
            }

            frm = new StartUpThreedOnlineLogDiagnosis();
            frm.Show();
        }
    }
}
