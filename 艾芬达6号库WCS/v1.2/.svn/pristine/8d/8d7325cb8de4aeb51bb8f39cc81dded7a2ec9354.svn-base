using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcsServiceForClient
{
    [ServiceContract]
    public interface IConveyorServiceForClient
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json)]   
        JsonOperationResult GetAlarms(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        JsonOperationResult GetHoldSignals(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        JsonOperationResult GetLocationStatus(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        JsonOperationResult GetLocationTasks(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        JsonOperationResult GetOccupys(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        JsonOperationResult GetTasks(String deviceName);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        OperationResult ClearTask(String deviceName, Int32 equipmentTaskId);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        OperationResult DeleteTask(String deviceName, Int32 equipmentTaskId);

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        OperationResult DeleteLocationTask(String deviceName,Int32 posNo, Int32 equipmentTaskId);
    }
}
