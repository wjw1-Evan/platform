using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.SysModels;

namespace Models
{
    public interface IDbSetBaseId
    {
        string Id { get; set; }
    }

    public interface IDbSetBase
    {
        string CreatedDate { get; set; }

        DateTimeOffset CreatedDateTime { get; set; }

        DateTimeOffset? UpdatedDateTime { get; set; }

        string CreatedBy { get; set; }

        string UpdatedBy { get; set; }

        bool Deleted { get; set; }
    }
    /// <summary>
    /// 企业关联
    /// </summary>
    public interface IEnterprise
    {
        [MaxLength(128)]
        string EnterpriseId { get; set; }
    }

    public abstract class DbSetBaseId : IDbSetBaseId
    {
        protected DbSetBaseId()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }
    }

    /// <summary>
    /// 企业关联基础表
    /// </summary>
    public abstract class DbSetBase : DbSetBaseId, IDbSetBase, IEnterprise
    {
        protected DbSetBase()
        {
            CreatedDate = DateTimeOffset.Now.ToString();
            CreatedDateTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [ScaffoldColumn(false)]
        [MaxLength(24)]
        [Index]
        [DataType(DataType.Date)]
        [Required]
        public string CreatedDate { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        [Editable(false)]
        [Index]
        [Required]
        [Index("IX_EnterpriseId-Deleted", 22)]
        public DateTimeOffset CreatedDateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Editable(false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ScaffoldColumn(false)]
        [ForeignKey("CreatedBy")]
        public virtual SysUser UserCreatedBy { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        [Editable(false)]
        public DateTimeOffset? UpdatedDateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Editable(false)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [ScaffoldColumn(false)]
        [ForeignKey("UpdatedBy")]
        public virtual SysUser UserUpdatedBy { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Description = "Remark_Description")]
        public string Remark { get; set; }

        /// <summary>
        /// 记录标记删除
        /// </summary>
        [ScaffoldColumn(false)]
        [Index]
        [Index("IX_EnterpriseId-Deleted", 20)]
        public bool Deleted { get; set; }

        /// <summary>
        /// 数据所在企业ID
        /// </summary>
        [MaxLength(128)]
        [ScaffoldColumn(false)]
        [Index]
        [Index("IX_EnterpriseId-Deleted", 21)]
        public string EnterpriseId { get; set; }
    }
}