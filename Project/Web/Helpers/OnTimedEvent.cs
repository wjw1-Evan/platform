using System;
using System.Configuration;
using Common;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using Web.Extensions;

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
        /// <param name="state"></param>
        /// <returns></returns>
        void Run(object state);
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
        /// <param name="state"></param>
        /// <returns></returns>
        public void Run(object state)
        {
            lock (state)
            {
                if (int.TryParse(ConfigurationManager.AppSettings["LogValidity"], out int logValidity))
                {
                    //清理过期用户操作日志  限制一下每次删除的数量
                    try
                    {
                        var re1 = _sysUserLogService.SqlCommand(
                            "DELETE TOP(10000) FROM SysUserLogs WHERE createddatetime<{0}",
                            DateTimeOffset.Now.AddDays(-logValidity));

                        if (re1 > 0)
                        {
                            _iSysLogService.Add(new SysLog {Log = "清理超过" + logValidity + "天用户操作日志" + re1 + "行"});
                            _unitOfWork.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        _iSysLogService.Add(new SysLog
                        {
                            Log = "日志清理功能出现故障(Exception)：" + e.GetInnerException()
                        });
                        _unitOfWork.Commit();
                    }

                    //清理过期系统日志
                    try
                    {
                        var re3 = _iSysLogService.SqlCommand("DELETE TOP(10000) FROM SysLogs WHERE createddatetime<{0}",
                            DateTimeOffset.Now.AddDays(-logValidity));

                        if (re3 > 0)
                        {
                            _iSysLogService.Add(new SysLog {Log = "清理超过" + logValidity + "天系统日志" + re3 + "行"});
                            _unitOfWork.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        _iSysLogService.Add(new SysLog
                        {
                            Log = "日志清理功能出现故障(Exception)：" + e.GetInnerException()
                        });
                        _unitOfWork.Commit();
                    }
                }
            }
        }
    }
}