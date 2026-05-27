using System.Web.Mvc;

namespace Client.WebUI.Areas
{
    public class WarningAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Warnings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Warnings_default",
                "Warnings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Client.WebUI.Areas.Warnings.Controllers" }
            );
        }
    }
}
