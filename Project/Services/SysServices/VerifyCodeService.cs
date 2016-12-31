using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class VerifyCodeService :RepositoryBase<VerifyCode>,IVerifyCodeService
    {
        public VerifyCodeService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
}
