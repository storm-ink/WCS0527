using System;

using System.Collections.Generic;

using System.Web.Http;

using Newtonsoft.Json;

using Wcs.DefaultImplementCollection.AGV;



namespace ZHQXC

{

    /// <summary>

    /// AGV 调度系统回调 WCS（自托管 WebApi，端口见 settings：wcsWebApiAddressPort）。

    /// </summary>

    [RoutePrefix("API/Agv")]

    public class AgvApiController : ApiController

    {

        readonly AgvIntegrationApplicationService _agvIntegrationService = new AgvIntegrationApplicationService();



        /// <summary>

        /// AGV 上报任务状态。支持 GET/POST；参数可在 Query 或 POST JSON Body。

        /// </summary>

        [Route("ReportTaskStatus")]

        [Route("~/api/Agv/ReportTaskStatus")]

        [HttpGet]

        [HttpPost]

        public AgvApiResult ReportTaskStatus(

            [FromUri] string TaskId = null,

            [FromUri] string TaskOwner = null,

            [FromUri] int? TaskStatus = null,

            [FromUri] string AdditionalInfo = null,

            [FromBody] AgvReportTaskStatusDto body = null)

        {
            return _agvIntegrationService.ReportTaskStatus(TaskId, TaskOwner, TaskStatus, AdditionalInfo, body);
        }
    }



    public class AgvReportTaskStatusDto

    {

        [JsonProperty("TaskId")]

        public string TaskId { get; set; }



        [JsonProperty("TaskOwner")]

        public string TaskOwner { get; set; }



        [JsonProperty("TaskStatus")]

        public int TaskStatus { get; set; }



        [JsonProperty("AdditionalInfo")]

        public string AdditionalInfo { get; set; }

    }

}


