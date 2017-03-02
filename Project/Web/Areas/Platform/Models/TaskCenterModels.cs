using System;
using System.ComponentModel.DataAnnotations;
using Models.TaskModels;

namespace Web.Areas.Platform.Models
{
    public class TaskCenterListModel
    {
        public string UserName { get; internal set; }

        public string TaskType { get; internal set; }

        public string TaskExecutor { get; internal set; }

        public string CreatedBy { get; internal set; }

        [DataType(DataType.Html)]
        public string Content { get; internal set; }

        [DataType("Files")]
        public string Files { get; internal set; }
        public string TaskExecutorId { get; internal set; }
        public decimal Duration { get; internal set; }
        public string ScheduleEndTime { get; internal set; }
        public string Id { get; internal set; }
        public string ActualEndTime { get; internal set; }
        public string Title { get; internal set; }
        public string CreatedDate { get; internal set; }
    }

    public class TaskCenterEditModel : TaskCenter
    {


        [ScaffoldColumn(false)]
        public decimal Duration { get; set; }

        [ScaffoldColumn(false)]
        public string ActualEndTime { get; set; }
    }
}