using IServices.ISysServices;
using IServices.ITaskServices;
using Models.TaskModels;
using Services.Infrastructure;

namespace Services.TaskServices
{
    public class TaskCenterService : RepositoryBase<TaskCenter>, ITaskCenterService
    {
        public TaskCenterService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    
    }
}