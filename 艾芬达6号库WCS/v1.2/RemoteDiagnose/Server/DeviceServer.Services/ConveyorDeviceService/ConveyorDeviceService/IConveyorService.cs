using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace DeviceServer.Services.ConveyorDeviceService
{
    [ServiceContract]
    public interface IConveyorService:Matedata.IMatedataService
    {
        [OperationContract]
        String GetStatus(String deviceName,String fullTypeName);

        [OperationContract]
        void ClearTask(String deviceName, Int32 equipmentTaskId, int? atDBBlockIndex);

        [OperationContract]
        void DeleteTask(String deviceName, Int32 equipmentTaskId, int? atDBBlockIndex);

        [OperationContract]
        void ClearLocationTask(String deviceName, Int32 posNo, Int32 equipmentTaskId);

        [OperationContract]
        void AddTask(String deviceName, Int32 equipmentTaskId, Int32? routeNo, String startLocationUserCode, String endLocationUserCode);
    }
}
