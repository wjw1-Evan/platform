using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysActionService : RepositoryBase<SysAction>, ISysActionService
    {
        public SysActionService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}