using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;

namespace Client.WebUI.Areas.RailGuidedVehicle.Controllers
{
    public class DiagnoseController : Controller
    {
        //
        // GET: /RailGuidedVehicle/Diagnose/

        public ActionResult Index()
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var names = context.Session.Query<Client.RailGuidedVehicle.RailGuidedVehicleDiagnosisData>()
                    .GroupBy(x => x.Name)
                    .Select(x => x.Key)
                    .OrderBy(x => x)
                    .ToList();

                NhRepositoryFactory factory = new NhRepositoryFactory(context);
                var userRepository = factory.Create<Spiral.Base.UserRepositoryBase>();

                var user = userRepository.GetByUserName(this.HttpContext.User.Identity.Name);

                ViewBag.RealName = user.RealName;

                ViewBag.DeviceNames = names;
            }

            return View();
        }

    }
}
