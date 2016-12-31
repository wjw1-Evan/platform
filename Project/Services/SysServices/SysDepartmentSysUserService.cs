using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysDepartmentSysUserService : RepositoryBase<SysDepartmentSysUser>, ISysDepartmentSysUserService
    {
        public SysDepartmentSysUserService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}