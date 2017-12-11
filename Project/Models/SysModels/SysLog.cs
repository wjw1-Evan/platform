using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    public class SysLog : DbSetBaseId
    {
        public SysLog()
        {
            CreatedDateTime = DateTimeOffset.Now;
            MachineName = Environment.MachineName;
        }

        [MaxLength(128)]
        public string MachineName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Log { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        [Index]
        [Required]
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}