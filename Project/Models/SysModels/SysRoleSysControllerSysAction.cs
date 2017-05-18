using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysRoleSysControllerSysAction : DbSetBaseId
    {

        [ForeignKey("SysRole")]
        [Required]
        public string RoleId { get; set; }

        public virtual SysRole SysRole { get; set; }


        [ForeignKey("SysControllerSysAction")]
        [Required]
        public string SysControllerSysActionId { get; set; }

        public virtual SysControllerSysAction SysControllerSysAction { get; set; }
    }
}