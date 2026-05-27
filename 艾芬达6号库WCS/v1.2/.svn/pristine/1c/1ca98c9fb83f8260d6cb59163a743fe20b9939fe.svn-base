using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.App.Plugins.TwoForksCraneManualClient
{
    public class LockerHelper
    {
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static String GetIpAddress()
        {
            string hostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostName);

            if (ipEntry.AddressList.Length > 1)
            {
                if (ipEntry.AddressList.Any(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                {
                    return ipEntry.AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .ToString();
                }
                else
                {
                    return ipEntry.AddressList.Last().ToString();
                }
            }
            else
            {
                return ipEntry.AddressList[0].ToString();
            }
        }

        public static String GetUserName()
        {
            return System.Environment.UserName;
        }
    }
}
