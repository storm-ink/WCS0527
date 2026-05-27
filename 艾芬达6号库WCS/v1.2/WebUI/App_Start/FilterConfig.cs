using System.Web;
using System.Web.Mvc;

namespace WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var attr = new HandleErrorAttribute();
            attr.Order = 10000;
            filters.Add(attr);
        }
    }
}