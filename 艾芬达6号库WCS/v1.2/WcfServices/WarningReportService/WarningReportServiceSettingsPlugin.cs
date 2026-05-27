using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wcs.Services.Wcf
{
    public class WarningReportServiceSettingsPlugin:WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var m = context.Application
                .GetMenu(WcsApplicationMenuType.Edit)
                .DropDownItems.Add("设置故障统计参数");

            m.Click += m_Click;
            return base.Initialization(context);
        }

        void m_Click(object sender, EventArgs e)
        {
            using (frmSetting frm = new frmSetting())
            {
                frm.ShowDialog();
            }
        }
    }
}
