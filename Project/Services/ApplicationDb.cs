using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Common;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.SysModels;
using Models.TaskModels;

namespace Services
{
    public sealed class ApplicationDbContext : IdentityDbContext<SysUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
           // // 更新数据库到最新的版本
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Services.Migrations.Configuration>());
            Database.CommandTimeout = 60;
            Database.Log = log => Log.Write("EF", log);
        }

        #region 任务中心

        public DbSet<TaskCenter> TaskCenters { get; set; }

        #endregion

        #region 系统表

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysEnterprise> SysEnterprises { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysRole> SysRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<IdentityUserRole> UserRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysArea> SysAreas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysController> SysControllers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysControllerSysAction> SysControllerSysActions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysAction> SysActions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysRoleSysControllerSysAction> SysRoleSysControllerSysActions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysHelp> SysHelps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysHelpClass> SysHelpClasses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysUserLog> SysUserLogs { get; set; }

        /// <summary>
        /// 用户所有搜索记录
        /// </summary>
        public DbSet<SysKeyword> SysKeywords { get; set; }

        /// <summary>
        /// 系统消息
        /// </summary>
        public DbSet<SysBroadcast> SysMessages { get; set; }

        /// <summary>
        /// 系统消息读取记录
        /// </summary>
        public DbSet<SysBroadcastReceived> SysBroadcastReceiveds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysSignalR> SysSignalRs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<SysSignalROnline> SysSignalROnlines { get; set; }

        /// <summary>
        /// 验证码存储
        /// </summary>
        public DbSet<VerifyCode> VerifyCodes { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public DbSet<SysDepartment> SysDepartments { get; set; }

        /// <summary>
        /// 用户关联部门
        /// </summary>
        public DbSet<SysDepartmentSysUser> SysDepartmentSysUsers { get; set; }

        /// <summary>
        /// 系统日志
        /// </summary>
        public DbSet<SysLog> SysLogs { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(38, 4));

            //为表生成 基本的存储过程 Insert Update Delete      SQL Server Compact 不支持
            modelBuilder.Types().Configure(a => a.MapToStoredProcedures());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        //用户表
    }
}