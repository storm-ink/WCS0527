using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Linq;

using Wcs;

using Wcs.Framework;

using NHibernate.Linq;



namespace Wcs.DefaultImplementCollection.AGV

{

    [DisplayName("子系统接口")]

    public class SinevaAGVDevice : TcpProtocolTaskableDevice

    {

        public SinevaAGVDevice(string name, int no, int receiveTimeout, int connectTimeout, int sendTimeout, bool allowConcurrency, System.Net.IPEndPoint ipEndPoint, System.Net.IPEndPoint bindEndPoint, IDataReceiver dataReceiver)

            : base(name, no, receiveTimeout, connectTimeout, sendTimeout, allowConcurrency, ipEndPoint, bindEndPoint, dataReceiver)

        {

        }



        protected override void OnDataReceived(NetPacket netPacket, NetTransferObject netTransferObject)

        {

            throw new NotImplementedException();

        }



        public override int[] OccupiedEquipmentTasks

        {

            get { return new int[] { }; }

        }



        public override TState Read<TState>()

        {

            throw new NotImplementedException();

        }



        public override IDeviceUserInterface CreateUserInterface()

        {

            return new SinevaAGVUserInterfaceFrm();

        }



        public override IsIdleResult IsIdle

        {

            get

            {

                if (!SinevaAGVDatabaseHand.MiddleDatabaseState)

                    return new IsIdleResult(false, "");

                return new IsIdleResult(true, "");

            }

        }



        public override string[] Warnings

        {

            get

            {

                var warnings = new List<string>();

                if (!SinevaAGVDatabaseHand.MiddleDatabaseState)

                    warnings.Add("中间库连接失败");

                return warnings.ToArray();

            }

        }



        public override bool Connect()

        {

            return true;

        }



        public override bool IsConnected

        {

            get { return true; }

        }



        public override void Disconnect()

        {

        }



        public virtual void AcceptLocation(params AGVSubSystemLocation[] locations)

        {

            if (locations == null)

                throw new ArgumentNullException("locations");



            if (locations.Any(x => x.Device != null && x.Device != this))

                throw new InvalidOperationException("有已被分配给其它设备的位置对象，而这些对象并不允许修改所属设备。");



            var invalidLocations = locations.Where(x => !(x is ILocationWildcard)).Intersect(this.Locations.Where(x => !(x is ILocationWildcard)));

            if (invalidLocations.Any())

            {

                throw new InvalidOperationException(String.Format("位置 {0} 在 {1} 中已存在",

                    string.Join(",", invalidLocations.Select(x => x.ToString()).ToArray()), this));

            }



            invalidLocations = locations.Where(x => x is ILocationWildcard).Intersect(this.Locations.Where(x => x is ILocationWildcard));

            if (invalidLocations.Any())

            {

                throw new InvalidOperationException(String.Format("通配符位置 {0} 在 {1} 中已存在",

                    string.Join(",", invalidLocations.Select(x => x.ToString()).ToArray()), this));

            }

            _locations.AddRange(locations);

        }



        public override void Write<TData>(TData data, Func<TaskableDevice, TData, bool> isSuccess)

        {

        }



        /// <summary>

        /// 下发：写中间库 + HTTP 通知 AGV。

        /// </summary>

        public override void SendTask(EquipmentAction action, params string[] args)

        {

            var agvAction = (SinevaAGVSubSystemAction)action;

            var fromLocation = (AGVSubSystemLocation)LocationConverter.ToLocation(agvAction.LoadLocation);

            var toLocation = (AGVSubSystemLocation)LocationConverter.ToLocation(agvAction.UnloadLocation);



            var model = BuildMiddleDatabaseTask(action, agvAction, fromLocation, toLocation);

            if (!SinevaAGVDatabaseHand.InterTaskToAGV(model, action.DeviceName))

                throw new Exception("AGV 中间库下发任务失败");



            AgvHttpApiClient.DispatchTask(BuildHttpDispatchRequest(action, agvAction, fromLocation, toLocation));

        }



        static AGV_T_interface BuildMiddleDatabaseTask(

            EquipmentAction action,

            SinevaAGVSubSystemAction agvAction,

            AGVSubSystemLocation fromLocation,

            AGVSubSystemLocation toLocation)

        {

            var model = new AGV_T_interface

            {

                interCode = agvAction.SendAGVTaskId,

                begionLoc = Convert.ToString(fromLocation.StationNo),

                endLoc = Convert.ToString(toLocation.StationNo),

                createDatetime = DateTime.Now,

                sort = "0",

                interType = "3",

                begionLevel = fromLocation.Level,

                endLevel = toLocation.Level,

                state = "1",

                category = "1",

                SalverType = "1",

                TrayCode = action.ContainerCode.ToString()

            };

            if (action.Movement?.Task?.AdditionalInfo != null

                && action.Movement.Task.AdditionalInfo.ContainsKey("priority"))

            {

                model.sort = Convert.ToString(action.Movement.Task.AdditionalInfo["priority"]);

            }

            return model;

        }



        static AgvDispatchTaskRequest BuildHttpDispatchRequest(

            EquipmentAction action,

            SinevaAGVSubSystemAction agvAction,

            AGVSubSystemLocation fromLocation,

            AGVSubSystemLocation toLocation)

        {

            var additional = new Dictionary<String, String>();

            if (action.Movement?.Task?.AdditionalInfo != null)

            {

                foreach (var kv in action.Movement.Task.AdditionalInfo)

                    additional[kv.Key] = kv.Value?.ToString() ?? "";

            }



            return new AgvDispatchTaskRequest

            {

                TaskId = agvAction.SendAGVTaskId,

                TaskOwner = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("agvTaskOwner", "wcs"),

                ContainerCode = action.ContainerCode.ToString(),

                StartLocationCode = Convert.ToString(fromLocation.StationNo),

                EndLocationCode = Convert.ToString(toLocation.StationNo),

                AdditionalInfo = additional.Count > 0 ? additional : null

            };

        }



        /// <summary>

        /// AGV 调 WCS 完成接口：更新中间表状态 → WCS 结单 → 删除中间表记录。

        /// </summary>

        public virtual void HandleAgvTaskStatusReport(AgvReportTaskStatusRequest request)

        {

            if (request == null || String.IsNullOrWhiteSpace(request.TaskId))

                throw new ArgumentException("TaskId 不能为空");



            var expectedOwner = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<String>("agvTaskOwner", "wcs");

            if (!String.IsNullOrWhiteSpace(request.TaskOwner)

                && !String.Equals(request.TaskOwner, expectedOwner, StringComparison.OrdinalIgnoreCase)

                && !String.Equals(request.TaskOwner, "wcs", StringComparison.OrdinalIgnoreCase))

            {

                _logger.Warn1(String.Format("忽略非本系统任务 TaskOwner={0}, TaskId={1}", request.TaskOwner, request.TaskId), this);

                return;

            }



            SinevaAGVSubSystemAction agvAction;

            using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))

            {

                agvAction = unitOfWork.session.Query<SinevaAGVSubSystemAction>()

                    .FirstOrDefault(x => x.SendAGVTaskId == request.TaskId || request.TaskId.Contains(x.SendAGVTaskId));

                if (agvAction == null)

                    throw new Exception(String.Format("未找到 AGV 任务 {0}", request.TaskId));

                unitOfWork.Commit();

            }



            var interCode = agvAction.SendAGVTaskId;

            var equipmentTaskId = agvAction.EquipmentTaskId;

            var completedCode = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<int>("agvTaskStatusCompleted", AgvReportedTaskStatus.Completed);

            var cancelledCode = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<int>("agvTaskStatusCancelled", AgvReportedTaskStatus.Cancelled);

            var errorCode = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<int>("agvTaskStatusError", AgvReportedTaskStatus.Error);



            if (request.TaskStatus == completedCode)

            {

                if (!SinevaAGVDatabaseHand.UpdateInterfaceTaskState(interCode, EquipmentTaskStatus.已完成))

                    _logger.Warn1(String.Format("中间表未更新为已完成 intercode={0}", interCode), this);



                FireTaskCompletedEvent(new TaskCompletedEventArgs(equipmentTaskId));

                SinevaAGVDatabaseHand.DeleteTaskByInterCode(interCode);

                _logger.Info1(String.Format("AGV 任务完成并已删除中间表记录 intercode={0}", interCode), this);

            }

            else if (request.TaskStatus == cancelledCode || request.TaskStatus == errorCode)

            {

                SinevaAGVDatabaseHand.UpdateInterfaceTaskState(interCode, EquipmentTaskStatus.错误);

                var msg = request.AdditionalInfo != null && request.AdditionalInfo.ContainsKey("msg")

                    ? request.AdditionalInfo["msg"]

                    : "AGV 上报任务异常";

                FireTaskErrorEvent(new TaskErrorEventArgs(equipmentTaskId, request.TaskStatus.ToString(), msg));

                SinevaAGVDatabaseHand.DeleteTaskByInterCode(interCode);

            }

            else

            {

                _logger.Info1(String.Format("AGV 任务 {0} 状态 {1}，WCS 暂不处理", request.TaskId, request.TaskStatus), this);

            }

        }



        public override void SendTaskPre(EquipmentAction action)

        {

            throw new NotImplementedException();

        }



        public override void CancelTask(EquipmentAction action)

        {

            var agvAction = (SinevaAGVSubSystemAction)action;

            _logger.Warn1(String.Format("AGV 取消任务请通过设备管理界面或通知 AGV 系统，SendAGVTaskId={0}", agvAction.SendAGVTaskId), this);

        }

    }

}


