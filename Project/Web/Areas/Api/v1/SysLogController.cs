using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using System.Web;
using System.Web.Http;

namespace Web.Areas.Api.v1
{
    /// <inheritdoc />
    /// <summary>
    /// 默认
    /// </summary>
    [Route("api/v1/syslog")]
    public class SysLogController : ApiController
    {
        private readonly ISysLogService _iSysLogService;
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSysLogService"></param>
        /// <param name="iUnitOfWork"></param>
        public SysLogController(ISysLogService iSysLogService, IUnitOfWork iUnitOfWork)
        {
            _iSysLogService = iSysLogService;
            _iUnitOfWork = iUnitOfWork;
        }

        // POST: api/Default
        /// <summary>
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="log"></param>
        public void Post(LogLevels logLevel, string log)
        {
            _iSysLogService.Add(new SysLog()
            {
                MachineName = HttpContext.Current.Request.UserHostAddress,
                LogLevel = logLevel,
                Log = log,

            });
            _iUnitOfWork.CommitAsync();
        }


    }
}
