using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    /// <summary>
    /// 记录后台用户操作痕迹
    /// </summary>
    public class SysUserLog : DbSetBase
    {
        [ForeignKey("SysControllerSysAction")]
        public string SysControllerSysActionId { get; set; }

        public virtual SysControllerSysAction SysControllerSysAction { get; set; }

        [MaxLength(40)]
        public string SysArea { get; set; }

        [MaxLength(40)]
        public string SysController { get; set; }

        [MaxLength(40)]
        public string SysAction { get; set; }

        [MaxLength(128)]
        public string RecordId { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual SysUser SysUser { get; set; }

        [MaxLength(100)]
        public string Ip { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [MaxLength(64)]
        public string RequestType { get; set; }

        public double Duration { get; set; }
    }

}