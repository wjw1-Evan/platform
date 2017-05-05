using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysSignalROnlineService : RepositoryBase<SysSignalROnline>, ISysSignalROnlineService
    {
        public SysSignalROnlineService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}