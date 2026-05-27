using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wcs;

namespace Wcs.App.Plugins.SystemInfo
{
    [WcsPluginInfo(typeof(SystemInfoPlugin), "系统信息", "Sineva", "2014/4/15 14:38:30", "用于查看目前模块的加载情况等基本的系统信息", false)]
    public class SystemInfoPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            //如果需要添加到菜单项中，可使用以下注释的代码模板
            ToolStripMenuItem tsmiSystemInfoPlugin = new ToolStripMenuItem("系统信息");
            tsmiSystemInfoPlugin.Click += tsmiSystemInfoPlugin_Click;
            context.Application.GetMenu(WcsApplicationMenuType.Help).DropDownItems.Add(tsmiSystemInfoPlugin);

            return base.Initialization(context);
        }

        void tsmiSystemInfoPlugin_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_systemInformation(this.Context))
            {
                frm.ShowDialog();
            }
        }
    }
}
