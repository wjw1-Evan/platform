using System.Web.Mvc;
using System.Web.Routing;
using DoddleReport.Web;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapReportingRoute();

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               new[] { "Web.Controllers" }
            );
        }
    }
}
