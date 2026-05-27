using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WcsServiceForClient
{
    [ServiceContract]
    public interface IDeviceServiceForClient
    {
        /// <summary>
        /// 获取指定设备的状态信息
        /// </summary>
        /// <param name="deviceName">设备名</param>
        /// <returns></returns>
        [OperationContract]
        JsonOperationResult GetStatusByName(String deviceName);

        /// <summary>
        /// 获取所有设备的状态信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        JsonOperationResult GetAllStatus();
    }
}
