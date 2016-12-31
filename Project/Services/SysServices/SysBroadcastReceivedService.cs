using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysBroadcastReceivedService : RepositoryBase<SysBroadcastReceived>, ISysBroadcastReceivedService
    {
        public SysBroadcastReceivedService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}