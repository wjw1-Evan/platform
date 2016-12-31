using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysRoleSysControllerSysAction 
    {

        public SysRoleSysControllerSysAction()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

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