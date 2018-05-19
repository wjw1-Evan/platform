using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lucene.Net.Linq.Mapping;
using Lucene.Net.Analysis;


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
        [Field(Analyzer = typeof(KeywordAnalyzer))]
        public string Title { get; set; }

        [MaxLength]
        [DataType(DataType.Html)]
        [Required]
        [Field(Analyzer = typeof(KeywordAnalyzer))]
        public string Content { get; set; }

        public int Sort { get; set; }

    }
}