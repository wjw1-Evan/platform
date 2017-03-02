using System;
using System.ComponentModel.DataAnnotations;
using Models.TaskModels;

namespace Web.Areas.Platform.Models
{
    public class TaskCenterListModel: TaskCenter
    {
        public string UserName { get; set; }

        public new string TaskType { get; internal set; }

        public new string TaskExecutor { get; internal set; }
     
        public new string CreatedBy { get; internal set; }
   

    }

    public class TaskCenterEditModel : TaskCenter
    {
    

        [ScaffoldColumn(false)]
        public new decimal Duration { get; set; }

        [ScaffoldColumn(false)]
        public new decimal ActualEndTime { get; set; }
    }
}