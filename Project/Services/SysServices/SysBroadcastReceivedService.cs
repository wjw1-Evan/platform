using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysBroadcastReceivedService : RepositoryBase<SysBroadcastReceived>, ISysBroadcastReceivedService
    {
        public SysBroadcastReceivedService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}