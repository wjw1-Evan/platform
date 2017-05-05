using System.Data.Entity;
using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysHelpService : RepositoryBase<SysHelp>, ISysHelpService
    {
        public SysHelpService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

     
    }
}