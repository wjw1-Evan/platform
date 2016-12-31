using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysDepartmentSysUser
    {
        public SysDepartmentSysUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [ForeignKey("SysDepartment")]
        [Required]
        public string SysDepartmentId { get; set; }

        public virtual SysDepartment SysDepartment { get; set; }
     
        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }

        public virtual SysUser SysUser { get; set; }

  
    }
}