using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Wcs;
using Wcs.Framework;
using NHibernate.Linq;
using NLog;
using Wcs.Framework.Exceptions;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Runtime.Remoting.Messaging;

namespace WcsServiceForClient
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public  class TaskServiceForClient:ITaskServiceForClient
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 获取当前所有任务
        /// </summary>
        /// <returns></returns>
        public JsonOperationResult GetTasks()
        {
            try
            {
                List<Task> tasks;
                using(NHUnitOfWork unitOfWork=new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    tasks = unitOfWork.session.Query<Task>().ToList();
                    unitOfWork.Commit();
                }

                var q = from o in tasks
                        select  new 
                        {
                            o.TaskCode, //任务号
                            o.Id,       //任务编号
                            o.Source,   //任务来源
                            o.StartLocation, //起点
                            o.EndLocation,   //终点
                            o.CurrentLocation, //当前位置
                            o.CreatedAt,       //创建时间
                            o.BizType,          //类型
                            o.Status,           //状态
                            o.Priority,         //优先级
                            o.ContainerCodes,   //容器编码
                           
                        };

                return new JsonOperationResult(true, q.ToList());
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        public JsonOperationResult GetTaskDetails(string taskNo)
        {
            try
            {
                Task task;
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Query<Task>()
                        .Where(x => x.TaskCode == taskNo)
                        .FirstOrDefault();
                    unitOfWork.Commit();
                }
                var ss = new
                {

                    task.TaskCode, //任务号
                    task.Id,       //任务编号
                    task.Source,   //任务来源
                    task.StartLocation, //起点
                    task.EndLocation,   //终点
                    task.CurrentLocation, //当前位置
                    task.CreatedAt,       //创建时间
                    task.BizType,          //类型
                    task.Status,           //状态
                    task.Priority,         //优先级
                    task.ContainerCodes,   //容器编码
                    Movements = task.Movements.Select(m => new
                    {
                        m.Id,
                        m.RouteId,
                        m.Status,
                        m.StartLocation,
                        m.DeviceName,
                        m.EndLocation,
                        Routes=getRouteInfo(m.RouteId),
                        EquipmentActions = m.EquipmentActions.Select(act => new 
                        {
                            act.Id,
                            act.Status,
                            act.EquipmentTaskId,
                            act.DeviceName,
                            ReadableDescription = act.ToReadableDescription()
                        }),
                    }),

                };

                return new JsonOperationResult(true,ss);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        LocationInfo[] getRouteInfo(Int32? routeId)
        {
            if (routeId == null)
            {
                return new LocationInfo[0];
            }
            var route = Wcs.Framework.Cfg.WcsConfiguration.Instance.RouteCollection.Routes.SingleOrDefault(x => x.Id == routeId);
            if (route != null)
            {
                return route.Locations.Select(x => LocationConverter.ToLocationInfo(x)).ToArray();
            }
            else
            {
                return new LocationInfo[0];
            }
        }

        public JsonOperationResult SuspendTask(string taskNo)
        {
            try
            {
                Task task;
                // var taskId = Convert.ToInt32(taskNo);
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Query<Task>()
                        .Where(x => x.TaskCode == taskNo)
                        .FirstOrDefault();
                    unitOfWork.Commit();
                }


                if (task == null)
                {
                    _logger.Error1(new TaskNotFoundException(taskNo), this);
                }
                var taskId = task.Id;

                TaskHelper.Suspend(taskId);

                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }


        }

        public JsonOperationResult CompleteTask(int id)
        {
            try
            {
                TaskHelper.Complete(id);

                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {
                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        public JsonOperationResult CompleteMovenent(int id)
        {
            try
            {
                TaskHelper.CompleteMovement(id);

                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }



        }

        public JsonOperationResult CompleteAction(int id)
        {
            try
            { 

                TaskHelper.CompleteAction(id);

                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        public JsonOperationResult CancelTask(string taskNo)
        {
            try
            {
                Task task;
                // var taskId = Convert.ToInt32(taskNo);
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Query<Task>()
                        .Where(x => x.TaskCode == taskNo)
                        .FirstOrDefault();
                    unitOfWork.Commit();
                }


                if (task == null)
                {
                    _logger.Error1(new TaskNotFoundException(taskNo), this);
                }
                var taskId = task.Id;
                TaskHelper.CancelTask(taskId);

                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        public JsonOperationResult ResumeTask(string taskNo)
        {
            try
            {
                Task task;
            
                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Query<Task>()
                        .Where(x => x.TaskCode == taskNo)
                        .FirstOrDefault();
                    unitOfWork.Commit();
                }


                if (task == null)
                {
                    _logger.Error1(new TaskNotFoundException(taskNo),this);
                }
                var taskId = task.Id;


                //在本次调用上下文中关闭用户信息验证框（仅本次有效）
                CallContext.SetData("双货叉.启用任务继续执行信息验证", false);
                CallContext.SetData("单货叉.启用任务继续执行信息验证", false);

                TaskHelper.Resume(taskId,null);

               return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }
        }

        public JsonOperationResult ResumeTaskWithCurrentLocation(string taskNo, string currentLocation)
        {
            try
            {
                Task task;

                using (NHUnitOfWork unitOfWork = new NHUnitOfWork(System.Data.IsolationLevel.ReadUncommitted))
                {
                    task = unitOfWork.session.Query<Task>()
                        .Where(x => x.TaskCode == taskNo)
                        .FirstOrDefault();
                    unitOfWork.Commit();
                }


                if (task == null)
                {
                    _logger.Error1(new TaskNotFoundException(taskNo), this);
                }
                var taskId = task.Id;


                //在本次调用上下文中关闭用户信息验证框（仅本次有效）
                CallContext.SetData("双货叉.启用任务继续执行信息验证", false);
                CallContext.SetData("单货叉.启用任务继续执行信息验证", false);

                TaskHelper.Resume(taskId, LocationConverter.UserCodeToLcation(currentLocation));


                return new JsonOperationResult(true,null);
            }
            catch (Exception ex)
            {

                return new JsonOperationResult(false,ex.Message,null);
            }
        }
    }
}
