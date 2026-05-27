using Matedata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wcs;
using Wcs.DefaultImpls.Conveyor;
using Wcs.Framework;
using NHibernate.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeviceServer.Services.ConveyorDeviceService
{
    public class ConveyorService : DeviceService,IConveyorService
    {
        static object getStatusLocker = new object();
        static Dictionary<String, MethodInfo> _readStatusMethods = new Dictionary<string, MethodInfo>();
        static Dictionary<String, Type> _statusTypes = new Dictionary<string, Type>();

        public string GetStatus(string deviceName, string fullTypeName)
        {
            var conveyorDevice = DeviceConverter.ToDevice<ConveyorDevice>(deviceName);

            lock (getStatusLocker)
            {
                if (!_statusTypes.ContainsKey(fullTypeName))
                {
                    Type t = Type.GetType(fullTypeName);
                    if (t == null)
                    {
                        throw new Exception(string.Format("未找到类型 {0}", fullTypeName));
                    }

                    _statusTypes.Add(fullTypeName, t);
                }

                if (!_readStatusMethods.ContainsKey(fullTypeName))
                {
                    var mi = conveyorDevice.GetType().GetMethod("ReadStatus");

                    if (mi == null)
                    {
                        throw new Exception(String.Format("在类型 {0} 中未找到 ReadStatus 方法", conveyorDevice.GetType()));
                    }

                    mi = mi.MakeGenericMethod(_statusTypes[fullTypeName]);
                    _readStatusMethods.Add(fullTypeName, mi);
                }
            }

            var obj = _readStatusMethods[fullTypeName].Invoke(conveyorDevice,null);

            var js = new Newtonsoft.Json.JsonSerializerSettings();
            js.ContractResolver = new WriteAllPropertiesContractResolver();

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,js);
        }

        public void ClearTask(string deviceName, int equipmentTaskId,int? atDBBlockIndex)
        {
            var conveyorDevice = DeviceConverter.ToDevice<ConveyorDevice>(deviceName);

            var count = conveyorDevice.Tasks.Count(x => x.AssignmentID == equipmentTaskId);

            if (count == 0)
            {
                throw new InvalidOperationException(string.Format("未找到设备任务 {0}", equipmentTaskId));
            }

            if (count > 1 && atDBBlockIndex.GetValueOrDefault(0)==0)
            {
                throw new InvalidOperationException(string.Format("未找到设备任务 {0}", equipmentTaskId));
            }

            var task = conveyorDevice.Tasks.First(x => x.AssignmentID == equipmentTaskId);
            if(atDBBlockIndex.GetValueOrDefault(0)!=0 && task.AtPacketIndex!=atDBBlockIndex.Value)
            {
                throw new InvalidOperationException(string.Format("设备任务 {0} 实际存放位置为 {1}，而用户指定的位置是 {2}", equipmentTaskId, task.AtPacketIndex, atDBBlockIndex));
            }

            ApplyClearTaskCommand clearCmd = new ApplyClearTaskCommand(
                task.AssignmentID, task.RotingNo, task.StartMotorNo, 
                task.DestinationNo, Convert.ToUInt16(task.AtPacketIndex), 
                Convert.ToUInt16(Wcs.Framework.SerialNumberFactory.GenerateRandomValue(1,UInt16.MaxValue)));

            conveyorDevice.Write(clearCmd, (device, cmd) =>
            {
                return conveyorDevice.Tasks[cmd.Index - 1].AssignmentID != cmd.AssignmentID;
            });
        }

        public void DeleteTask(string deviceName, int equipmentTaskId, int? atDBBlockIndex)
        {
            var conveyorDevice = DeviceConverter.ToDevice<ConveyorDevice>(deviceName);

            var count = conveyorDevice.Tasks.Count(x => x.AssignmentID == equipmentTaskId);

            if (count == 0)
            {
                throw new InvalidOperationException(string.Format("未找到设备任务 {0}", equipmentTaskId));
            }

            if (count > 1 && atDBBlockIndex.GetValueOrDefault(0) == 0)
            {
                throw new InvalidOperationException(string.Format("未找到设备任务 {0}", equipmentTaskId));
            }

            var task = conveyorDevice.Tasks.First(x => x.AssignmentID == equipmentTaskId);
            if (atDBBlockIndex.GetValueOrDefault(0) != 0 && task.AtPacketIndex != atDBBlockIndex.Value)
            {
                throw new InvalidOperationException(string.Format("设备任务 {0} 实际存放位置为 {1}，而用户指定的位置是 {2}", equipmentTaskId, task.AtPacketIndex, atDBBlockIndex));
            }

            ApplyDeleteTaskCommand deleteCmd = new ApplyDeleteTaskCommand(
                task.AssignmentID, task.RotingNo, task.StartMotorNo,
                task.DestinationNo, Convert.ToUInt16(task.AtPacketIndex),
                Convert.ToUInt16(Wcs.Framework.SerialNumberFactory.GenerateRandomValue(1, UInt16.MaxValue)));

            conveyorDevice.Write(deleteCmd, (device, cmd) =>
            {
                return conveyorDevice.Tasks[cmd.Index - 1].AssignmentID != cmd.AssignmentID;
            });
        }

        public void ClearLocationTask(string deviceName, int posNo, int equipmentTaskId)
        {
            var conveyorDevice = DeviceConverter.ToDevice<ConveyorDevice>(deviceName);

            var count = conveyorDevice.LocationCurrentTasks.Count(x => x.PosNo == posNo && x.TaskNo==equipmentTaskId);

            if (count == 0)
            {
                throw new InvalidOperationException(string.Format("未找到货位任务 {0}", equipmentTaskId));
            }

            if (count > 1)
            {
                throw new InvalidOperationException(string.Format("货位任务 {0} 在设备中找到 {1} 个", equipmentTaskId,count));
            }

            var task = conveyorDevice.LocationCurrentTasks.First(x => x.PosNo == posNo && x.TaskNo==equipmentTaskId);

            ClearLocationTaskCommand clearCmd = new ClearLocationTaskCommand(
                task.PosNo,task.TaskNo,task.TUID,Convert.ToUInt16(task.AtPacketIndex),
                Convert.ToUInt16(Wcs.Framework.SerialNumberFactory.GenerateRandomValue(1, UInt16.MaxValue)));

            conveyorDevice.Write(clearCmd, (device, cmd) =>
            {
                return conveyorDevice.LocationCurrentTasks[cmd.Index - 1].TaskNo != cmd.TaskNo;
            });
        }

        public void AddTask(string deviceName, int equipmentTaskId, int? routeNo, string startLocationUserCode, string endLocationUserCode)
        {
            var conveyorDevice = DeviceConverter.ToDevice<ConveyorDevice>(deviceName);

            var startLocation = LocationConverter.UserCodeToLcation(startLocationUserCode);
            var endLocation = LocationConverter.UserCodeToLcation(endLocationUserCode);
            var paths = PathHelper.FindNextPath(null, startLocation, endLocation, startLocation, null, RouteType.Normal | RouteType.Counting);

            paths = paths.Where(x => x.Value.EndLocation.Equals(endLocation) && x.Value.Device == conveyorDevice)
                .ToDictionary(x => x.Key, x => x.Value);
            
            if (paths.Count == 0)
            {
                throw new NotSupportedException(string.Format("未找到从 {0} 到 {1} 的连通路径", startLocation, endLocation));
            }

            Int32 taskRouteNo;
            if (routeNo.GetValueOrDefault(0) != 0)
            {
                if (!paths.Any(x => x.Value.No == routeNo))
                {
                    throw new NotSupportedException(string.Format("从 {0} 到 {1} 的连通路径中不包含指定的路径号 {2}", startLocation, endLocation, routeNo));
                }

                taskRouteNo = routeNo.Value;
            }
            else
            {
                taskRouteNo = paths.First().Value.No;
            }

            var index = getTaskBlockIndex(conveyorDevice);

            AddTaskCommand addTaskCommand = new AddTaskCommand(
                Convert.ToUInt32(equipmentTaskId), Convert.ToUInt16(taskRouteNo),
                Convert.ToUInt16(startLocation.DeviceCode), Convert.ToUInt16(endLocation.DeviceCode),
                Convert.ToUInt16(index),
                 Convert.ToUInt16(Wcs.Framework.SerialNumberFactory.GenerateRandomValue(1, UInt16.MaxValue)));

            conveyorDevice.Write(addTaskCommand, addTaskCommand.SendSuccess);
        }

        /// <summary>
        /// 为指定的输送线子任务分配一个地址
        /// </summary>
        Int32 getTaskBlockIndex(ConveyorDevice conveyorDevice)
        {
            if (conveyorDevice.Tasks == null || conveyorDevice.Tasks.Length == 0)
            {
                throw new Exception(String.Format("分配任务地址失败，原因是 {0} 未连接或状态同步失败",  conveyorDevice));
            }

            List<int> usedIndexs;
            using (NHUnitOfWork newUnitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
            {
                usedIndexs = newUnitOfWork
                    .session
                    .Query<ConveyorTransferAction>()
                .Where(x => x.DeviceName == conveyorDevice.Name
                        && (x.Status == EquipmentActionStatus.New
                        || x.Status == EquipmentActionStatus.Executing
                        || x.Status == EquipmentActionStatus.Error
                        || x.Status == EquipmentActionStatus.Suspend))
                    .ToList()
                    .Where(x => x.AtPlcDBIndex.GetValueOrDefault(0) > 0)
                    .Select(x => x.AtPlcDBIndex.GetValueOrDefault(0))
                    .ToList();

                newUnitOfWork.Commit();

            }
            //可用的任务区大小
            var taskblockSize = conveyorDevice.AllowTaskBlockSpace;
            if (taskblockSize > conveyorDevice.Tasks.Length || taskblockSize <= 0)
            {
                taskblockSize = conveyorDevice.Tasks.Length;
            }

            //允许分配的索引列表
            int[] allIndexs = new int[taskblockSize];
            for (int i = 0; i < taskblockSize; i++)
            {
                allIndexs[i] = i + 1;
            }

            if (allIndexs.Except(usedIndexs).Count() == 0)
            {
                throw new InvalidOperationException(String.Format("{0} 任务块已被完全分配使用，当前无法分配新的任务地址", conveyorDevice));
            }

            for (UInt16 index = 1; index <= taskblockSize; index++)
            {
                //已被分配占用
                if (usedIndexs.Contains(index))
                {
                    continue;
                }

                if (conveyorDevice.Tasks == null || conveyorDevice.Tasks.Length < index)
                {
                    throw new Exception(String.Format("分配地址时输送线任务块大小发生变化，超出索引边界。index {1},length {2}", index, conveyorDevice.Tasks == null ? 0 : conveyorDevice.Tasks.Length));
                }

                if (conveyorDevice.Tasks[index - 1].HandShake == TaskNetTransferObjectHandShake.Empty)
                {
                    return index;
                }
            }

            throw new Exception(String.Format("{0} 任务块已被写满，当前无法分配任务地址", conveyorDevice));
        }

        public string GetDiagnoseMatedatas(int minDataId, int batchSize, string appInfo)
        {
            throw new NotImplementedException();
        }
    }
}
