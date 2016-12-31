using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using BootstrapSupport;

namespace Web.Areas.Platform.Helpers
{

    /// <summary>
    /// 扩展添加和编辑后的返回结果，提示和跳转
    /// </summary>
    public class EditSuccessResult : ActionResult
    {
        private RouteValueDictionary RouteValues { get; set; }
        private string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="routeValues"></param>
        public EditSuccessResult(string id,RouteValueDictionary routeValues=null)
        {
            Id = id;
            RouteValues = routeValues ?? new RouteValueDictionary();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.TempData[Alerts.Success] = string.IsNullOrEmpty(Id) ? "添加成功" : "编辑成功";
            
            foreach (var key in context.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(a => !a.IsEmpty()))
            {
                RouteValues.Add(key, context.RequestContext.HttpContext.Request.QueryString[key]);
            }
       
            RouteValues.Add("action", string.IsNullOrEmpty(Id) ? "Create" : "Index");

            var result = new RedirectToRouteResult(RouteValues);
            
            result.ExecuteResult(context);
        }

    }
}