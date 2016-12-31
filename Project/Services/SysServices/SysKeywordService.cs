using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysKeywordService : RepositoryBase<SysKeyword>, ISysKeywordService
    {
        public SysKeywordService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
               : base(databaseFactory, userInfo)
        {

        }




    }
}