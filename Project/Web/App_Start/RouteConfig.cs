using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Helpers;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               new[] { "Web.Controllers" }
            );
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //跨域配置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "Api-v1",
            //    routeTemplate: "api/v1/{controller}/{id}",
            //    defaults: new { version = "v1", id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "Api-v2",
            //    routeTemplate: "api/v2/{controller}/{id}",
            //    defaults: new { version="v2", id = RouteParameter.Optional }
            //);

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Filters.Add(new WebApiTrackerAttribute());
        }
    }
}
