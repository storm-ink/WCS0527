using System;

using System.Collections.Generic;

using System.Web.Http;

using Newtonsoft.Json;

using NLog;

using Wcs;

using Wcs.DefaultImplementCollection.AGV;

using Wcs.Framework;



namespace ZHQXC

{

    /// <summary>

    /// AGV 调度系统回调 WCS（自托管 WebApi，端口见 settings：wcsWebApiAddressPort）。

    /// </summary>

    [RoutePrefix("API/Agv")]

    public class AgvApiController : ApiController

    {

        static readonly Logger _logger = LogManager.GetCurrentClassLogger();



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

            if (body != null)

            {

                if (String.IsNullOrWhiteSpace(TaskId))

                    TaskId = body.TaskId;

                if (String.IsNullOrWhiteSpace(TaskOwner))

                    TaskOwner = body.TaskOwner;

                if (!TaskStatus.HasValue)

                    TaskStatus = body.TaskStatus;

                if (String.IsNullOrWhiteSpace(AdditionalInfo))

                    AdditionalInfo = body.AdditionalInfo;

            }



            if (String.IsNullOrWhiteSpace(TaskId))

                return new AgvApiResult { Code = 400, Msg = "TaskId 不能为空" };

            if (!TaskStatus.HasValue)

                return new AgvApiResult { Code = 400, Msg = "TaskStatus 不能为空" };



            return ReportTaskStatusCore(TaskId, TaskOwner, TaskStatus.Value, AdditionalInfo);

        }



        AgvApiResult ReportTaskStatusCore(string taskId, string taskOwner, int taskStatus, string additionalInfo)

        {

            _logger.Info1($"AGV-->WCS ReportTaskStatus TaskId={taskId}, Owner={taskOwner}, Status={taskStatus}", this);



            try

            {

                var deviceName = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection

                    .GetSetting<string>("agvHttpApiDeviceName", "AGV调度系统");

                var device = DeviceConverter.ToDevice<SinevaAGVDevice>(deviceName);



                Dictionary<string, string> additional = null;

                if (!string.IsNullOrWhiteSpace(additionalInfo))

                {

                    try

                    {

                        additional = JsonConvert.DeserializeObject<Dictionary<string, string>>(additionalInfo);

                    }

                    catch

                    {

                        additional = new Dictionary<string, string> { ["raw"] = additionalInfo };

                    }

                }



                var request = new AgvReportTaskStatusRequest

                {

                    TaskId = taskId,

                    TaskOwner = taskOwner,

                    TaskStatus = taskStatus,

                    AdditionalInfo = additional

                };



                device.HandleAgvTaskStatusReport(request);



                return new AgvApiResult { Code = 200, Msg = "成功" };

            }

            catch (Exception ex)

            {

                _logger.Error1(ex, this);

                return new AgvApiResult { Code = 500, Msg = ex.Message };

            }

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


