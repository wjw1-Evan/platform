using System;
using System.Data.Entity;
using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;
using System.Threading.Tasks;


namespace Services.SysServices
{

    public class SysUserService : RepositoryBase<SysUser>, ISysUserService
    {

        public SysUserService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }

    



}