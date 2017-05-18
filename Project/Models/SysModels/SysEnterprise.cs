using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Models.SysModels
{
    public class SysEnterprise : DbSetBaseId
    {

        [MaxLength(200)]
        [Required]

        public string EnterpriseName { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; }
    }
}