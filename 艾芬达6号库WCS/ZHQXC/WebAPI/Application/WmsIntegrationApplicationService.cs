using Newtonsoft.Json;
using NHibernate.Linq;
using NLog;
using Sineva.WMS.Dto.WCSDto.ReplyDto;
using Sineva.WMS.Dto.WCSDto.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using Wcs;
using Wcs.DefaultImplementCollection.Conveyor;
using Wcs.Framework;
using Wcs.FrameworkExtend;

namespace ZHQXC
{
    internal class WmsIntegrationApplicationService
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static ThreadRunningLog WMSToWCSAPILogs
        {
            get
            {
                ThreadRunningLog log = new ThreadRunningLog();
                log.Init("WMSToWCSAPILogs");
                return log;
            }
        }

        public ReplyBase HandleEquipmentTaskRequest(AssignEquipTaskRequest request, string rawPayload = null)
        {
            ReplyBase replyBase = CreateReplyBase(request == null ? null : request.IctId);
            PreTask preTask;

            if (request == null)
            {
                return Fail(replyBase, "400", "请求体不可为空", "EquipmentTaskRequest", rawPayload);
            }

            try
            {
                LogReceived("EquipmentTaskRequest", rawPayload, request);
                ConvertToPreTask(request, out preTask);

                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    if (unitOfWork.session.Query<PreTask>().Any(x => x.TaskCode == preTask.TaskCode))
                    {
                        unitOfWork.Commit();
                        _logger.Info1(string.Format("WMS任务下发成功,任务已存在，接收数据：{0}", rawPayload ?? JsonConvert.SerializeObject(request)), this);
                        return Success(replyBase);
                    }

                    var additionalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(preTask.AdditionalInfo);
                    if (additionalInfo.ContainsKey("REQUEST")
                        && unitOfWork.session.Query<Task>().Any(x => x.AdditionalInfo.ContainsKey("REQUEST") && x.AdditionalInfo["REQUEST"] == preTask.TaskCode))
                    {
                        unitOfWork.Commit();
                        throw new ArgumentException(string.Format("请求ID {0} 重复", additionalInfo["REQUEST"]));
                    }

                    unitOfWork.session.Save(preTask);
                    unitOfWork.Commit();
                }

                Wcs.Framework.EventBus.EventBus.Instance.Publish<Wcs.FrameworkExtend.Events.PreTaskAddedEvent>(
                    new Wcs.FrameworkExtend.Events.PreTaskAddedEvent(preTask));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                return Fail(replyBase, "500", "操作执行异常（执行请求失败）", "EquipmentTaskRequest", rawPayload, ex);
            }

            _logger.Info1(string.Format("WMS任务下发成功，接收数据：{0}", rawPayload ?? JsonConvert.SerializeObject(request)), this);
            return Success(replyBase);
        }

        public ReplyBase HandleIssueNonCompliant(IssueNonCompliantRequest request, string rawPayload = null)
        {
            ReplyBase replyBase = CreateReplyBase(request == null ? null : request.IctId);
            if (request == null)
            {
                return Fail(replyBase, "400", "请求体不可为空", "issue_non_compliant", rawPayload);
            }

            try
            {
                LogReceived("issue_non_compliant", rawPayload, request);

                if (request.Data.LocationCode == "00-001-1023"
                    || request.Data.LocationCode == "00-001-1024"
                    || request.Data.LocationCode == "00-001-1025"
                    || request.Data.LocationCode == "00-001-1026"
                    || request.Data.LocationCode == "00-001-1027"
                    || request.Data.LocationCode == "00-001-1028")
                {
                    ExecuteDePalletizingCommand executeDePalletizingCommand = new ExecuteDePalletizingCommand()
                    {
                        PosNo = Convert.ToUInt16(request.Data.LocationCode.Substring(request.Data.LocationCode.Length - 4)),
                        HandShake = 1,
                        Room = Convert.ToUInt16(request.Data.Chamber),
                        StartLayer = Convert.ToUInt16(request.Data.StartLayer),
                        EndLayer = Convert.ToUInt16(request.Data.EndLayer),
                        IsNG = Convert.ToUInt16(request.Data.IsNG == 0 ? 1 : 2),
                        IsOver = Convert.ToUInt16(request.Data.IsFinish == "true" ? 2 : 1),
                        TaskNo = request.Data.TaskId,
                        Box = request.Data.BoxCode
                    };

                    string conveyorName = ResolveSpecialConveyorName(request.Data.LocationCode);
                    ConveyorDevice conveyor = DeviceConverter.ToDevice<ConveyorDevice>(conveyorName);

                    try
                    {
                        conveyor.Write<ExecuteDePalletizingCommand>(executeDePalletizingCommand, executeDePalletizingCommand.SendSuccess);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    replyBase.IctResult = false;
                    replyBase.IctCode = "400";
                    replyBase.IctMsg = string.Format("WMS下发拣选的货位为空，下发信息为{0} ", JsonConvert.SerializeObject(request));
                    replyBase.IctDatetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    replyBase.Data = "";
                    WMSToWCSAPILogs.Log(string.Format("WMS-->WCS WMS调用WCS issue_non_compliant 接口WCS回复：{0}，WMS传入数据异常,货位信息为空", JsonConvert.SerializeObject(replyBase)));
                    return replyBase;
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                return Fail(replyBase, "500", "操作执行异常（执行请求失败）", "issue_non_compliant", rawPayload, ex);
            }

            _logger.Info1(string.Format("WMS任务下发成功，接收数据：{0}", rawPayload ?? JsonConvert.SerializeObject(request)), this);
            return Success(replyBase);
        }

        public ReplyBase HandleBoxArriveInfo(BoxArriveInfoReportReply request, string rawPayload = null)
        {
            ReplyBase replyBase = CreateReplyBase(request == null ? null : request.IctId);
            if (request == null)
            {
                return Fail(replyBase, "400", "请求体不可为空", "box_arrive_info", rawPayload);
            }

            try
            {
                LogReceived("box_arrive_info", rawPayload, request);

                BoxInfomationCommand boxInfomationCommand = new BoxInfomationCommand()
                {
                    PosNo = Convert.ToUInt16(request.Data.LocationCode.Substring(request.Data.LocationCode.Length - 4)),
                    Room1Floot = Convert.ToUInt16(request.Data.Chamber1Layer),
                    Room2Floot = Convert.ToUInt16(request.Data.Chamber2Layer),
                    Room3Floot = Convert.ToUInt16(request.Data.Chamber3Layer),
                    Room4Floot = Convert.ToUInt16(request.Data.Chamber4Layer),
                    Box = request.Data.Boxcode,
                    DataID = 34
                };

                string conveyorName = ResolveSpecialConveyorName(request.Data.LocationCode);
                if (string.IsNullOrWhiteSpace(conveyorName))
                {
                    replyBase.IctResult = false;
                    replyBase.IctCode = "400";
                    replyBase.IctMsg = string.Format("WMS下发拣选的货位为空，下发信息为{0} ", JsonConvert.SerializeObject(request));
                    replyBase.IctDatetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    replyBase.Data = "";
                    WMSToWCSAPILogs.Log(string.Format("WMS-->WCS WMS调用WCS box_arrive_info 接口WCS回复：{0}，WMS传入数据异常,货位信息为空", JsonConvert.SerializeObject(replyBase)));
                    return replyBase;
                }

                ConveyorDevice conveyor = DeviceConverter.ToDevice<ConveyorDevice>(conveyorName);
                try
                {
                    conveyor.Write<BoxInfomationCommand>(boxInfomationCommand, boxInfomationCommand.SendSuccess);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                return Fail(replyBase, "500", "操作执行异常（执行请求失败）", "box_arrive_info", rawPayload, ex);
            }

            _logger.Info1(string.Format("WMS任务下发成功，接收数据：{0}", rawPayload ?? JsonConvert.SerializeObject(request)), this);
            return Success(replyBase);
        }

        static string ResolveSpecialConveyorName(string locationCode)
        {
            if (locationCode == "00-001-1023" || locationCode == "00-001-1024")
            {
                return "专机CV(A)";
            }

            if (locationCode == "00-001-1025" || locationCode == "00-001-1026")
            {
                return "专机CV(B)";
            }

            if (locationCode == "00-001-1027" || locationCode == "00-001-1028")
            {
                return "专机CV(C)";
            }

            return null;
        }

        static ReplyBase CreateReplyBase(string ictId)
        {
            return new ReplyBase
            {
                IctId = ictId
            };
        }

        static ReplyBase Success(ReplyBase replyBase)
        {
            replyBase.IctDatetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            replyBase.IctMsg = "成功";
            replyBase.IctResult = true;
            WMSToWCSAPILogs.Log(string.Format("WMS-->WCS WCS回复：{0}", JsonConvert.SerializeObject(replyBase)));
            return replyBase;
        }

        static ReplyBase Fail(ReplyBase replyBase, string code, string message, string actionName, string rawPayload, Exception ex = null)
        {
            replyBase.IctResult = false;
            replyBase.IctCode = code;
            replyBase.IctMsg = message;
            replyBase.IctDatetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            replyBase.Data = ex == null ? message : ex.Message;
            WMSToWCSAPILogs.Log(string.Format("WMS-->WCS WMS调用WCS {0} 接口WCS回复：{1}，异常消息：{2}", actionName, JsonConvert.SerializeObject(replyBase), ex ?? (object)message));
            return replyBase;
        }

        static void LogReceived(string actionName, string rawPayload, object request)
        {
            WMSToWCSAPILogs.Log(string.Format("WMS-->WCS 收到WMS调用 {0} 接口，接收数据：{1}", actionName, rawPayload ?? JsonConvert.SerializeObject(request)));
        }

        static bool ConvertToPreTask(AssignEquipTaskRequest assignEquipTaskRequest, out PreTask preTask)
        {
            string taskCode = assignEquipTaskRequest.Data.TaskId;

            if (string.IsNullOrWhiteSpace(assignEquipTaskRequest.Data.Site.Source))
            {
                throw new InvalidOperationException("起点字段 Source 值无效，不可为空！");
            }

            IEnumerable<Location> starts = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .LocationCollection
                .Locations
                .Where(x => x.UserCode == assignEquipTaskRequest.Data.Site.Source);

            if (starts == null)
            {
                throw new InvalidOperationException(string.Format("起点字段 Source 值 {0} 无效", assignEquipTaskRequest.Data.Site.Source));
            }

            if (starts.Count() > 1)
            {
                throw new InvalidOperationException(string.Format("起点字段 Source 值 {0} 无效，匹配到多个位置{1}", assignEquipTaskRequest.Data.Site.Source, string.Join(",", starts.Select(x => x.UserCode))));
            }

            Location start = starts.First();

            if (string.IsNullOrWhiteSpace(assignEquipTaskRequest.Data.Site.Destination))
            {
                throw new InvalidOperationException("终点字段 Destination 值无效，不可为空！");
            }

            IEnumerable<Location> ends = Wcs.Framework.Cfg.WcsConfiguration
                .Instance
                .LocationCollection
                .Locations
                .Where(x => x.UserCode == assignEquipTaskRequest.Data.Site.Destination);

            if (ends == null)
            {
                throw new InvalidOperationException(string.Format("终点字段 Destination 值 {0} 无效", assignEquipTaskRequest.Data.Site.Destination));
            }

            if (ends.Count() > 1)
            {
                throw new InvalidOperationException(string.Format("终点字段 Destination 值 {0} 无效，匹配到多个位置{1}", assignEquipTaskRequest.Data.Site.Destination, string.Join(",", ends.Select(x => x.UserCode))));
            }

            Location end = ends.First();
            bool ableArrived = RouteHelper.AbleArrived(start, end);
            if (!ableArrived)
            {
                throw new InvalidOperationException(string.Format("{0} 到 {1} 无法连通", start, end));
            }

            string taskType = assignEquipTaskRequest.Data.TaskMode;
            int priority = assignEquipTaskRequest.Data.Priority;
            preTask = new PreTask(taskCode, LocationConverter.ToLocationInfo(start), LocationConverter.ToLocationInfo(end));
            preTask.TaskType = taskType;
            preTask.Priority = priority;

            if (!string.IsNullOrWhiteSpace(assignEquipTaskRequest.Data.PalletCode))
            {
                preTask.ContainerCodes = JsonConvert.SerializeObject(new List<string> { assignEquipTaskRequest.Data.PalletCode });
            }

            preTask.Source = TaskSource.Wms;
            Dictionary<string, string> additionalAttr = new Dictionary<string, string>();
            if (assignEquipTaskRequest.Data.AdditionalAttr != null && assignEquipTaskRequest.Data.AdditionalAttr.Count > 0)
            {
                additionalAttr = assignEquipTaskRequest.Data.AdditionalAttr;
            }

            if (!string.IsNullOrWhiteSpace(assignEquipTaskRequest.Data.RequestId))
            {
                additionalAttr.Add("REQUEST", assignEquipTaskRequest.Data.RequestId);
            }

            preTask.AdditionalInfo = JsonConvert.SerializeObject(additionalAttr);
            return true;
        }
    }
}
