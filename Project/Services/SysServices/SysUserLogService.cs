using System;
using System.Configuration;
using System.Data.Entity;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysUserLogService : RepositoryBase<SysUserLog>, ISysUserLogService
    {
        public SysUserLogService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
     
    
    }

    
}