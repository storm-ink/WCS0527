using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Wcs;
using WebUI.WebUIService;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult GCCollect()
        {
            GC.Collect();
            return Content("OK");
        }
        public ActionResult Index()
        {
            return Redirect("/index.htm");
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }

        [HttpGet]
        public ActionResult GetConveyorCurrentTasks(String deviceName)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result=uisc.GetConveyorCurrentTasks(deviceName);
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetConveyorStatus(String deviceName)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.GetConveyorStatus(deviceName);
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetConveyorStatusFromLastReceivedPackage(String deviceName,string type)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.GetConveyorStatusFromLastReceivedPackage(deviceName, type);
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTasks()
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.GetTasks();
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTaskDetails(string taskNo)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.GetTaskDetails(taskNo);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SuspendTask(string taskNo)
        {
            try
            {
                using (WebUIServiceClient uisc = new WebUIServiceClient())
                {
                    var result = uisc.SuspendTask(taskNo);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success=false,
                    Data = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public ActionResult CompleteTask(String ObjectType, Int32 id)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.CompleteTask(ObjectType, id);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CancelTask(String taskNo)
        {
            using (WebUIServiceClient uisc = new WebUIServiceClient())
            {
                var result = uisc.CancelTask(taskNo);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ResumeTask(String taskNo)
        {
            try
            {
                using (WebUIServiceClient uisc = new WebUIServiceClient())
                {
                    var result = uisc.ResumeTask(taskNo);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Data = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ResumeTaskWithCurrentLocation(String taskNo, String currentLocation)
        {
            try
            {
                using (WebUIServiceClient uisc = new WebUIServiceClient())
                {
                    var result = uisc.ResumeTaskWithCurrentLocation(taskNo, currentLocation);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Data = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDevicesConnectionStatus()
        {
            try
            {
                using (WebUIServiceClient uisc = new WebUIServiceClient())
                {
                    var result = uisc.GetDevicesConnectionStatus();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Data = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }        
    }
}
