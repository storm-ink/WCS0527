using System.Web.Mvc;

namespace Client.WebUI.Areas
{
    public class ConveyorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Conveyor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Conveyor_default",
                "Conveyor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Client.WebUI.Areas.Conveyor.Controllers" }
            );
        }
    }
}
