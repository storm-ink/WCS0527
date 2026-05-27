using System;

using System.Collections.Generic;

using Newtonsoft.Json;



namespace Wcs.DefaultImplementCollection.AGV

{

    /// <summary>

    /// AGV HTTP 接口通用响应（code=200 表示成功）。

    /// </summary>

    public class AgvApiResult

    {

        [JsonProperty("msg")]

        public string Msg { get; set; }



        [JsonProperty("code")]

        public int Code { get; set; }



        [JsonIgnore]

        public bool IsSuccess => Code == 200;

    }



    /// <summary>

    /// AGV 上报的任务状态（与 AGV 厂商约定后可改 settings 映射）。

    /// </summary>

    public static class AgvReportedTaskStatus

    {

        public const int Running = 100;

        public const int Completed = 200;

        public const int Cancelled = 300;

        public const int Error = 400;

    }



    /// <summary>

    /// WCS 调用 AGV「下发任务」接口参数。

    /// </summary>

    public class AgvDispatchTaskRequest

    {

        public string TaskId { get; set; }

        public string TaskOwner { get; set; } = "wcs";

        public string ContainerCode { get; set; }

        public string StartLocationCode { get; set; }

        public string EndLocationCode { get; set; }

        public Dictionary<string, string> AdditionalInfo { get; set; }

    }



    /// <summary>

    /// AGV 调用 WCS「上报任务状态」参数。

    /// </summary>

    public class AgvReportTaskStatusRequest

    {

        public string TaskId { get; set; }

        public string TaskOwner { get; set; }

        public int TaskStatus { get; set; }

        public Dictionary<string, string> AdditionalInfo { get; set; }

    }

}


