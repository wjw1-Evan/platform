using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using BootstrapSupport;
using Resources;

namespace Web.Areas.Platform.Helpers
{

    /// <summary>
    /// 扩展删除后的返回结果，提示和跳转
    /// </summary>
    public class DeleteSuccessResult : ActionResult
    {
        private RouteValueDictionary RouteValues { get; set; }
        private string Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="routeValues"></param>
        public DeleteSuccessResult(string index = "Index", RouteValueDictionary routeValues = null)
        {
            Index = index;
            RouteValues = routeValues ?? new RouteValueDictionary();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.TempData[Alerts.Success] = Lang.DeleteSuccess;

            foreach (var key in context.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(a => !a.IsEmpty()))
            {
                RouteValues.Add(key, context.RequestContext.HttpContext.Request.QueryString[key]);
            }

            RouteValues.Add("action",Index);

            var result = new RedirectToRouteResult(RouteValues);

            result.ExecuteResult(context);
        }

    }

}