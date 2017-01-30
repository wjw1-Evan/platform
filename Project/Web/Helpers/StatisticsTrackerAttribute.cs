using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using IServices.ISysServices;
using Models.SysModels;

namespace Web.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class StatisticsTrackerAttribute : ActionFilterAttribute
    {
        private DateTime _datetimenow;

        #region Action时间监控
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _datetimenow = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        #endregion

        #region View 视图生成时间监控
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            //记录用户访问记录
            var area = (string)filterContext.RouteData.DataTokens["area"];
            var action = (string)filterContext.RouteData.Values["action"];
            var controller = (string)filterContext.RouteData.Values["controller"];
            var recordId = (string)filterContext.RouteData.Values["id"];

            var userInfo = DependencyResolver.Current.GetService<IUserInfo>();
            var sysControllerSysActionService = DependencyResolver.Current.GetService<ISysControllerSysActionService>();
            var sysUserLogService = DependencyResolver.Current.GetService<ISysUserLogService>();

            var sysControllerSysAction =
                sysControllerSysActionService.GetAll(a => a.SysController.ControllerName.Equals(controller) &&
                            a.SysController.SysArea.AreaName.Equals(area) && a.SysAction.ActionName.Equals(action)).Include(a => a.SysController.SysArea).Include(a => a.SysController).Include(a => a.SysAction).OrderBy(a => a.SysController.SystemId).FirstOrDefault();

            var sysuserlog = new SysUserLog
            {
                UserName = userInfo.UserName,
                EnterpriseId = userInfo.EnterpriseId,
                CreatedBy = userInfo.UserId,
                Ip = filterContext.RequestContext.HttpContext.Request.ServerVariables["Remote_Addr"],
                SysControllerSysActionId = sysControllerSysAction?.Id,
                RecordId = recordId,
                Url = filterContext.RequestContext.HttpContext.Request.RawUrl,
                SysArea = sysControllerSysAction?.SysController.SysArea.Name ?? area,
                SysController = sysControllerSysAction?.SysController.Name ?? controller,
                SysAction = sysControllerSysAction?.SysAction.Name ?? action,
                Duration = (DateTime.Now - _datetimenow).TotalSeconds,
                RequestType= filterContext.HttpContext.Request.RequestType
            };

            sysUserLogService.Save(null, sysuserlog);

            sysUserLogService.Commit();
        }

        #endregion



    }

}