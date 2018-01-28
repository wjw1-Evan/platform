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
            LogLevel = 0;
        }

        /// <summary>
        /// 机器
        /// </summary>
        [MaxLength(128)]
        public string MachineName { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevels LogLevel { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Log { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        [Index]
        [Required]
        public DateTimeOffset CreatedDateTime { get; set; }
    }

    public enum LogLevels
    {
        /// <summary>
        /// 用于记录最详细的日志消息，通常仅用于开发阶段调试问题。这些消息可能包含敏感的应用程序数据，因此不应该用于生产环境。默认应禁用。举例：Credentials: {"User":"someuser", "Password":"P@ssword"}
        /// </summary>
        Trace = 0,
        /// <summary>
        /// 这种消息在开发阶段短期内比较有用。它们包含一些可能会对调试有所助益、但没有长期价值的信息。默认情况下这是最详细的日志。举例： Entering method Configure with flag set to true
        /// </summary>
        Debug = 1,
        /// <summary>
        /// 这种消息被用于跟踪应用程序的一般流程。与 Verbose 级别的消息相反，这些日志应该有一定的长期价值。举例： Request received for path /foo
        /// </summary>
        Information = 2,
        /// <summary>
        /// 当应用程序出现错误或其它不会导致程序停止的流程异常或意外事件时使用警告级别，以供日后调查。在一个通用的地方处理警告级别的异常。举例： Login failed for IP 127.0.0.1 或 FileNotFoundException for file foo.txt
        /// </summary>
        Warning = 3,
        /// <summary>
        /// 当应用程序由于某些故障停止工作则需要记录错误日志。这些消息应该指明当前活动或操作（比如当前的 HTTP 请求），而不是应用程序范围的故障。举例： Cannot insert record due to duplicate key violation
        /// </summary>
        Error = 4,
        /// <summary>
        /// 当应用程序或系统崩溃、遇到灾难性故障，需要立即被关注时，应当记录关键级别的日志。举例：数据丢失、磁盘空间不够等。
        /// </summary>
        Critical = 5
    }
}
