using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.AudioFormat;
using System.Web;
using System.Web.Mvc;

namespace Client.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public FileResult Speek(String text)
        {
            byte[] bytes=new byte[0];

            System.Threading.ThreadPool.QueueUserWorkItem((stat) =>
            {
                using (System.Speech.Synthesis.SpeechSynthesizer ss = new System.Speech.Synthesis.SpeechSynthesizer())
                {
                    ///推荐保存方式 ，编码率11K，默认44文件较大！小于11K 时声音变化不理想！
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        ss.SetOutputToWaveStream(ms);
                        ss.Speak(text);

                        bytes = ms.GetBuffer();
                    }
                }

            });

            while (bytes.Length == 0)
            {
                System.Threading.Thread.Sleep(200);
            }
            
            return File(bytes, "audio/x-wav");
        }

        public ActionResult Workers()
        {

            return View(App.PeriodicWorkerManager.Workers);
        }

        public ActionResult StopWorker(Int32 id)
        {
            var worker = App.PeriodicWorkerManager.Workers.FirstOrDefault(x => x.GetHashCode() == id);
            if (worker != null)
            {
                worker.Stop();
            }

            return RedirectToAction("Workers");
        }

        public ActionResult StartWorker(Int32 id)
        {
            var worker = App.PeriodicWorkerManager.Workers.FirstOrDefault(x => x.GetHashCode() == id);
            if (worker != null)
            {
                worker.Start();
            }

            return RedirectToAction("Workers");
        }
    }
}
