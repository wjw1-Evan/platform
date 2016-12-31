using System.ComponentModel.DataAnnotations;

namespace Models.SysModels
{
    public class SysSignalROnline:DbSetBase
    {
        [MaxLength(100)]
        public string ConnectionId { get; set; }

        [MaxLength(100)]
        public string GroupId { get; set; }
    }
}