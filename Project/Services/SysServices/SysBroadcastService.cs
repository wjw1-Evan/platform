using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{

    public class SysBroadcastService : RepositoryBase<SysBroadcast>, ISysBroadcastService
    {

        public SysBroadcastService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
         
        }

      
    }
}