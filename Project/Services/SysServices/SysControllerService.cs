using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysControllerService : RepositoryBase<SysController>, ISysControllerService
    {
        public SysControllerService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }


    }
}