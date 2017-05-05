using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysEnterpriseService : RepositoryBase<SysEnterprise>, ISysEnterpriseService
    {
        public SysEnterpriseService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}