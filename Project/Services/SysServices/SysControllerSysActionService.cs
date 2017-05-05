using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysControllerSysActionService : RepositoryBase<SysControllerSysAction>, ISysControllerSysActionService
    {
        public SysControllerSysActionService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }


    }
}