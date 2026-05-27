using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Wcs.DefaultImpls.Conveyor;

namespace WcsServiceForClient
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class ConveyorServiceForClient:IConveyorServiceForClient
    {
        public JsonOperationResult GetAlarms(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x=>x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var alarms = conveyorDevice
                    .MachineAlarms
                    .Select(x => new
                {
                    PosNo = x.PosNo,
                    Alarms = x.ToAlarms()
                });

                return new JsonOperationResult(true, alarms);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message,null);
            }
        }

        public JsonOperationResult GetHoldSignals(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x => x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var occupiedSignals = conveyorDevice
                    .OccupiedSignals
                    .Select(x => new
                    {
                        AssignmentID = x.AssignmentID,
                        AtPacketIndex = x.AtPacketIndex,
                        HandShake = Convert.ToInt32(x.HandShake),
                        IO_Data = x.IO_Data,
                        PosNo = x.PosNo,
                        TU_ID = x.TU_ID,
                        TU_Type = x.TU_Type
                    });

                return new JsonOperationResult(true, occupiedSignals);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public JsonOperationResult GetLocationStatus(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x => x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var status = conveyorDevice
                    .ConveyorLocationStates
                    .Select(x => new 
                    {
                        AtPacketIndex = x.AtPacketIndex,
                        PosNo = x.PosNo,
                        Status = Convert.ToInt32(x.Status)
                    });

                return new JsonOperationResult(true, status);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public JsonOperationResult GetLocationTasks(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x => x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var status = conveyorDevice
                    .LocationCurrentTasks
                    .Select(x => new
                    {
                        AtPacketIndex = x.AtPacketIndex,
                        Fnh_Rcv_X = x.Fnh_Rcv_X,
                        Fnh_Rcv_Y = x.Fnh_Rcv_Y,
                        PosNo = x.PosNo,
                        Rcv_Rdy = x.Rcv_Rdy,
                        Rqs_Snt = x.Rqs_Snt,
                        Str_Rcv_X = x.Str_Rcv_X,
                        Str_Rcv_Y = x.Str_Rcv_Y,
                        TaskNo = x.TaskNo,
                        TUID = x.TUID
                    });

                return new JsonOperationResult(true, status);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public JsonOperationResult GetOccupys(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x => x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var status = conveyorDevice
                    .OccupyStatus
                    .Select(x => new
                    {
                        AftPosPotocell =x.AftPosPotocell,
                        AftProPotocell = x.AftProPotocell,
                        AftSloPotocell = x.AftSloPotocell,
                        AtPacketIndex = x.AtPacketIndex,
                        DownPotocell = x.DownPotocell,
                        FroPosPotocell = x.FroPosPotocell,
                        FroProPotocell = x.FroProPotocell,
                        FroSloPotocell =x.FroSloPotocell,
                        PhocllUseStatus = x.PhocllUseStatus,
                        PosNo = x.PosNo,
                        UpPotocell = x.UpPotocell
                    });

                return new JsonOperationResult(true, status);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public JsonOperationResult GetTasks(string deviceName)
        {
            try
            {
                var conveyorDevice = Wcs.Framework.Cfg
                .WcsConfiguration
                .Instance
                .DeviceCollection
                .ParticularDeviceCollection
                .SelectMany(x => x.DeviceElements)
                .Where(x => x.Device is ConveyorDevice)
                .Select(x => x.Device as ConveyorDevice)
                .Single(x => x.Name.Equals(deviceName, StringComparison.CurrentCultureIgnoreCase));

                var status = conveyorDevice
                    .Tasks
                    .Select(x => new
                    {
                        AssignmentID  =x.AssignmentID,
                        AtPacketIndex = x.AtPacketIndex,
                        DestinationNo= x.DestinationNo,
                        HandShake= Convert.ToInt32(x.HandShake),
                        IO_Data = x.IO_Data,
                        ReadTask = x.ReadTask,
                        RotingNo = x.RotingNo,
                        Spare = x.Spare,
                        StartMotorNo=  x.StartMotorNo,
                        TaskStatus =Convert.ToInt32( x.TaskStatus),
                        TU_ID  =x.TU_ID,
                        TU_Type = x.TU_Type
                    });

                return new JsonOperationResult(true, status);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false, ex.Message, null);
            }
        }

        public OperationResult ClearTask(string deviceName, int equipmentTaskId)
        {
            throw new NotImplementedException();
        }

        public OperationResult DeleteTask(string deviceName, int equipmentTaskId)
        {
            throw new NotImplementedException();
        }

        public OperationResult DeleteLocationTask(string deviceName, int posNo, int equipmentTaskId)
        {
            throw new NotImplementedException();
        }
    }
}
