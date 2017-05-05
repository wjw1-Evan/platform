using System.Data.Entity;
using IServices.ISysServices;
using IServices.ITaskServices;
using Models.TaskModels;
using Services.Infrastructure;

namespace Services.TaskServices
{
    public class TaskCenterService : RepositoryBase<TaskCenter>, ITaskCenterService
    {
        public TaskCenterService(DbContext databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}