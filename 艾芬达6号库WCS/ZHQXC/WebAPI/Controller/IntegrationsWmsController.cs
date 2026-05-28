using Newtonsoft.Json;
using Sineva.WMS.Dto.WCSDto.ReplyDto;
using Sineva.WMS.Dto.WCSDto.RequestDto;
using System.Net;
using System.Web.Http;

namespace ZHQXC
{
    [RoutePrefix("api/v1/integrations/wms")]
    public class IntegrationsWmsController : ApiController
    {
        readonly WmsIntegrationApplicationService _wmsIntegrationService = new WmsIntegrationApplicationService();

        [Route("equipment-task-request")]
        [HttpPost]
        public IHttpActionResult EquipmentTaskRequest([FromBody] AssignEquipTaskRequest request)
        {
            ReplyBase reply = _wmsIntegrationService.HandleEquipmentTaskRequest(
                request,
                request == null ? null : JsonConvert.SerializeObject(request));
            return BuildReply(reply, "wms_request_invalid", "wms_request_rejected");
        }

        [Route("issue-non-compliant")]
        [HttpPost]
        public IHttpActionResult IssueNonCompliant([FromBody] IssueNonCompliantRequest request)
        {
            ReplyBase reply = _wmsIntegrationService.HandleIssueNonCompliant(
                request,
                request == null ? null : JsonConvert.SerializeObject(request));
            return BuildReply(reply, "wms_request_invalid", "wms_request_rejected");
        }

        [Route("box-arrive-info")]
        [HttpPost]
        public IHttpActionResult BoxArriveInfo([FromBody] BoxArriveInfoReportReply request)
        {
            ReplyBase reply = _wmsIntegrationService.HandleBoxArriveInfo(
                request,
                request == null ? null : JsonConvert.SerializeObject(request));
            return BuildReply(reply, "wms_request_invalid", "wms_request_rejected");
        }

        IHttpActionResult BuildReply(ReplyBase reply, string invalidCode, string failedCode)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            UnifiedApiResponse response;

            if (reply == null)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response = UnifiedApiResponse.Fail(failedCode, "WMS 接口执行失败");
                return Content(statusCode, response);
            }

            if (reply.IctResult)
            {
                response = UnifiedApiResponse.Ok(reply, string.IsNullOrWhiteSpace(reply.IctMsg) ? "成功" : reply.IctMsg);
            }
            else
            {
                statusCode = reply.IctCode == "400" ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
                response = UnifiedApiResponse.Fail(reply.IctCode == "400" ? invalidCode : failedCode, reply.IctMsg ?? "失败");
                response.Data = reply;
            }

            return Content(statusCode, response);
        }
    }
}
