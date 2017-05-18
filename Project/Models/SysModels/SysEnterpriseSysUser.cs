using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysEnterpriseSysUser : DbSetBaseId
    {

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