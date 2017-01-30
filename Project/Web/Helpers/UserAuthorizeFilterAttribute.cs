using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using IServices.ISysServices;

namespace Web.Helpers
{
    /// <summary>
    /// 用户身份验证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 需要验证的区域   
        /// </summary>
        public IList<string> Areas { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var area = (string)httpContext.Request.RequestContext.RouteData.DataTokens["area"];

            //是否对该Area区域进行身份验证
            if (!Areas.Contains(area)) return true;

            //判断是否用户已登录
            if (!httpContext.User.Identity.IsAuthenticated) return false;

            //判断用户是否有该区域访问的权限
            //如果权限中有该区域的任何一个操作既可以进行访问
            var action = (string)httpContext.Request.RequestContext.RouteData.Values["action"];
            var controller = (string)httpContext.Request.RequestContext.RouteData.Values["controller"];
            //var recordId = (string)httpContext.Request.RequestContext.RouteData.Values["id"];

            var sysRoleService = DependencyResolver.Current.GetService<ISysRoleService>();
            var userInfo = DependencyResolver.Current.GetService<IUserInfo>();

            //检测用户是否有权限访问 
            if (!sysRoleService.CheckSysUserSysRoleSysControllerSysActions(userInfo.UserId, area, action, controller))
                throw new Exception("用户：" + userInfo.UserName + "(" + userInfo.UserId + ") 没有权限访问 " + area + " > " + controller + " > " + action + " ！请联系系统管理员进行权限分配！");
        
            return true;
        }
    }
}