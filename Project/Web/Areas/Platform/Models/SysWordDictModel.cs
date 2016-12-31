using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Platform.Models
{
    /// <summary>
    /// 系统分词词库
    /// </summary>
    public class SysWordDictModel
    {

        /// <summary>
        /// 
        /// </summary>
        public SysWordDictModel()
        {
            Ferquency = 0;
        }

        /// <summary>
        /// 关键词
        /// </summary>
        [Required]
        public string Word { get; set; }

        /// <summary>
        /// 频率 （注：还没明白有什么用）
        /// </summary>
        public int Ferquency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ScaffoldColumn(false)]
        public string Id { get; set; }

    }
}