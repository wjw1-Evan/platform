using System;
using System.ComponentModel.DataAnnotations;

namespace Models.SysModels
{
    public class SysSignalR : DbSetBase
    {
        [MaxLength(100)]
        public string GroupId { get; set; }

        public Guid? UserId1 { get; set; } //收件人

        [MaxLength(100)]
        public string UserName { get; set; } //发件人

        [MaxLength(100)]
        public string UserName1 { get; set; } //收件人

        [MaxLength]
        public string Message { get; set; }
    }
}