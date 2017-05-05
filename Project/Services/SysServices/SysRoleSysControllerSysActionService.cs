using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysRoleSysControllerSysActionService : RepositoryBase<SysRoleSysControllerSysAction>,
        ISysRoleSysControllerSysActionService
    {
        public SysRoleSysControllerSysActionService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}