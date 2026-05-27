using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Areas.RailGuidedVehicle.Controllers
{
    [Authorize]
    public class DeviceServiceController : Controller
    {
        [WmsOperation("单货位穿梭车.控制服务.诊断指令")]
        public ContentResult Diagnose(string name)
        {
            try
            {
                using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
                {
                    return Content(client.Diagnose(name));
                }
            }
            catch (Exception ex)
            {
                var msg = getRemoteExceptionMessage(ex);
                throw new Exception(msg);
            }
        }

        [WmsOperation("单货位穿梭车.控制服务.获取实时状态")]
        [HttpGet]
        public ContentResult GetAllStatus()
        {
            using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
            {
                return Content(client.GetAllStatus());
            }
        }

        [WmsOperation("单货位穿梭车.控制服务.移动指令")]
        [HttpPost]
        public JsonResult Move(string name, int station)
        {
            try
            {
                using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
                {
                    client.Move(name, station);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 移动指令发送成功", name)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位穿梭车.控制服务.取货指令")]
        [HttpPost]
        public JsonResult Picking(string name, int direction)
        {
            try
            {
                using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
                {
                    client.Picking(name, direction);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 取货指令发送成功", name)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位穿梭车.控制服务.放货指令")]
        [HttpPost]
        public JsonResult Putting(string name, int direction)
        {
            try
            {
                using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
                {
                    client.Putting(name, direction);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 放货指令发送成功", name)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }

        [WmsOperation("单货位穿梭车.控制服务.清除任务指令")]
        [HttpPost]
        public JsonResult ClearTask(string name)
        {
            try
            {
                using (App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient client = new App.RailGuidedVehicleDeviceServiceProxy.RailGuidedVehicleServiceClient())
                {
                    client.ClearTask(name);
                }

                return Json(new
                {
                    success = true,
                    msg = string.Format("{0} 任务清除指令发送成功", name)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = getRemoteExceptionMessage(ex)
                });
            }
        }


        String getRemoteExceptionMessage(Exception ex)
        {
            var msg =  ex.Message;

            var match=Regex.Match(msg,@"System\.ServiceModel\.FaultException\:\s*(?<msg>.*?)\r\n");
            if(match!=null)
            {
                return match.Groups["msg"].Value;
            }else{
                return msg;
            }
        }
    }
}
