using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysDepartmentSysUser : DbSetBaseId
    {
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