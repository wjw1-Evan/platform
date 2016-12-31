using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysControllerSysActionService : RepositoryBase<SysControllerSysAction>, ISysControllerSysActionService
    {
        public SysControllerSysActionService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }


    }
}