using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Infrastructure;

namespace Models.SysModels
{
    public class SysController : IUserDictionary
    {
        public SysController()
        {
            Id = Guid.NewGuid().ToString();
            SystemId = "000";
            TargetBlank = false;
            ControllerName = "Index";
            ActionName = "Index";
            Display = true;
            Enable = true;
            Ico = "fa-list-ul";
            SysActionsId=new List<string>();
        }


        [Key]
        [ScaffoldColumn(false)]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [Display(Name = "Area")]
        [Required]
        [ForeignKey("SysArea")]
        public string SysAreaId { get; set; }

        [ScaffoldColumn(false)]
        public virtual SysArea SysArea { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string ControllerName { get; set; }

        [MaxLength(50)]
        public string ActionName { get; set; }

        [MaxLength(50)]
        public string Parameter { get; set; }

        [MaxLength(50)]
        [Required]
        public string SystemId { get; set; }

        public bool Display { get; set; }

        public bool TargetBlank { get; set; }

        [DataType("Ico")]
        public string Ico { get; set; }

        [Display(Name = "Action")]
        [DataType("MultiSelectList")]
        //[Required]
        public List<string> SysActionsId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<SysControllerSysAction> SysControllerSysActions { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }

        public bool Enable { get; set; }
    }
}