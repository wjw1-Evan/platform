using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Infrastructure;

namespace Models.SysModels
{
    public class SysArea : IUserDictionary
    {
        public SysArea()
        {
            Id = Guid.NewGuid().ToString();
            SystemId = "000";
            Enable = true;
        }


        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

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