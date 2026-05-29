using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using Wcs;
using Wcs.DefaultImplementCollection.AGV;
using Wcs.Framework;

namespace ZHQXC
{
    internal class AgvIntegrationApplicationService
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AgvApiResult ReportTaskStatus(string taskId, string taskOwner, int? taskStatus, string additionalInfo, AgvReportTaskStatusDto body)
        {
            if (body != null)
            {
                if (string.IsNullOrWhiteSpace(taskId))
                {
                    taskId = body.TaskId;
                }

                if (string.IsNullOrWhiteSpace(taskOwner))
                {
                    taskOwner = body.TaskOwner;
                }

                if (!taskStatus.HasValue)
                {
                    taskStatus = body.TaskStatus;
                }

                if (string.IsNullOrWhiteSpace(additionalInfo))
                {
                    additionalInfo = body.AdditionalInfo;
                }
            }

            return ReportTaskStatus(taskId, taskOwner, taskStatus, additionalInfo);
        }

        public AgvApiResult ReportTaskStatus(string taskId, string taskOwner, int? taskStatus, string additionalInfo)
        {
            if (string.IsNullOrWhiteSpace(taskId))
            {
                return new AgvApiResult { Code = 400, Msg = "TaskId 不能为空" };
            }

            if (!taskStatus.HasValue)
            {
                return new AgvApiResult { Code = 400, Msg = "TaskStatus 不能为空" };
            }

            return ReportTaskStatusCore(taskId, taskOwner, taskStatus.Value, additionalInfo);
        }

        public AgvApiResult ReportTaskStatus(AgvReportTaskStatusDto body)
        {
            if (body == null)
            {
                return new AgvApiResult { Code = 400, Msg = "请求体不可为空" };
            }

            return ReportTaskStatus(body.TaskId, body.TaskOwner, body.TaskStatus, body.AdditionalInfo);
        }

        AgvApiResult ReportTaskStatusCore(string taskId, string taskOwner, int taskStatus, string additionalInfo)
        {
            _logger.Info1(string.Format("AGV-->WCS ReportTaskStatus TaskId={0}, Owner={1}, Status={2}", taskId, taskOwner, taskStatus), this);

            try
            {
                string deviceName = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection
                    .GetSetting<string>("agvHttpApiDeviceName", "AGV调度系统");

                SinevaAGVDevice device = DeviceConverter.ToDevice<SinevaAGVDevice>(deviceName);
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

                AgvReportTaskStatusRequest request = new AgvReportTaskStatusRequest
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
}
