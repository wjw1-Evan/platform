using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysHelpClassService : RepositoryBase<SysHelpClass>, ISysHelpClassService
    {
        public SysHelpClassService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}