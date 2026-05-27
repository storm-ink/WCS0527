using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Wcs;
using Wcs.Framework;
using NHibernate.Linq;

namespace WcsServiceForWms
{
    public class WcfWcsServiceForWms : IWcfWcsServiceForWms
    {
        /// <summary>
        /// 优先级常量
        /// </summary>
        const String _priority = "priority";
        Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        public bool SendTask(DC2.WcsTaskInfo taskInfo)
        {
            try
            {
                _logger.Info(string.Format("收到来自于 {0} 服务的 SendTask 调用，任务号 {1}", this, taskInfo.TaskCode), this, taskInfo);

                Task task = ConvertToTask(taskInfo);

                RouteType findOptions = RouteType.Normal;
                if (task.BizType == TaskBizType.Counting)
                {
                    findOptions = RouteType.Counting;
                }

                var startLocation = LocationConverter.UserCodeToLcation(taskInfo.From);
                var endLocation = LocationConverter.UserCodeToLcation(taskInfo.To);

                var path = RouteHelper.AbleArrived(startLocation, endLocation);
                if (!path)
                {
                    throw new Exception(String.Format("{0} {1} 到 {2} 无法连通", task.BizType.GetDescription(), task.StartLocation.UserCode, task.EndLocation.UserCode));
                }
                List<Wcs.Framework.EventBus.IEvent> events = new List<Wcs.Framework.EventBus.IEvent>();

                Task _task;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    _task = unitOfWork.session.Query<Task>().FirstOrDefault(x => x.TaskCode == task.TaskCode);
                    unitOfWork.Commit();
                }

                if (_task != null)
                    _logger.Info(string.Format("收到来自于 {0} 服务的 SendTask 变更任务调用，任务号 {1}，原任务信息({2})，变更后任务信息({3})", this, taskInfo.TaskCode, GetTaskInfo(_task), GetTaskInfo(task)), this, taskInfo);
                else
                    _logger.Info(string.Format("收到来自于 {0} 服务的 SendTask 新增任务调用，任务号 {1}，任务信息({2})", this, taskInfo.TaskCode, GetTaskInfo(task)), this, taskInfo);

                using (NHUnitOfWork unitOfWork = new NHUnitOfWork())
                {
                    if (!string.IsNullOrWhiteSpace(taskInfo.RequestId))
                    {
                        int requestId;
                        if (int.TryParse(taskInfo.RequestId, out requestId))
                        {
                            var request = unitOfWork.session.Get<Request>(requestId);
                            if (request != null)
                            {
                                request.Status = RequestStatus.Processed;
                                task.FromRequest = request;
                                events.Add(new Wcs.Framework.Events.RequestStatusChangedEvent(request.Id, request.Status));

                                unitOfWork.session.Delete(request);

                                events.Add(new Wcs.Framework.Events.RequestArchivedEvent(request.Id, request.Source, request.Status));
                            }
                        }
                    }
                    if (_task != null)
                    {
                        task.Id = _task.Id;
                        task.CreatedAt = _task.CreatedAt;

                        LogicMovement[] movements = task.Movements.Where(x => x.Status != LogicMovementStatus.Cancelled && x.Status != LogicMovementStatus.Completed).ToArray();
                        foreach (var movement in movements)
                        {
                            movement.Status = LogicMovementStatus.Cancelled;
                            foreach (var action in movement.EquipmentActions)
                            {
                                action.Status = EquipmentActionStatus.Cancelled;
                            }
                        }
                        task.AdditionalInfo.Add(DateTime.Now.ToString("HH:MM:ss.ffff"), "变更任务");

                        unitOfWork.session.SaveOrUpdate(task);
                        events.Add(new Wcs.Framework.Events.TaskUpdateEvent(_task, task));
                    }
                    else
                    {
                        unitOfWork.session.Save(task);
                        events.Add(new Wcs.Framework.Events.TaskAddedEvent(task));
                        _logger.Info(string.Format("收到来自于 {0} 服务的 SendTask 调用，任务号 {1}", this, taskInfo.TaskCode, DateTime.Now), this, taskInfo);
                    }

                    unitOfWork.Commit();
                }

                //从分布式事务中分离
                System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
                {
                    Wcs.Framework.EventBus.EventBus.Instance.Publish(events.ToArray());
                });

                if (_task != null)
                    _logger.Info(string.Format("收到来自于 {0} 服务的 SendTask 变更任务调用，变更任务 {1}", this, taskInfo.TaskCode), this, taskInfo);
                else
                    _logger.Info1(string.Format("收到来自于 {0} 服务的 SendTask 新增任务调用，添加任务 {1}", this, task), this, taskInfo);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this, taskInfo);
                throw ex;
            }
        }

        public Task ConvertToTask(DC2.WcsTaskInfo taskInfo)
        {
            Location startLocation = LocationConverter.UserCodeToLcation(taskInfo.From);
            Location endLocation = LocationConverter.UserCodeToLcation(taskInfo.To);

            if (startLocation == null)
            {
                throw new Exception(String.Format("无法识别的货位编码 {0}", taskInfo.From));
            }

            if (endLocation == null)
            {
                throw new Exception(String.Format("无法识别的货位编码 {0}", taskInfo.To));
            }

            Task task = new Task(taskInfo.TaskCode, new LocationInfo(startLocation.Device.Name, startLocation.DeviceCode, startLocation.UserCode), new LocationInfo(endLocation.Device.Name, endLocation.DeviceCode, endLocation.UserCode));
            task.ContainerCodes.AddAll(taskInfo.ContainerCodes);
            task.Source = TaskSource.Wms;
            task.TaskType = taskInfo.TaskType;

            if (taskInfo.AdditionalInfo != null)
            {
                ///附加属性处理
                foreach (var item in taskInfo.AdditionalInfo)
                {
                    ///优先级处理
                    Int32 _pri;
                    if (item.Key == _priority && Int32.TryParse(item.Value, out _pri))
                        task.Priority = _pri;

                    task.AdditionalInfo.Add(item);
                }
            }

            if (OutTaskTypes.Contains(taskInfo.TaskType))
            {
                task.Direction = TaskDirection.Out;
                if (ScalTaskTypes.Contains(taskInfo.TaskType))
                    task.BizType = TaskBizType.Counting;
                else
                    task.BizType = TaskBizType.Normal;
            }
            else if (InTaskTypes.Contains(taskInfo.TaskType))
            {
                task.Direction = TaskDirection.In;
                if (ScalTaskTypes.Contains(taskInfo.TaskType))
                    task.BizType = TaskBizType.Counting;
                else
                    task.BizType = TaskBizType.Normal;
            }
            else
            {
                task.Direction = TaskDirection.Unknow;
                task.BizType = TaskBizType.Normal;
            }

            return task;
        }

        string GetTaskInfo(Task task)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("任务号：" + task.TaskCode);
            sb.Append(",任务类型：" + task.TaskType);
            sb.Append(",容器编码：" + String.Join("/", task.ContainerCodes));
            sb.Append(",起点：" + task.StartLocation.UserCode);
            sb.Append(",终点" + task.EndLocation.UserCode);

            return sb.ToString();
        }

        //public DC2.PathStatus[] FindPathStatus(string startLocation, string[] endLocations)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 出库出库任务类型
        /// </summary>
        string[] OutTaskTypes
        {
            get
            {
                List<String> _taskTypes = new List<String>();
                var _taskType = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("出库任务类型", "");
                if (!String.IsNullOrWhiteSpace(_taskType))
                    _taskTypes = _taskType.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                return _taskTypes.ToArray();
            }
        }

        /// <summary>
        /// 入库任务类型
        /// </summary>
        String[] InTaskTypes
        {
            get
            {
                List<String> _taskTypes = new List<String>();
                var _taskType = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("入库任务类型", "");
                if (!String.IsNullOrWhiteSpace(_taskType))
                    _taskTypes = _taskType.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                return _taskTypes.ToArray();
            }
        }

        /// <summary>
        /// 盘点任务类型
        /// </summary>
        String[] ScalTaskTypes
        {
            get
            {
                List<String> _taskTypes = new List<String>();
                var _taskType = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("盘点任务类型", "");
                if (!String.IsNullOrWhiteSpace(_taskType))
                    _taskTypes = _taskType.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                return _taskTypes.ToArray();
            }
        }
    }
}
