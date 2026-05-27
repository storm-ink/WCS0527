using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs;
using Wcs.Framework;
using NLog;

namespace WMSServices
{
    public static class WmsServicesHelper
    {
        static Logger _logger = LogManager.CreateNullLogger();
        /// <summary>
        /// 将指定任务向Wms报完成
        /// </summary>
        /// <param name="task"></param>
        /// <param name="additionalInfo"></param>
        public static void CompleteWmsTask(Task task, Dictionary<String, String> additionalInfo = null)
        {
            WmsServices.WcsTaskInfo taskInfo = new WmsServices.WcsTaskInfo();

            taskInfo.TaskCode = task.TaskCode;
            taskInfo.ContainerCodes = task.ContainerCodes.ToArray();
            taskInfo.TaskType = task.TaskType;
            taskInfo.From = task.StartLocation.UserCode;
            taskInfo.To = task.EndLocation.UserCode;
            taskInfo.AdditionalInfo = new Dictionary<string, string>();
            if (task.AdditionalInfo != null)
            {
                foreach (var item in task.AdditionalInfo)
                {
                    taskInfo.AdditionalInfo.Add(item.Key, item.Value);
                }
            }

            if (task.Status == TaskStatus.Cancelled)
            {
                taskInfo.AdditionalInfo.Add("已取消", "true");
            }

            if (additionalInfo != null)
            {
                foreach (var item in additionalInfo)
                {
                    taskInfo.AdditionalInfo.Add(item.Key, item.Value);
                }
            }
            using (WmsServices.WcfWmsServiceForWcsClient client = new WmsServices.WcfWmsServiceForWcsClient())
            {
                try
                {
                    client.CompleteTask(taskInfo);
                    client.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error1(ex, typeof(WmsServicesHelper));

                    Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Add(
                               new Wcs.Framework.MessageBoard.Messages.TipMessage(
                                   Wcs.Framework.MessageBoard.MessageLevel.Error,
                                   "Wms接口",
                                   "向WMS报完成失败,详细信息：" + ex.Message,
                                   ""
                                   ));
                    if (client != null)
                        client.Abort();
                    throw ex;
                }
            }

            var msg = string.Format("向 Wms 通知 {0} 完成成功（附加属性：{1}）。",
                    task,
                    String.Join(",", taskInfo.AdditionalInfo.ToArray().Select(x => string.Format("{0}={1}", x.Key, x.Value)))
                    );

            _logger.Info(msg, typeof(WmsServicesHelper));

            Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Add(
                       new Wcs.Framework.MessageBoard.Messages.TipMessage(
                           Wcs.Framework.MessageBoard.MessageLevel.Info, "Wms接口", msg, ""
                           ));
        }
        
        /// <summary>
        /// 在指定位置请求Wms
        /// </summary>
        /// <param name="requestLocation"></param>
        /// <param name="containerCodes"></param>
        /// <param name="requestType"></param>
        /// <param name="additionalInfo"></param>
        public static void RequestWms(WmsServices.WcsRequestInfo requestInfo)
        {
            String msg = "";
            using (WmsServices.WcfWmsServiceForWcsClient client = new WmsServices.WcfWmsServiceForWcsClient())
            {
                try
                {
                    client.Request(requestInfo);

                    client.Close();
                }
                catch (Exception ex)
                {
                    if (client != null)
                        client.Abort();

                    Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Add(
                            new Wcs.Framework.MessageBoard.Messages.TipMessage(Wcs.Framework.MessageBoard.MessageLevel.Emergency,
                            "Wms接口",
                            String.Format("位置 {0} 条码 {1} 请求类型为 {2} 附加属性 {3}，向WMS请求失败", requestInfo.LocationUserCode, requestInfo.ContainerCodes == null ? "Null" : String.Join(",", requestInfo.ContainerCodes), requestInfo.RequestType, (requestInfo.AdditionalInfo == null || requestInfo.AdditionalInfo.Count() == 0)? "null":String.Join("/", requestInfo.AdditionalInfo.Select(x=>x.Key+"->"+x.Value).ToArray())),
                            null));

                    _logger.Error(string.Format("位置 {0} 向WMS请求失败，异常信息：{1}", requestInfo.LocationUserCode, ex.Message), typeof(WmsServicesHelper));

                    throw ex;
                }
            }

            msg = string.Format("在 {0} 向 Wms 请求成功（类型：{1}，条码：{2}，附加属性：{3}）。",
                requestInfo.LocationUserCode,
                requestInfo.RequestType,
                requestInfo.ContainerCodes == null ? "" : string.Join(",", requestInfo.ContainerCodes),
                (requestInfo.AdditionalInfo == null || requestInfo.AdditionalInfo.Count() == 0)? "null":String.Join("/", requestInfo.AdditionalInfo.Select(x=>x.Key+"->"+x.Value).ToArray())
                );

            _logger.Info1(msg, typeof(WmsServicesHelper));

            Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Add(
                        new Wcs.Framework.MessageBoard.Messages.TipMessage(
                            Wcs.Framework.MessageBoard.MessageLevel.Info, "Wms接口", msg, ""
                            )
                    );
        }
    }
}
