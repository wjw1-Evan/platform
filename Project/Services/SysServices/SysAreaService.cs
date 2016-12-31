using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysAreaService : RepositoryBase<SysArea>, ISysAreaService
    {
        public SysAreaService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

   

    }
}