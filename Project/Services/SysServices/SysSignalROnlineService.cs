using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysSignalROnlineService : RepositoryBase<SysSignalROnline>, ISysSignalROnlineService
    {
        public SysSignalROnlineService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}