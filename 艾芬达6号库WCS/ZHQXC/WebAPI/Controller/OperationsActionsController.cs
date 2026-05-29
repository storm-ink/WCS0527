using NLog;
using System;
using System.Net;
using System.Web.Http;
using Wcs;

namespace ZHQXC
{
    [RoutePrefix("api/v1/operations/actions")]
    public class OperationsActionsController : ApiController
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly OperationsTaskApplicationService _taskService = new OperationsTaskApplicationService();

        [Route("{actionId:int}/complete")]
        [HttpPost]
        public IHttpActionResult CompleteAction(int actionId)
        {
            return ExecuteActionMutation(
                actionId,
                delegate { return _taskService.CompleteActionById(actionId); },
                "ops_action_complete_failed",
                "物理动作已完成");
        }

        [Route("{actionId:int}/cancel")]
        [HttpPost]
        public IHttpActionResult CancelAction(int actionId)
        {
            return ExecuteActionMutation(
                actionId,
                delegate { return _taskService.CancelActionById(actionId); },
                "ops_action_cancel_failed",
                "物理动作已取消");
        }

        IHttpActionResult ExecuteActionMutation(int actionId, Func<ZHQXC.Client.WCSTask> operation, string errorCode, string successMessage)
        {
            if (actionId <= 0)
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "actionId 必须大于 0", new UnifiedApiError("actionId", "actionId 必须大于 0")));
            }

            try
            {
                return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(operation(), successMessage));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                if (ex is ArgumentException || ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
                {
                    return Content(HttpStatusCode.BadRequest, UnifiedApiResponse.Fail("common_validation_error", ex.Message));
                }

                return Content(HttpStatusCode.InternalServerError, UnifiedApiResponse.Fail(errorCode, ex.Message));
            }
        }
    }
}
