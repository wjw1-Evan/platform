using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Models.SysModels;

namespace Models.TaskModels
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskTypes
    {
        撰写,
        开发,
        测试
    }

    /// <summary>
    /// 任务中心
    /// </summary>
    public class TaskCenter : DbSetBase
    {
        public TaskCenter()
        {

        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskTypes TaskType { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        [MaxLength]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Content { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [DataType("Files")]
        public string Files { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        [DataType(DataType.DateTime)]
        [MaxLength(64)]
        public string ScheduleEndTime { get; set; }

        /// <summary>
        /// 实际结束时间
        /// 填写该时间代表任务完成
        /// </summary>
        [DataType(DataType.DateTime)]
        [MaxLength(64)]
        public string ActualEndTime { get; set; }

        /// <summary>
        /// 持续时间
        /// </summary>
        [Range(typeof(decimal), "0", "24")]
        [Display(Description = "Hour")]
        public decimal Duration { get; set; }

        /// <summary>
        /// 任务执行人
        /// </summary>
        [ForeignKey("TaskExecutor")]
        [Display(Name = "TaskExecutor")]
        [Required]
        public string TaskExecutorId { get; set; }

        /// <summary>
        /// 任务执行人
        /// </summary>
        [ScaffoldColumn(false)]
        public virtual SysUser TaskExecutor { get; set; }

    }
}