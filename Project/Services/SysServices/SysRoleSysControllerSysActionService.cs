using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysRoleSysControllerSysActionService : RepositoryBase<SysRoleSysControllerSysAction>,
        ISysRoleSysControllerSysActionService
    {
        public SysRoleSysControllerSysActionService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}