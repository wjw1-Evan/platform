using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysEnterpriseSysUser
    {
        public SysEnterpriseSysUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }
        

        [ForeignKey("SysEnterprise")]
        [Required]
        public string SysEnterpriseId { get; set; }
        [ScaffoldColumn(false)]
        public virtual SysEnterprise SysEnterprise { get; set; }

     
        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }
        [ScaffoldColumn(false)]
        public virtual SysUser SysUser { get; set; }

    }
}