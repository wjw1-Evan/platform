using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.SysModels;

namespace Models
{
    public interface IDbSetBase
    {
        string Id { get; set; }

        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }

        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        bool Deleted { get; set; }
    }
    /// <summary>
    /// 企业关联
    /// </summary>
    public interface IEnterprise
    {
        [MaxLength(100)]
        string EnterpriseId { get; set; }
    }
    /// <summary>
    /// 企业无关联基础表
    /// </summary>
    public abstract class DbSetBaseNoEnt : IDbSetBase
    {
        protected DbSetBaseNoEnt()
        {
            Id = Guid.NewGuid().ToString();

            CreatedDate = DateTimeLocal.Now;
            Deleted = false;
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [Editable(false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [Index(IsClustered = false)]
        public DateTime CreatedDate { get; set; }

        [Editable(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("CreatedBy")]
        public virtual SysUser UserCreatedBy { get; set; }

        [Editable(false)]
        [Index(IsClustered = false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        [Editable(false)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("UpdatedBy")]
        public virtual SysUser UserUpdatedBy { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Description = "Remark_Description")]
        public string Remark { get; set; }

        [ScaffoldColumn(false)]
        [Index(IsClustered = false)]
        public bool Deleted { get; set; }
        
    }
    /// <summary>
    /// 企业关联基础表
    /// </summary>
    public abstract class DbSetBase : IDbSetBase, IEnterprise
    {
        protected DbSetBase()
        {
            Id = Guid.NewGuid().ToString();
          
            CreatedDate = DateTimeLocal.Now;
            Deleted = false;
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [Editable(false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [Index(IsClustered = false)]
        public DateTime CreatedDate { get; set; }
       
        [Editable(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("CreatedBy")]
        public virtual SysUser UserCreatedBy { get; set; }

        [Editable(false)]
        [Index(IsClustered = false)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        [Editable(false)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("UpdatedBy")]
        public virtual SysUser UserUpdatedBy { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Description = "Remark_Description")]
        public string Remark { get; set; }

        [ScaffoldColumn(false)]
        [Index(IsClustered = false)]
        public bool Deleted { get; set; }

        /// <summary>
        /// 数据所在企业ID
        /// </summary>
        [MaxLength(128)]
        [ScaffoldColumn(false)]
        public string EnterpriseId { get; set; }
    }
}