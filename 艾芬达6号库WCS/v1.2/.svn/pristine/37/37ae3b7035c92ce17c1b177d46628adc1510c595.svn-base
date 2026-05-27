using Spiral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using Spiral.Web;
namespace Client.WebUI.Areas.SingleForkCrane.Controllers
{
    [Authorize]
    public class DiagnoseController : Controller
    {
        [WmsOperation("单货位堆垛机.远程诊断")]
        public ActionResult Index()
        {
            using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
            {
                var names = context.Session.Query<Client.SingleForkCrane.StatusDiagnosisData>()
                    .GroupBy(x => x.Name)
                    .Select(x => x.Key)
                    .OrderBy(x => x)
                    .ToList();

                NhRepositoryFactory factory = new NhRepositoryFactory(context);
                var userRepository = factory.Create<Spiral.Base.UserRepositoryBase>();

                var user = userRepository.GetByUserName(this.HttpContext.User.Identity.Name);

                ViewBag.RealName = user.RealName;

                ViewBag.CraneNames = names;
            }

            return View();
        }

    }
}
