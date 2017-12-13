using EFSecondLevelCache;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Services.SysServices
{
    public class SysRoleService : RepositoryBase<SysRole>, ISysRoleService
    {
        public SysRoleService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

        public Task<bool> CheckSysUserSysRoleSysControllerSysActions(string userid, string area,
            string action,
            string controller)
        {

            //var re=GetAll(a => a.Users.Any(b => b.UserId.Equals(userid)) &&
            //                             a.SysRoleSysControllerSysActions.Any(
            //                                 b =>
            //                                     b.SysControllerSysAction.SysController.SysArea.AreaName.Equals(area) &&
            //                                     b.SysControllerSysAction.SysController.ControllerName.Equals(controller) &&
            //                                     b.SysControllerSysAction.SysAction.ActionName.Equals(action))).Select(a => a.Id).FromCacheAsync(CachePolicy.WithSlidingExpiration(new TimeSpan(0, 0, 1, 0))).Result.Any();




            var re =
                GetAll(a => a.Users.Any(b => b.UserId.Equals(userid)) &&
                            a.SysRoleSysControllerSysActions.Any(
                                b =>
                                    b.SysControllerSysAction.SysController.SysArea.AreaName.Equals(area) &&
                                    b.SysControllerSysAction.SysController.ControllerName.Equals(controller) &&
                                    b.SysControllerSysAction.SysAction.ActionName.Equals(action))).Cacheable().AnyAsync();


            return re;

        }
    }
}
