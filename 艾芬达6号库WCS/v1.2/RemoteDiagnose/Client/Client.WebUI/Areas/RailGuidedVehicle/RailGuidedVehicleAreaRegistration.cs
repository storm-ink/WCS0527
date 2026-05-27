using System.Web.Mvc;

namespace Client.WebUI.Areas
{
    public class RailGuidedVehicleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RailGuidedVehicle";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RailGuidedVehicle_default",
                "RailGuidedVehicle/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Client.WebUI.Areas.RailGuidedVehicle.Controllers" }
            );
        }
    }
}
