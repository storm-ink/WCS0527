using NLog;
using System;
using System.Net;
using System.Web.Http;
using Wcs;

namespace ZHQXC
{
    [RoutePrefix("api/v1/operations/movements")]
    public class OperationsMovementsController : ApiController
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly OperationsTaskApplicationService _taskService = new OperationsTaskApplicationService();

        [Route("{movementId:int}/complete")]
        [HttpPost]
        public IHttpActionResult CompleteMovement(int movementId)
        {
            return ExecuteMovementMutation(
                movementId,
                delegate { return _taskService.CompleteMovementById(movementId); },
                "ops_movement_complete_failed",
                "逻辑动作已完成");
        }

        [Route("{movementId:int}/cancel")]
        [HttpPost]
        public IHttpActionResult CancelMovement(int movementId)
        {
            return ExecuteMovementMutation(
                movementId,
                delegate { return _taskService.CancelMovementById(movementId); },
                "ops_movement_cancel_failed",
                "逻辑动作已取消");
        }

        IHttpActionResult ExecuteMovementMutation(int movementId, Func<ZHQXC.Client.WCSTask> operation, string errorCode, string successMessage)
        {
            if (movementId <= 0)
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "movementId 必须大于 0", new UnifiedApiError("movementId", "movementId 必须大于 0")));
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
