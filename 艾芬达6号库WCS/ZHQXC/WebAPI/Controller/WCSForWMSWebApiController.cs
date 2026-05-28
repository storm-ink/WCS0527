using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Sineva.WMS.Dto.WCSDto.ReplyDto;
using Sineva.WMS.Dto.WCSDto.RequestDto;
using System;
using System.Web.Http;
using Wcs;
using ZHQXC;

namespace ZHQXC.WebAPI.Controller
{
    [RoutePrefix("API/WCSForWMSWebApi")]
    public class WCSForWMSWebApiController : ApiController
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        readonly WmsIntegrationApplicationService _wmsIntegrationService = new WmsIntegrationApplicationService();

        static ThreadRunningLog WMSToWCSAPILogs
        {
            get
            {
                ThreadRunningLog log = new ThreadRunningLog();
                log.Init("WMSToWCSAPILogs");
                return log;
            }
        }

        //-------------------------------------------------------立库---------------------------------------------------------------------------------------------------------------》》》

        /// <summary>
        /// 3.WMS任务下发
        /// </summary>
        /// <returns></returns>
        [Route("EquipmentTaskRequest")]
        [HttpPost]
        public ReplyBase EquipmentTaskRequest([FromBody] JObject msg)
        {
            string rawPayload = msg == null ? null : msg.ToString();
            try
            {
                AssignEquipTaskRequest request = msg == null ? null : JsonConvert.DeserializeObject<AssignEquipTaskRequest>(rawPayload);
                return _wmsIntegrationService.HandleEquipmentTaskRequest(request, rawPayload);
            }
            catch (Exception ex)
            {
                return BuildParseFailedReply(ex, "EquipmentTaskRequest", rawPayload);
            }
        }
       
        //-------------------------------------------------------下料口---------------------------------------------------------------------------------------------------------------》》》
        [Route("issue_non_compliant")]
        [HttpPost]
        public ReplyBase issue_non_compliant([FromBody] JObject msg)
        {
            string rawPayload = msg == null ? null : msg.ToString();
            try
            {
                IssueNonCompliantRequest request = msg == null ? null : JsonConvert.DeserializeObject<IssueNonCompliantRequest>(rawPayload);
                return _wmsIntegrationService.HandleIssueNonCompliant(request, rawPayload);
            }
            catch (Exception ex)
            {
                return BuildParseFailedReply(ex, "issue_non_compliant", rawPayload);
            }
        }


        [Route("box_arrive_info")]
        [HttpPost]
        public ReplyBase box_arrive_info([FromBody] JObject msg)
        {
            string rawPayload = msg == null ? null : msg.ToString();
            try
            {
                BoxArriveInfoReportReply request = msg == null ? null : JsonConvert.DeserializeObject<BoxArriveInfoReportReply>(rawPayload);
                return _wmsIntegrationService.HandleBoxArriveInfo(request, rawPayload);
            }
            catch (Exception ex)
            {
                return BuildParseFailedReply(ex, "box_arrive_info", rawPayload);
            }
        }

        ReplyBase BuildParseFailedReply(Exception ex, string actionName, string rawPayload)
        {
            _logger.Error1(ex, this);
            ReplyBase replyBase = new ReplyBase
            {
                IctResult = false,
                IctCode = "400",
                IctMsg = ex.Message,
                IctDatetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Data = ex.Message
            };

            WMSToWCSAPILogs.Log(string.Format("WMS-->WCS WMS调用WCS {0} 接口WCS回复：{1}，WMS传入数据解析发生异常,异常消息：\r\n {2}", actionName, JsonConvert.SerializeObject(replyBase), ex));
            return replyBase;
        }
    }
}
