using System;
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Timers;
using Common;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOnTimedEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="elapsedEventArgs"></param>
        /// <returns></returns>
        Task Run(object source, ElapsedEventArgs elapsedEventArgs);
    }

    /// <summary>
    /// 
    /// </summary>
    public class OnTimedEvent : IOnTimedEvent
    {
        private readonly ISysUserLogService _sysUserLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysLogService _iSysLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sysUserLogService"></param>
        /// <param name="iSysLogService"></param>
        public OnTimedEvent(IUnitOfWork unitOfWork, ISysUserLogService sysUserLogService, ISysLogService iSysLogService)
        {
            _unitOfWork = unitOfWork;
            _sysUserLogService = sysUserLogService;
            _iSysLogService = iSysLogService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="elapsedEventArgs"></param>
        /// <returns></returns>
        public async Task Run(object source, ElapsedEventArgs elapsedEventArgs)
        {

            if (int.TryParse(ConfigurationManager.AppSettings["LogValidity"], out int logValidity))
            {
                //清理过期用户操作日志
                try
                {
                    var re1 = await _sysUserLogService.SqlCommandAsync("DELETE FROM SysUserLogs WHERE createddatetime<{0}", DateTimeLocal.Now.AddDays(-logValidity));

                    if (re1 > 0)
                    {
                        _iSysLogService.Add(new SysLog { Log = "清理超过" + logValidity + "天用户操作日志" + re1 + "行" });
                        await _unitOfWork.CommitAsync();
                    }
                }
                catch (AggregateException e)
                {
                    foreach (var innerException in e.InnerExceptions)
                    {
                        _iSysLogService.Add(new SysLog { Log = innerException.Message });
                        await _unitOfWork.CommitAsync();
                    }
                }


                //清理过期系统日志
                try
                {
                    var re3 = await _iSysLogService.SqlCommandAsync("DELETE FROM SysLogs WHERE createddatetime<{0}", DateTimeLocal.Now.AddDays(-logValidity));

                    if (re3 > 0)
                    {
                        _iSysLogService.Add(new SysLog { Log = "清理超过" + logValidity + "天系统日志" + re3 + "行" });
                        await _unitOfWork.CommitAsync();
                    }
                }
                catch (AggregateException e)
                {
                    foreach (var innerException in e.InnerExceptions)
                    {
                        _iSysLogService.Add(new SysLog { Log = innerException.Message });
                        await _unitOfWork.CommitAsync();
                    }
                }


            }
        }
    }
}