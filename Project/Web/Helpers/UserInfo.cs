using System.Web;
using IServices.ISysServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.Helpers
{

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : IUserInfo
    {
        
        /// <summary>
        /// 用户当前企业ID
        /// </summary>
        public string EnterpriseId => GetEnterpriseId();

        private static string GetEnterpriseId()
        {
            var userManager = HttpContext.Current?.GetOwinContext()?.GetUserManager<ApplicationUserManager>();

            var user = userManager?.FindById(UserInfo.GetUserId());

            return user?.CurrentEnterpriseId;

        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId => GetUserId();

        private static string GetUserId()
        {
            return HttpContext.Current?.User?.Identity?.GetUserId();
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName => GetUserName();


        private static string GetUserName()
        {
            return HttpContext.Current?.User?.Identity?.Name;
        }

    }
}