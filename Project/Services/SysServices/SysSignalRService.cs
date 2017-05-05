using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysSignalRService : RepositoryBase<SysSignalR>, ISysSignalRService
    {
        public SysSignalRService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }


    }
}