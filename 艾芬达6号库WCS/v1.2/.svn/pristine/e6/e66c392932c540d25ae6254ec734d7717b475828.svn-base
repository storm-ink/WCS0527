using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wcs.WebClient.Controllers
{
    public class MonitorController : Controller
    {
        string StageDir
        {
            get
            {
                var path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                var dir = System.IO.Path.Combine(path, "stages");
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                return dir;
            }
        }

        //
        // GET: /Monitor/

        public ActionResult Index(String name)
        {
            var filePath = System.IO.Path.Combine(StageDir, name + ".txt");
            var contents = System.IO.File.ReadAllText(filePath);

            ViewBag.content = contents;
            return View();
        }

    }
}
