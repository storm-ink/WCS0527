using NLog;
using System;
using System.Net;
using System.Web.Http;
using Wcs;
using ZHQXC.Client;
using Wcs.Framework.Exceptions;

namespace ZHQXC
{
    [RoutePrefix("api/v1/operations/tasks")]
    public class OperationsTasksController : ApiController
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly OperationsTaskApplicationService _taskService = new OperationsTaskApplicationService();

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetTasks()
        {
            return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(_taskService.GetTasks()));
        }

        [Route("{taskCode}")]
        [HttpGet]
        public IHttpActionResult GetTask(string taskCode)
        {
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "taskCode 不可为空", new UnifiedApiError("taskCode", "taskCode 不可为空")));
            }

            WCSTask task = _taskService.GetTaskByCode(taskCode);
            if (task == null)
            {
                return Content(HttpStatusCode.NotFound, UnifiedApiResponse.Fail("ops_task_not_found", string.Format("未找到任务 {0}", taskCode)));
            }

            return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(task));
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult AddTask([FromBody] WCSTask task)
        {
            if (task == null)
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "请求体不可为空", new UnifiedApiError("body", "请求体不可为空")));
            }

            try
            {
                return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(_taskService.AddTask(task), "任务创建成功"));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                if (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return Content(HttpStatusCode.BadRequest, UnifiedApiResponse.Fail("common_validation_error", ex.Message));
                }

                return Content(HttpStatusCode.InternalServerError, UnifiedApiResponse.Fail("ops_task_create_failed", ex.Message));
            }
        }

        [Route("{taskCode}/suspend")]
        [HttpPost]
        public IHttpActionResult SuspendTask(string taskCode)
        {
            return ExecuteTaskMutation(
                taskCode,
                delegate { return _taskService.SuspendTaskByCode(taskCode); },
                "ops_task_suspend_failed",
                "任务已暂停");
        }

        [Route("{taskCode}/cancel")]
        [HttpPost]
        public IHttpActionResult CancelTask(string taskCode)
        {
            return ExecuteTaskMutation(
                taskCode,
                delegate { return _taskService.CancelTaskByCode(taskCode); },
                "ops_task_cancel_failed",
                "任务已取消");
        }

        [Route("{taskCode}/resume")]
        [HttpPost]
        public IHttpActionResult ResumeTask(string taskCode, [FromBody] ResumeTaskRequest request)
        {
            return ExecuteTaskMutation(
                taskCode,
                delegate { return _taskService.ResumeTaskByCode(taskCode, request == null ? null : request.CurrentUserCode); },
                "ops_task_resume_failed",
                "任务已继续");
        }

        [Route("{taskCode}/resume-at-location")]
        [HttpPost]
        public IHttpActionResult ResumeTaskAtLocation(string taskCode, [FromBody] ResumeTaskRequest request)
        {
            return ExecuteTaskMutation(
                taskCode,
                delegate { return _taskService.ResumeTaskByCode(taskCode, request == null ? null : request.CurrentUserCode); },
                "ops_task_resume_failed",
                "任务已继续");
        }

        [Route("{taskCode}/archive")]
        [HttpPost]
        public IHttpActionResult ArchiveTask(string taskCode)
        {
            return ExecuteTaskMutation(
                taskCode,
                delegate { return _taskService.ArchiveTaskByCode(taskCode); },
                "ops_task_archive_failed",
                "任务已归档");
        }

        [Route("by-id/{taskId:int}/complete")]
        [HttpPost]
        public IHttpActionResult CompleteTask(int taskId)
        {
            try
            {
                WCSTask task = _taskService.CompleteTaskById(taskId);
                return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(task, "任务已完成"));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                if (ex is TaskNotFoundException)
                {
                    return Content(HttpStatusCode.NotFound, UnifiedApiResponse.Fail("ops_task_not_found", ex.Message));
                }

                return Content(HttpStatusCode.InternalServerError, UnifiedApiResponse.Fail("ops_task_complete_failed", ex.Message));
            }
        }

        IHttpActionResult ExecuteTaskMutation(string taskCode, Func<WCSTask> operation, string errorCode, string successMessage)
        {
            if (string.IsNullOrWhiteSpace(taskCode))
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "taskCode 不可为空", new UnifiedApiError("taskCode", "taskCode 不可为空")));
            }

            try
            {
                WCSTask task = operation();
                if (task == null)
                {
                    return Content(HttpStatusCode.NotFound, UnifiedApiResponse.Fail("ops_task_not_found", string.Format("未找到任务 {0}", taskCode)));
                }

                return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(task, successMessage));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                if (ex is TaskNotFoundException)
                {
                    return Content(HttpStatusCode.NotFound, UnifiedApiResponse.Fail("ops_task_not_found", ex.Message));
                }

                if (ex is ArgumentException || ex is ArgumentNullException)
                {
                    return Content(HttpStatusCode.BadRequest, UnifiedApiResponse.Fail("common_validation_error", ex.Message));
                }

                return Content(HttpStatusCode.InternalServerError, UnifiedApiResponse.Fail(errorCode, ex.Message));
            }
        }
    }
}
