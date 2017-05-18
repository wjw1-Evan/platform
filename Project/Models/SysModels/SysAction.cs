using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Infrastructure;

namespace Models.SysModels
{

    public class SysAction : DbSetBaseId, IUserDictionary
    {
        public SysAction()
        {
            SystemId = "000";
            Enable = true;
        }

        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [MaxLength(40)]
        [Required]
        public string ActionName { get; set; }

        [MaxLength(50)]
        [Required]
        public string SystemId { get; set; }

        public bool System { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysControllerSysAction> SysControllerSysActions { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }

        public bool Enable { get; set; }

    }
}