using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysDepartmentSysUserService : RepositoryBase<SysDepartmentSysUser>, ISysDepartmentSysUserService
    {
        public SysDepartmentSysUserService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}