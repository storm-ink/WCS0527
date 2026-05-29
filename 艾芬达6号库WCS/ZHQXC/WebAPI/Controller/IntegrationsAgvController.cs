using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using Wcs.DefaultImplementCollection.AGV;

namespace ZHQXC
{
    [RoutePrefix("api/v1/integrations/agv")]
    public class IntegrationsAgvController : ApiController
    {
        readonly AgvIntegrationApplicationService _agvIntegrationService = new AgvIntegrationApplicationService();

        [Route("task-status-report")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult ReportTaskStatus(
            [FromUri] string TaskId = null,
            [FromUri] string TaskOwner = null,
            [FromUri] int? TaskStatus = null,
            [FromUri] string AdditionalInfo = null,
            [FromBody] AgvReportTaskStatusDto body = null)
        {
            AgvApiResult reply = _agvIntegrationService.ReportTaskStatus(TaskId, TaskOwner, TaskStatus, AdditionalInfo, body);
            return BuildReply(reply);
        }

        IHttpActionResult BuildReply(AgvApiResult reply)
        {
            HttpStatusCode statusCode;
            UnifiedApiResponse response;

            if (reply == null)
            {
                statusCode = HttpStatusCode.InternalServerError;
                response = UnifiedApiResponse.Fail("agv_task_status_report_failed", "AGV 接口执行失败");
            }
            else if (reply.Code == 200)
            {
                statusCode = HttpStatusCode.OK;
                response = UnifiedApiResponse.Ok(reply, reply.Msg ?? "成功");
            }
            else
            {
                statusCode = reply.Code == 400 ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
                response = UnifiedApiResponse.Fail(reply.Code == 400 ? "agv_request_invalid" : "agv_task_status_report_failed", reply.Msg ?? "失败");
                response.Data = reply;
            }

            return Content(statusCode, response);
        }
    }
}
