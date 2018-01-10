using EFSecondLevelCache;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Web.Helpers
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class StatisticsTrackerAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private DateTime _actiondatetimenow;

        private DateTime _viewdatetimenow;

        private double _actionDuration;

        private double _viewDuration;

        #region Action时间监控
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _actiondatetimenow = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            _actionDuration = Math.Round((DateTime.Now - _actiondatetimenow).TotalMilliseconds, 3);

        }
        #endregion

        #region View 视图生成时间监控
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _viewdatetimenow = DateTime.Now;
            base.OnResultExecuting(filterContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            _viewDuration = Math.Round((DateTime.Now - _viewdatetimenow).TotalMilliseconds, 3);


            //记录用户访问记录
            var area = (string)filterContext.RouteData.DataTokens["area"];
            var action = (string)filterContext.RouteData.Values["action"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var recordId = (string)filterContext.RouteData.Values["id"];

            var sysControllerSysActionService = DependencyResolver.Current.GetService<ISysControllerSysActionService>();
            var sysUserLogService = DependencyResolver.Current.GetService<ISysUserLogService>();
            var iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

            var sysControllerSysAction =
                sysControllerSysActionService.GetAll(a => a.SysController.ControllerName.Equals(controller) &&
                            a.SysController.SysArea.AreaName.Equals(area) && a.SysAction.ActionName.Equals(action)).OrderBy(a => a.SysController.SystemId).Select(a => new { a.Id, SysAreaName = a.SysController.SysArea.Name, SysControllerName = a.SysController.Name, SysActionName = a.SysAction.Name }).Cacheable().FirstOrDefaultAsync().Result;

            var sysuserlog = new SysUserLog
            {
                Ip = filterContext.RequestContext.HttpContext.Request.ServerVariables["Remote_Addr"],
                RecordId = recordId,
                Url = filterContext.RequestContext.HttpContext.Request.RawUrl,
                SysArea = sysControllerSysAction?.SysAreaName ?? area,
                SysController = sysControllerSysAction?.SysControllerName ?? controller,
                SysAction = sysControllerSysAction?.SysActionName ?? action,
                ViewDuration = _viewDuration,
                ActionDuration = _actionDuration,
                Duration = Math.Round((DateTime.Now - _actiondatetimenow).TotalMilliseconds, 3),
                RequestType = filterContext.HttpContext.Request.RequestType
            };

            sysUserLogService.Save(null, sysuserlog);

            iUnitOfWork.CommitAsync().Wait();

        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
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
            base.OnActionExecuted(actionExecutedContext);

            ////记录用户访问记录
            var routetemplate = actionExecutedContext.ActionContext.RequestContext.RouteData.Route.RouteTemplate;


            var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var controller = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var sysuserlog = new SysUserLog
            {
                Ip = HttpContext.Current.Request.UserHostAddress,
                RecordId = "",
                Url = actionExecutedContext.Request.RequestUri.PathAndQuery,
                SysArea = "WebApi",
                SysController = controller,
                SysAction = action,
                ActionDuration = Math.Round((DateTime.Now - _datetimenow).TotalSeconds, 3),
                Duration = Math.Round((DateTime.Now - _datetimenow).TotalSeconds, 3),
                RequestType = actionExecutedContext.Request.Method.Method,

            };



            var sysUserLogService = DependencyResolver.Current.GetService<ISysUserLogService>();
            var iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

            sysUserLogService.Save(null, sysuserlog);

            iUnitOfWork.CommitAsync().Wait();

        }


        #endregion
    }
}
