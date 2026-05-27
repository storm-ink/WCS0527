using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ProxyManager
{
    public class ProxyManagerPlugin:Wcs.WcsPlugin
    {
        public override bool Initialization(WcsContext context)
        {
            var menu = (ToolStripMenuItem)context.Application.GetMenu(WcsApplicationMenuType.Tools)
                .DropDownItems.Add("子系统", null);

            var hide_mi = menu.DropDownItems.Add("全部隐藏", null, (sender, e) =>
            {
                bool error = false;
                foreach (Wcs.DefaultImpls.Proxy.ClientDevice client 
                    in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection
                    .ParticularDeviceCollection.SelectMany(x=>x.DeviceElements)
                    .Where(x=>x.Device is Wcs.DefaultImpls.Proxy.ClientDevice)
                    .Select(x=>x.Device)
                    )
                {
                    try
                    {
                        if (client.Hide() == false && error == false)
                        {
                            error = true;
                        }
                    }
                    catch (Exception)
                    {
                        if (error == false)
                        {
                            error = true;
                        }

                    }
                }

                if (error)
                {
                   //发生了一些异常
                }
            });

            var show_mi = menu.DropDownItems.Add("全部显示", null, (sender, e) =>
            {
                bool error = false;
                foreach (Wcs.DefaultImpls.Proxy.ClientDevice client
                    in Wcs.Framework.Cfg.WcsConfiguration.Instance.DeviceCollection
                    .ParticularDeviceCollection.SelectMany(x => x.DeviceElements)
                    .Where(x => x.Device is Wcs.DefaultImpls.Proxy.ClientDevice)
                    .Select(x => x.Device)
                    )
                {
                    try
                    {
                        if (client.Show() == false && error == false)
                        {
                            error = true;
                        }
                    }
                    catch (Exception)
                    {
                        if (error == false)
                        {
                            error = true;
                        }

                    }
                }

                if (error)
                {
                    //发生了一些异常
                }

            });



            return base.Initialization(context);
        }
    }
}
