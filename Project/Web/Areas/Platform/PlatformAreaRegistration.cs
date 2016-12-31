using System.Web.Mvc;
using DoddleReport.Web;

namespace Web.Areas.Platform
{
    public class PlatformAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 网站管理平台
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Platform";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapReportingRoute();

            context.MapRoute(
                "Platform_default",
                "Platform/{controller}/{action}/{id}",
                new {Controller="Index", action = "Index", id = UrlParameter.Optional },
                new[] { "Web.Areas.Platform.Controllers" }
            );
        }
    }
}