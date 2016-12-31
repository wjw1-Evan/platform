using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SysModels
{
    /// <summary>
    /// 标签类型
    /// </summary>
    public enum KeywordType
    {
        /// <summary>
        /// 管理员定义关键词
        /// </summary>
        系统标签 = 0,

        /// <summary>
        /// 用户搜索关键字
        /// </summary>
        用户搜索 = 1 //用户搜索关键字
    }

    public class SysKeyword : DbSetBase
    {
        public SysKeyword()
        {
            Enable = true;
        }

        /// <summary>
        /// 关键词
        /// </summary>
        [Index]
        [Required]
        [MaxLength(40)]
        public string Keyword { get; set; }

        /// <summary>
        /// 搜索结果数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 搜索类型
        /// </summary>
        public KeywordType Type { get; set; }

        /// <summary>
        /// 是否启用该词在前台显示
        /// </summary>
        public bool Enable { get; set; }


    }
}