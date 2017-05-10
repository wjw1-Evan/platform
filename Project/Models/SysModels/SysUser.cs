using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Models.SysModels
{


    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class SysUser : IdentityUser, IDbSetBase
    {
        public SysUser()
        {
            CreatedDate = DateTimeLocal.NowDate;
            CreatedDateTime = DateTimeLocal.Now;
            Deleted = false;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SysUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SysUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

        [MaxLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        [MaxLength(300)]
        public string Picture { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        [MaxLength(50)]
        public string CreatedDate { get; set; }
        
        /// <summary>
        /// 创建日期时间
        /// </summary>
        [Editable(false)]
        [Index]
        [Required]
        public DateTime CreatedDateTime { get; set; }

        [Editable(false)]
        [DataType(DataType.DateTime)]
        public string UpdatedDate { get; set; }

        [Editable(false)]
        [MaxLength(128)]
        public string CreatedBy { get; set; }

        [Editable(false)]
        [MaxLength(128)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("UpdatedBy")]
        public virtual SysUser UserUpdatedBy { get; set; }


        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [ScaffoldColumn(false)]
        [Index(IsClustered = false)]
        public bool Deleted { get; set; }

        [MaxLength(128)]
        [ScaffoldColumn(false)]
        public string CurrentEnterpriseId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; } = new List<SysEnterpriseSysUser>();

        [ScaffoldColumn(false)]
        public virtual ICollection<SysDepartmentSysUser> SysDepartmentSysUsers { get; set; } = new List<SysDepartmentSysUser>();

    

   
    }
    
}
