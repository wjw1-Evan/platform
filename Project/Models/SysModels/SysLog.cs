using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.Infrastructure;

namespace Models.SysModels
{
    public class SysLog : DbSetBaseId
    {
        public SysLog()
        {
            CreatedDateTime = DateTimeLocal.Now;
        }

        public string Log { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        [Index]
        [Required]
        public DateTime CreatedDateTime { get; set; }
    }
}