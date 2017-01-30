using System.Collections.Generic;
using System.Web.Mvc;
using Web.Helpers;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new UserAuthorizeAttribute { Areas = new List<string> { "Platform" } });
            filters.Add(new StatisticsTrackerAttribute());
        }
    }
}
