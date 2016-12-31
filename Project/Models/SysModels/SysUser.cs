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
    /// <summary>
    /// 性别类型
    /// </summary>
    public enum SexType
    {
        男 = 1,
        女 = 2
    }

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        企业账户 = 1
    }


    /// <summary>
    /// 审核状态
    /// </summary>
    public enum CheckStatus
    {
        未申请 = 1,
        审核中 = 2,
        未通过 = 3,
        已通过 = 4
    }

    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class SysUser : IdentityUser, IDbSetBase
    {
        public SysUser()
        {
            CreatedDate = DateTimeLocal.Now;
            Deleted = false;
            AccountType = SysModels.AccountType.企业账户;
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


        [Index(IsUnique = true)]
        public override string UserName { get; set; }


        [MaxLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// 头像URL
        /// </summary>
        [MaxLength(300)]
        public string Picture { get; set; }

        [Editable(false)]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Editable(false)]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

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

        [Display(Name = "账户类型")]

        public AccountType? AccountType { get; set; }

        [MaxLength(128)]
        [ScaffoldColumn(false)]
        public string CurrentEnterpriseId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; } = new List<SysEnterpriseSysUser>();

        [ScaffoldColumn(false)]
        public virtual ICollection<SysDepartmentSysUser> SysDepartmentSysUsers { get; set; } = new List<SysDepartmentSysUser>();

    }

    /// <summary>
    /// 用户积分
    /// </summary>
    public class UserPoint : DbSetBase
    {
        [Required, MaxLength(128), ForeignKey("SysUser")]
        public string SysUserId { get; set; }

        [ScaffoldColumn(false)]
        public virtual SysUser SysUser { get; set; }

        public string PointSystemCode { get; set; } //UserPointSetting.SystemId
        public int Point { get; set; }

        /// <summary>
        /// 关联的对象ID，撤销积分操作时使用
        /// </summary>
        [MaxLength(128)]
        public string ItemId { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
    /// <summary>
    /// 积分操作类型
    /// </summary>
    public enum UserPointRestriction
    {
        没有限制 = 1,
        只奖励一次 = 10,
        至多n积分 = 20,
        每天至多n次 = 30,
        每天至多n积分 = 40
    }

    /// <summary>
    /// 积分规则设定
    /// </summary>
    public class UserPointSetting : DbSetBase
    {
        public UserPointSetting()
        {
            Enabled = true;
        }

        [MaxLength(128), Required(ErrorMessage = "请输入{0}")]
        public string SystemId { get; set; }
        [MaxLength(200), Required(ErrorMessage = "请输入{0}")]
        public string Description { get; set; }
        [Display(Name = "PointValue"), Required(ErrorMessage = "请输入{0}")]
        public int PointValue { get; set; }
        [Display(Name = "操作类型"), Required(ErrorMessage = "请输入{0}")]
        public UserPointRestriction UserPointRestriction { get; set; }
        [Display(Name = "UserPointRestrictionValue", Description = "UserPointRestrictionValue_Description")]
        public int UserPointRestrictionValue { get; set; }//限制值

        public bool Enabled { get; set; }
    }

    /// <summary>
    /// 用户等级设置
    /// </summary>
    public class UserGradeConfiguration : DbSetBase
    {
        [Required(ErrorMessage = "请输入等级"), Display(Name = "等级")]
        public int Level { get; set; }
        [Required(ErrorMessage = "请输入积分下限"), Display(Name = "积分下限")]
        public int MinPoints { get; set; }

        [Required(ErrorMessage = "请输入积分上限"), Display(Name = "积分上限")]
        public int MaxPoints { get; set; }

        [MaxLength(200), DataType(DataType.MultilineText), Required(ErrorMessage = "等级权限"), Display(Name = "等级权限")]
        public string Description4Rights { get; set; }
    }

    /// <summary>
    /// 实名认证状态
    /// </summary>
    public enum RealNameCertificationStatus
    {
        未申请 = 1,
        已申请 = 2,
        已通过 = 3,
        未通过 = 4
    }

}
