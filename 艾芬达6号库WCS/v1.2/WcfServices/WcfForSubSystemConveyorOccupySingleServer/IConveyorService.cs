using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfForSubSystemConveyorOccupySingleServer
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IConveyorService”。
    [ServiceContract]
    public interface IConveyorService
    {
        /// <summary>
        /// 获取输送线站位数据数据
        /// </summary>
        /// <param name="ListConveyor">需要获取的输送线列表</param>
        /// <returns>输送线号和对应的输送线数据，对应的Dictionar集合</returns>
        [OperationContract]
        Dictionary<int, ConveyorDate> GetConveyorState(List<int> ListConveyor);
        /// <summary>
        /// 获取所有输送线状态数据
        /// </summary>
        /// <returns>所有输送线数据状态</returns>
        [OperationContract]
        Dictionary<int, ConveyorDate> GetAllStat();
    }
}
