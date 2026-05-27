using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcsServiceForClient
{
    [ServiceContract()]
    public interface ICraneServiceForClient
    {
        /// <summary>
        /// 获取指定堆垛机的状态信息
        /// </summary>
        /// <param name="deviceName">堆垛机名</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]     
        JsonOperationResult GetStatusByName(String deviceName);

        /// <summary>
        /// 获取所有堆垛机的状态信息
        /// </summary>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]     
        JsonOperationResult GetAllStatus();
    }
}
