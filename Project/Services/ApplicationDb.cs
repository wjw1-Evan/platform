using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.SysModels;

namespace Services
{
    public sealed class ApplicationDbContext : IdentityDbContext<SysUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
           
        }
      

        #region //系统表
    
        public DbSet<SysEnterprise> SysEnterprises { get; set; }

        public DbSet<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; }

        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<SysArea> SysAreas { get; set; }
        public DbSet<SysController> SysControllers { get; set; }
        public DbSet<SysControllerSysAction> SysControllerSysActions { get; set; }
        public DbSet<SysAction> SysActions { get; set; }
        public DbSet<SysRoleSysControllerSysAction> SysRoleSysControllerSysActions { get; set; }
        public DbSet<SysHelp> SysHelps { get; set; }
        public DbSet<SysHelpClass> SysHelpClasses { get; set; }

        public DbSet<SysUserLog> SysUserLogs { get; set; }

        public DbSet<SysKeyword> SysKeywords { get; set; }//用户所有搜索记录

        //系统消息
        public DbSet<SysBroadcast> SysMessages { get; set; }

        //系统消息读取记录
        public DbSet<SysBroadcastReceived> SysBroadcastReceiveds { get; set; }

        public DbSet<SysSignalR> SysSignalRs { get; set; }

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
        public DbSet<SysDepartmentSysUser> SysDepartmentSysUser { get; set; }



        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //为表生成 基本的存储过程 Insert Update Delete
            modelBuilder.Types().Configure(a => a.MapToStoredProcedures());

            base.OnModelCreating(modelBuilder);
        }

        public int Commit()
        {
            return SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return SaveChangesAsync();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        //用户表
    }
}