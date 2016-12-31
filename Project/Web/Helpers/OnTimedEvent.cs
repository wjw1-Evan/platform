using System.Threading.Tasks;
using System.Timers;
using IServices.Infrastructure;
using IServices.ISysServices;

namespace Web.Helpers
{
    public interface IOnTimedEvent
    {
        Task Run(object source, ElapsedEventArgs elapsedEventArgs);
    }

    public class OnTimedEvent : IOnTimedEvent
    {
        private readonly ISysUserLogService _sysUserLogService;
        private readonly IUnitOfWork _unitOfWork;

        public OnTimedEvent(IUnitOfWork unitOfWork, ISysUserLogService sysUserLogService)
        {
            _unitOfWork = unitOfWork;
            _sysUserLogService = sysUserLogService;
        }

        public async Task Run(object source, ElapsedEventArgs elapsedEventArgs)
        {
            //清理过期用户操作日志
            _sysUserLogService.DeleteExpiredData();
            await _unitOfWork.CommitAsync();
        }
    }
}