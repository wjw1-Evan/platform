using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using IServices.ISysServices;
using Models.SysModels;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class StatisticsTrackerAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private DateTime _datetimenow;

        #region Action时间监控
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _datetimenow = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        #endregion

        #region View 视图生成时间监控
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            //记录用户访问记录
            var area = (string)filterContext.RouteData.DataTokens["area"];
            var action = (string)filterContext.RouteData.Values["action"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var recordId = (string)filterContext.RouteData.Values["id"];

            var sysControllerSysActionService = DependencyResolver.Current.GetService<ISysControllerSysActionService>();
            var sysUserLogService = DependencyResolver.Current.GetService<ISysUserLogService>();

            var sysControllerSysAction =
                sysControllerSysActionService.GetAll(a => a.SysController.ControllerName.Equals(controller) &&
                            a.SysController.SysArea.AreaName.Equals(area) && a.SysAction.ActionName.Equals(action)).Include(a => a.SysController.SysArea).Include(a => a.SysController).Include(a => a.SysAction).OrderBy(a => a.SysController.SystemId).FirstOrDefault();

            var sysuserlog = new SysUserLog
            {
                Ip = filterContext.RequestContext.HttpContext.Request.ServerVariables["Remote_Addr"],
                SysControllerSysActionId = sysControllerSysAction?.Id,
                RecordId = recordId,
                Url = filterContext.RequestContext.HttpContext.Request.RawUrl,
                SysArea = sysControllerSysAction?.SysController.SysArea.Name ?? area,
                SysController = sysControllerSysAction?.SysController.Name ?? controller,
                SysAction = sysControllerSysAction?.SysAction.Name ?? action,
                Duration = Math.Round((DateTime.Now - _datetimenow).TotalMilliseconds),
                RequestType = filterContext.HttpContext.Request.RequestType
            };

            sysUserLogService.Save(null, sysuserlog);

            sysUserLogService.Commit();
        }

        #endregion



    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class WebApiTrackerAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        private DateTime _datetimenow;

        #region Action时间监控
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _datetimenow = DateTime.Now;
            base.OnActionExecuting(actionContext);
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
          

            ////记录用户访问记录
            var action = (string)actionExecutedContext.ActionContext.RequestContext.RouteData.Values["action"];
            var controller = (string)actionExecutedContext.ActionContext.RequestContext.RouteData.Values["controller"];
            var recordId = (string)actionExecutedContext.ActionContext.RequestContext.RouteData.Values["id"];

            var sysuserlog = new SysUserLog
            {
                Ip = HttpContext.Current.Request.UserHostAddress,
                RecordId = recordId,
                Url = actionExecutedContext.Request.RequestUri.PathAndQuery,
                SysArea = "WebApi",
                SysController = controller,
                SysAction = action,
                Duration = Math.Round((DateTime.Now - _datetimenow).TotalMilliseconds),
                RequestType =actionExecutedContext.Request.Method.Method
            };

            var sysUserLogService = DependencyResolver.Current.GetService<ISysUserLogService>();

            sysUserLogService.Save(null, sysuserlog);

            sysUserLogService.Commit();
            base.OnActionExecuted(actionExecutedContext);
        }
       

        #endregion
    }
}