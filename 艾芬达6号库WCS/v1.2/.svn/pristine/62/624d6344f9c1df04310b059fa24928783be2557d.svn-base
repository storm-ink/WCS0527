using System.Web.Mvc;

namespace Client.WebUI.Areas
{
    public class SingleForkCraneAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SingleForkCrane";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SingleForkCrane_default",
                "SingleForkCrane/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Client.WebUI.Areas.SingleForkCrane.Controllers" }
            );
        }
    }
}
