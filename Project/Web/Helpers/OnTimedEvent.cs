using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Timers;
using IServices.Infrastructure;
using IServices.ISysServices;

using Models.SysModels;

namespace Web.Helpers
{
    public interface IOnTimedEvent
    {
        void Run(object source, ElapsedEventArgs elapsedEventArgs);
    }

    public class OnTimedEvent : IOnTimedEvent
    {
     
        private readonly ISysUserLogService _sysUserLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysUserService _iSysUserService;
   


        public OnTimedEvent(IUnitOfWork unitOfWork, ISysUserLogService sysUserLogService, ISysUserService iSysUserService )
        {
            _unitOfWork = unitOfWork;
            _sysUserLogService = sysUserLogService;
            _iSysUserService = iSysUserService;
        }

        public void Run(object source, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                _sysUserLogService.DeleteExpiredData();
                _unitOfWork.CommitAsync();
                
            }
            catch (Exception )
            {
               
            }

        }
    }
}