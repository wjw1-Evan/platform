using System;
using System.Configuration;
using System.Data.Objects.SqlClient;
using System.Linq;
using Common;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;

namespace Services.SysServices
{
    public class SysUserLogService : RepositoryBase<SysUserLog>, ISysUserLogService
    {
        public SysUserLogService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
        public override IQueryable<SysUserLog> GetAll(bool containsDeleted = false, bool allEnt = false)
        {
            return base.GetAll(containsDeleted, allEnt).OrderByDescending(a => a.CreatedDate);
        }

        public void DeleteExpiredData()
        {
            //只保留一定数量的日志,根据web.config中的设置值，默认单位：天。
            if (ConfigurationManager.AppSettings["LogValidity"] == null) return;

            var logValidity = Convert.ToDouble(ConfigurationManager.AppSettings["LogValidity"]);

            Delete(a => SqlFunctions.DateDiff("d", a.CreatedDate, DateTimeLocal.Now) > logValidity);
        }
    }

    
}