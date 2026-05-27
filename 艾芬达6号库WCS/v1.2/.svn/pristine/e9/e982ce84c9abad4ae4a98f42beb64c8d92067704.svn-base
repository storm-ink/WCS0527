using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Matedata
{
    /// <summary>
    /// 表示继承此接口的类型是一个元数据服务
    /// </summary>
    [ServiceContract]
    public interface IMatedataService
    {
        /// <summary>
        /// 获取诊断元数据
        /// </summary>
        /// <param name="minDataId">最小数据 Id</param>
        /// <param name="batchSize">批大小</param>
        /// <param name="appInfo">附加信息</param>
        /// <returns></returns>
        [OperationContract]
        String GetDiagnoseMatedatas(int minDataId, int batchSize,String appInfo);
    }
}
