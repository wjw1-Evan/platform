using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Infrastructure;

namespace Models.SysModels
{
  
    /// <summary>
    /// 组织架构
    /// </summary>
    public class SysDepartment : DbSetBase,IUserDictionary
    {
        public SysDepartment()
        {

            SystemId = "000";
            Enable = true;
        }

        [MaxLength(40)]
        [Required]
        
        public string Name { get; set; }

        [MaxLength(30)]
        [Required]
        public string SystemId { get; set; }

        public bool Enable { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysDepartmentSysUser> SysDepartmentSysUsers { get; set; }
    }
}