using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Controllers
{
    [Authorize]
    public class MonitorController : Controller
    {
        //
        // GET: /Monitor/
         [Authorize(Roles = "admins")]
        [Spiral.Web.WmsOperation("运行视图.实时监控")]
        public ActionResult Index(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                name = App.MonitorHelper.GetList().Select(x => x.Key).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("未定义任何监控界面");
                }

                return RedirectToAction("Index", new { name = name });
            }

            return View();
        }


       [Spiral.Web.WmsOperation("运行视图.实时监控")]
        public ActionResult Editor()
        {
            return View();
        }

        public JsonResult LoadList(string name)
        {
            var names = App.MonitorHelper.GetList().Select(x => x.Key).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }


        public ContentResult Load(string name)
        {
            return Content(App.MonitorHelper.Read(name));
        }


        public ActionResult Save(string name, string contents)
        {
            App.MonitorHelper.Save(name, contents);
            return Content("");
        }

        [Authorize(Roles = "admins")]
        [Spiral.Web.WmsOperation("运行视图.实时监控")]
        public ActionResult Playback(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                name = App.MonitorHelper.GetList().Select(x => x.Key).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("未定义任何监控界面");
                }

                return RedirectToAction("Playback", new { name = name });
            }

            return View();
        }
    }
}
