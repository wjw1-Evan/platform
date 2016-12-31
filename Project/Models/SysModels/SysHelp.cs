using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.SysModels
{
  
    public class SysHelp : DbSetBase
    {
        public SysHelp()
        {
            Sort = 0;
        }


        [Required]
        [ForeignKey("SysHelpClass")]
        [DataType("SystemId")]
        [Display(Name = "Class")]
        public string SysHelpClassId { get; set; }


        [ScaffoldColumn(false)]
        public virtual SysHelpClass SysHelpClass { get; set; }


        [MaxLength(100)]

        [Required]
        public string Title { get; set; }

        [MaxLength]
        [DataType(DataType.Html)]
        [Required]
    

        public string Content { get; set; }

        public int Sort { get; set; }

    }
}