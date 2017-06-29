using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using IServices.ISysServices;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private ISysUserLogService _ISysUserLogService;

        public HomeController(ISysUserLogService iSysUserLogService)
        {
            _ISysUserLogService = iSysUserLogService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Index", new { area = "Platform" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            var aa = _ISysUserLogService.SqlQuery<int>("select count(*) from sysuserlogs11") ;
            
            return Content(aa.First().ToString());
        }
    }
}
