using System;
using System.Configuration;
using System.Threading;
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
        /// <returns></returns>
        void Run(object source);
    }

    /// <summary>
    /// 
    /// </summary>
    public class OnTimedEvent : IOnTimedEvent
    {
        private readonly ISysUserLogService _sysUserLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysLogService _iSysLogService;
        private static int _inTimer = 0;

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
        /// <returns></returns>
        public void Run(object source)
        {
            if (Interlocked.Exchange(ref _inTimer, 1) != 0) return;

            if (int.TryParse(ConfigurationManager.AppSettings["LogValidity"], out int logValidity))
            {
                //清理过期用户操作日志  限制一下每次删除的数量
                try
                {
                    var re1 =  _sysUserLogService.SqlCommand("DELETE TOP(10000) FROM SysUserLogs WHERE createddatetime<{0}", DateTimeLocal.Now.AddDays(-logValidity));

                    if (re1 > 0)
                    {
                        _iSysLogService.Add(new SysLog { Log = "清理超过" + logValidity + "天用户操作日志" + re1 + "行" });
                         _unitOfWork.Commit();
                    }
                }
                catch (AggregateException e)
                {
                    foreach (var innerException in e.InnerExceptions)
                    {
                        _iSysLogService.Add(new SysLog { Log = innerException.Message });
                         _unitOfWork.Commit();
                    }
                }


                //清理过期系统日志
                try
                {
                    var re3 =  _iSysLogService.SqlCommand("DELETE TOP(10000) FROM SysLogs WHERE createddatetime<{0}", DateTimeLocal.Now.AddDays(-logValidity));

                    if (re3 > 0)
                    {
                        _iSysLogService.Add(new SysLog { Log = "清理超过" + logValidity + "天系统日志" + re3 + "行" });
                         _unitOfWork.Commit();
                    }
                }
                catch (AggregateException e)
                {
                    foreach (var innerException in e.InnerExceptions)
                    {
                        _iSysLogService.Add(new SysLog { Log = innerException.Message });
                         _unitOfWork.Commit();
                    }
                }

                Interlocked.Exchange(ref _inTimer, 0);
            }
        }
    }
}