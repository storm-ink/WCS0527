using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcsServiceForClient
{
    [ServiceContract]
    public interface ITaskServiceForClient
    {
        /// <summary>
        /// 获取所有任务
        /// </summary>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult GetTasks();
        /// <summary>
        /// 获取指定任务号的任务明细（包括子逻辑动作物理动作等）信息
        /// </summary>
        /// <param name="taskNo">任务号</param>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult GetTaskDetails(String taskNo);
        /// <summary>
        /// 暂停指定的任务
        /// </summary>
        /// <param name="taskNo">任务号</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult SuspendTask(String taskNo);
        /// <summary>
        /// 完成指定的任务对象
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult CompleteTask(Int32 id);
        /// <summary>
        /// 完成指定的逻辑动作对象
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult CompleteMovenent(Int32 id);
        /// <summary>
        /// 完成指定的物理动作对象
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult CompleteAction(Int32 id);
        /// <summary>
        /// 取消指定的任务
        /// </summary>
        /// <param name="taskNo">任务号</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult CancelTask(String taskNo);
        /// <summary>
        /// 继续执行指定的任务
        /// </summary>
        /// <param name="taskNo">任务号</param>
        ///</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult ResumeTask(String taskNo);
        /// <summary>
        /// 继续执行指定的任务
        /// </summary>
        /// <param name="taskNo">任务号</param>
        /// <param name="currentLocation">当前停靠位置</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json)]  
        JsonOperationResult ResumeTaskWithCurrentLocation(String taskNo, String currentLocation);
    }
}
