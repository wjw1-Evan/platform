using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysUserCapitalService : RepositoryBase<SysUserCapital>, ISysUserCapitalService
    {
        public SysUserCapitalService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }



    }
}