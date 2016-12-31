using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    /// <summary>
    /// 记录后台用户操作痕迹
    /// </summary>
    public class SysUserLog : DbSetBase
    {
      
        [Required]
        [ForeignKey("SysControllerSysAction")]
        public string SysControllerSysActionId { get; set; }

        public virtual SysControllerSysAction SysControllerSysAction { get; set; }

        [MaxLength(128)]
        public string RecordId { get; set; }


        [ForeignKey("CreatedBy")]
        public virtual SysUser SysUser { get; set; }

        [MaxLength(100)]
        [Required]
        public string Ip { get; set; }

        public string Url { get; set; }
    }
    
}