using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysBroadcastReceived : DbSetBase//DbSetEnterpriseBase
    {
        [MaxLength(128)]
        [Required]
        [ForeignKey("SysBroadcast")]
        public string SysBroadcastId { get; set; }

        public virtual SysBroadcast SysBroadcast { get; set; }
    }
}
