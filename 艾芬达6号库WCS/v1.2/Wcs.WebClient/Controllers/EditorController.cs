using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wcs.WebClient.Controllers
{
    public class EditorController : Controller
    {
        //
        // GET: /Editor/

        public ActionResult Index()
        {
            return View();
        }

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
        
        public ActionResult Load(string name)
        {
            var filePath = System.IO.Path.Combine(StageDir, name + ".txt");
            var contents = System.IO.File.ReadAllText(filePath);

            return Content(contents);
        }

        public JsonResult LoadList(string name)
        {
            var files = System.IO.Directory.GetFiles(StageDir, "*.txt");
            var result=files.Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(string name, string contents)
        {
            var filePath = System.IO.Path.Combine(StageDir, name + ".txt");
            System.IO.File.WriteAllText(filePath, contents);

            return Content("");
        }
    }
}
