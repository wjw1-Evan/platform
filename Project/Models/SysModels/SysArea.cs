using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Infrastructure;

namespace Models.SysModels
{
    public class SysArea : DbSetBaseId, IUserDictionary
    {
        public SysArea()
        {
            SystemId = "000";
            Enable = true;
        }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [MaxLength(40)]
        [Required]
        public string AreaName { get; set; }

        [MaxLength(30)]
        [Required]
        public string SystemId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysController> SysControllers { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }

        public bool Enable { get; set; }

    }
}