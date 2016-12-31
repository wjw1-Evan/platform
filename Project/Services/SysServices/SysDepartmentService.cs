using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysDepartmentService : RepositoryBase<SysDepartment>, ISysDepartmentService
    {
        public SysDepartmentService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}