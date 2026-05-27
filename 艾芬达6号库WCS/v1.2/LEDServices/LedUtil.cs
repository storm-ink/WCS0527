using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace LEDServices
{
   public static class LedUtil
   {
       private static Logger _logger = LogManager.GetCurrentClassLogger();

       /// <summary>
       /// 将内容发送到 led。
       /// </summary>
       public static void SendAreaContents(string ledId, string areaName, string text)
       {
           _logger.Debug("正在调用 LED 服务，ledId 是【{0}】，areaName 是【{1}】，内容是【{2}】。", ledId, areaName, text);

           try
           {
               LEDService.LEDServiceClient client = new LEDService.LEDServiceClient();
               try
               {
                   client.SendDynamicAreaContents(ledId, areaName, text);
                   client.Close();
                   _logger.Info("已调用 LED 服务，ledId 是【{0}】，areaName 是【{1}】。", ledId, areaName);
               }
               catch (Exception ex)
               {
                   client.Abort();
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               string msg = string.Format("调用 LED 服务时出错，ledId 是【{0}】，areaName 是【{1}】。", ledId, areaName);
               _logger.ErrorException(msg, ex);
           }
       }

       public static void SendAreaContentsAsync(string ledId, string areaName, string text)
       {
           _logger.Debug("正在调用 LED 服务（异步），ledId 是【{0}】，areaName 是【{1}】，内容是【{2}】。", ledId, areaName, text);

           Action action = () =>
           {
               LedUtil.SendAreaContents(ledId, areaName, text);
           };

           AsyncCallback callback = ar =>
           {
               action.EndInvoke(ar);
           };

           action.BeginInvoke(callback, null);
       }

       /// <summary>
       /// 向字符串的右侧补充空格，使达到固定的宽度（以字节数记）。
       /// </summary>
       /// <param name="str"></param>
       /// <param name="totalByteCount">宽度。</param>
       /// <returns></returns>
       public static string PadRight(string str, int totalByteCount)
       {
           int count = 0;
           if (!string.IsNullOrEmpty(str))
           {
               count = Encoding.GetEncoding("GB2312").GetByteCount(str);
           }
           int paddingCount = totalByteCount - count;
           if (paddingCount <= 0)
           {
               return str;
           }
           string white = new string(' ', paddingCount);
           return str + white;
       }

       /// <summary>
       /// LED 动态区域的每行宽度（以字节数记）。
       /// </summary>
       public const int ByteCountPerLine = 16;


       /// <summary>
       /// 通过出库口名称来获取LED的区域名称
       /// </summary>
       /// <param name="portName"></param>
       /// <returns></returns>
       //public static string GetNumberTextOfPort(string portName)
       //{
       //    switch (portName)
       //    {
       //        case "发货口一":
       //            return "发货① ";
       //        case "发货口二":
       //            return "发货② ";
       //        case "发货口三":
       //            return "发货③ ";
       //        case "发货口四":
       //            return "发货④ ";
       //    }
       //    return string.Empty;
       //}
   }
}
