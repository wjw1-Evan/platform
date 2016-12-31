using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Models.SysModels
{
    public class SysEnterprise 
    {
        public SysEnterprise()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [MaxLength(200)]
        [Required]

        public string EnterpriseName { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; }
    }
}