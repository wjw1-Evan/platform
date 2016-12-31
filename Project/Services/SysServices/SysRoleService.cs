using System;
using System.Linq;
using EntityFramework.Caching;
using EntityFramework.Extensions;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysRoleService : RepositoryBase<SysRole>, ISysRoleService
    {
        public SysRoleService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

        public bool CheckSysUserSysRoleSysControllerSysActions( string userid, string area,
            string action,
            string controller)
        {
            return
                GetAll(a => a.Users.Any(b => b.UserId.Equals(userid)) &&
                                       a.SysRoleSysControllerSysActions.Any(
                                           b =>
                                               b.SysControllerSysAction.SysController.SysArea.AreaName.Equals(area) &&
                                               b.SysControllerSysAction.SysController.ControllerName.Equals(controller) &&
                                               b.SysControllerSysAction.SysAction.ActionName.Equals(action))).FromCache(CachePolicy.WithSlidingExpiration(new TimeSpan(0, 0, 1, 0))).Any();
        }
    }
}