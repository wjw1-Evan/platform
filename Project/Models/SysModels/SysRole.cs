using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Models.SysModels
{
    public class SysRole : IdentityRole, IEnterprise
    {
        public SysRole()
        {
            SysDefault = false;
            SystemId = "000";
        }

        [MaxLength(100)]
        [Required]
        public string RoleName { get; set; }

        public bool SysDefault { get; set; }

        [MaxLength(50)]
        [Required]
        public string SystemId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysRoleSysControllerSysAction> SysRoleSysControllerSysActions { get; set; }

        [MaxLength(128)]
        [ScaffoldColumn(false)]
        public string EnterpriseId { get; set; }
    }
}