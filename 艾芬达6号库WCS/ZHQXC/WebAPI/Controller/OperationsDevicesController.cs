using NLog;
using System;
using System.Net;
using System.Web.Http;

namespace ZHQXC
{
    [RoutePrefix("api/v1/operations/devices")]
    public class OperationsDevicesController : ApiController
    {
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        readonly OperationsDeviceApplicationService _deviceService = new OperationsDeviceApplicationService();

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetDevices()
        {
            return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(_deviceService.GetDevices()));
        }

        [Route("{deviceName}")]
        [HttpGet]
        public IHttpActionResult GetDevice(string deviceName)
        {
            if (string.IsNullOrWhiteSpace(deviceName))
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "deviceName 不可为空", new UnifiedApiError("deviceName", "deviceName 不可为空")));
            }

            OperationsDeviceDto device = _deviceService.GetDevice(deviceName);
            if (device == null)
            {
                return Content(HttpStatusCode.NotFound, UnifiedApiResponse.Fail("ops_device_not_found", string.Format("未找到设备 {0}", deviceName)));
            }

            return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(device));
        }

        [Route("{deviceName}/lock")]
        [HttpPost]
        public IHttpActionResult LockDevice(string deviceName)
        {
            return ExecuteDeviceLock(deviceName, true, "ops_device_lock_failed");
        }

        [Route("{deviceName}/unlock")]
        [HttpPost]
        public IHttpActionResult UnlockDevice(string deviceName)
        {
            return ExecuteDeviceLock(deviceName, false, "ops_device_unlock_failed");
        }

        IHttpActionResult ExecuteDeviceLock(string deviceName, bool lockDevice, string errorCode)
        {
            if (string.IsNullOrWhiteSpace(deviceName))
            {
                return Content(HttpStatusCode.BadRequest,
                    UnifiedApiResponse.Fail("common_validation_error", "deviceName 不可为空", new UnifiedApiError("deviceName", "deviceName 不可为空")));
            }

            try
            {
                DeviceLockerHand result = _deviceService.SetDeviceLock(deviceName, lockDevice);
                if (!result.Result)
                {
                    return Content(HttpStatusCode.Conflict, UnifiedApiResponse.Fail(errorCode, result.Message));
                }

                return Content(HttpStatusCode.OK, UnifiedApiResponse.Ok(_deviceService.GetDevice(deviceName), result.Message));
            }
            catch (Exception ex)
            {
                _logger.Error1(ex, this);
                return Content(HttpStatusCode.InternalServerError, UnifiedApiResponse.Fail(errorCode, ex.Message));
            }
        }
    }
}
