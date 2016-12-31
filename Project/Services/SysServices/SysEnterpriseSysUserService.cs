using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysEnterpriseSysUserService : RepositoryBase<SysEnterpriseSysUser>, ISysEnterpriseSysUserService
    {
        public SysEnterpriseSysUserService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

      


    }
}