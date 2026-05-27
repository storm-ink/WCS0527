using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;
using Wcs.Framework;

namespace Wcs.App.Plugins.OutTaskFirst
{
    [WcsPluginInfo(typeof(OutTaskFirstPlugin), "出库优先选项", "Sineva", "2014/4/29 16:20:42", "提供一个选项，允许用户关闭或启动“出库优先”功能。", false)]
    public class OutTaskFirstPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            //如果需要添加到菜单项中，可使用以下注释的代码模板
            ToolStripMenuItem tsmiOutTaskFirstPlugin = new ToolStripMenuItem("出库优先");
            bool v = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("出库优先");
            tsmiOutTaskFirstPlugin.Checked = v;
            tsmiOutTaskFirstPlugin.Click += tsmiOutTaskFirstPlugin_Click;

            context.Application.GetMenu(WcsApplicationMenuType.Edit).DropDownItems.Add(tsmiOutTaskFirstPlugin);

            return base.Initialization(context);
        }
        
        [Wcs.Security.WcsPermission(System.Security.Permissions.SecurityAction.Demand, OperationName = "任务管理\\出库优先设置")]
        void tsmiOutTaskFirstPlugin_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var v = !item.Checked;
            item.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>("出库优先", v);
        }
    }
}
