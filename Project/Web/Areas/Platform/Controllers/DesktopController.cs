using IServices.Infrastructure;
using IServices.ISysServices;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Areas.Platform.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class DesktopController : Controller
    {

        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysUserLogService _iSysUserLogService;
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUnitOfWork"></param>
        /// <param name="iSysUserLogService"></param>
        public DesktopController(IUnitOfWork iUnitOfWork, ISysUserLogService iSysUserLogService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iSysUserLogService = iSysUserLogService;
           
        }

        /// <summary>
        /// </summary>



        // GET: Platform/Index
        /// <summary>
        /// 
        /// </summary>

        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            //近十天用户活动次数
            ViewBag.SysUserLogCountDay = _iSysUserLogService.GetAll().GroupBy(a => a.CreatedDate).Select(a => new { a.Key, Count = a.Count() }).OrderBy(a => a.Key).ToDictionaryAsync(a => a.Key, a => a.Count).Result;

            //执行速度
            ViewBag.SysUserLogDayDuration = _iSysUserLogService.GetAll().GroupBy(a => a.CreatedDate).Select(a => new { a.Key, Duration = Math.Round(a.Average(b => b.Duration), 3) }).OrderBy(a => a.Key).ToDictionaryAsync(a => a.Key, b => b.Duration).Result;


         

            return View();
        }
        
    }
}
