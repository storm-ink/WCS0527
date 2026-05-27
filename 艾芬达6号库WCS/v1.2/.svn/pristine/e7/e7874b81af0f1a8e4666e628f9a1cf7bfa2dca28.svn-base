using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wcs;

namespace DeviceEventQueueSettings
{
    [WcsPluginInfo(typeof(DeviceEventQueueSettingsPlugin), "设置", "Sineva", "2013年4月", "设备事件设置", false, "设备管理", "设备事件", 2, 2, 0)]
    public class DeviceEventQueueSettingsPlugin : Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            barButtonItem.Caption = "动作序列管理";
            barButtonItem.Id = 1;
            barButtonItem.Name = $"barBtn_{this.GetType().Name.ToLower()}";
            barButtonItem.ItemClick += tsmi_Click;
            this.barButtonItems = new BarButtonItem[] { barButtonItem };

            return base.Initialization(context);

            var menu = (ToolStripMenuItem)context.Application.GetMenu(WcsApplicationMenuType.Edit).DropDownItems.Add("设备事件");
            var item1 = (ToolStripMenuItem)menu.DropDownItems.Add("异步处理");
            item1.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("设备事件.异步处理");
            item1.ToolTipText = "勾选此选项后设备引发的和任务相关的事件将被异步派发，否则将使用同步方式派发。";
            item1.Click += Item_Click;

            var item2 = (ToolStripMenuItem)menu.DropDownItems.Add("记录事件派发日志");
            item2.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("deviceEventQueue-logger-enabled");
            item2.ToolTipText = "勾选此选项后设备事件派发器产生的所有日志将被输出。";
            item2.Click += Item2_Click;

            var item3 = (ToolStripMenuItem)menu.DropDownItems.Add("记录事件总线日志");
            item3.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("eventBusEventPublisher-logger-enabled");
            item3.ToolTipText = "勾选此选项后事件总线产生的所有日志将被输出。";
            item3.Click += Item3_Click;

            return base.Initialization(context);
        }


        frmDeviceEventQueueSettings frmMain;
        void tsmi_Click(object sender, EventArgs e)
        {
            this.Context.Application.MainForm.OpenNewForm<frmDeviceEventQueueSettings>("设备事件设置");
        }
    }
}
