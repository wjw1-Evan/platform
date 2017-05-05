using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysEnterpriseSysUserService : RepositoryBase<SysEnterpriseSysUser>, ISysEnterpriseSysUserService
    {
        public SysEnterpriseSysUserService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

      


    }
}