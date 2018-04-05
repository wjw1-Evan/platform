using IServices.ISysServices;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SysUserLogController : Controller
    {
        private readonly ISysUserLogService _sysUserLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysUserLogService"></param>
        public SysUserLogController(ISysUserLogService sysUserLogService)
        {
            _sysUserLogService = sysUserLogService;
        }

        //
        // GET: /Platform/SysUserLog/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string keyword, string ordering, bool export = false, int pageIndex = 1, bool search = false)
        {
            var model =
                _sysUserLogService.GetAll()
                                  .Select(
                                      a =>
                                      new
                                      {
                                          a.SysUser.UserName,
                                          a.SysArea,
                                          a.SysController,
                                          a.SysAction,
                                          a.RecordId,
                                          a.ActionDuration,
                                          a.ViewDuration,
                                          a.Duration,
                                          a.RequestType,
                                          a.Ip,
                                          a.Url,
                                          a.CreatedDateTime,
                                      }).Search(keyword);
            if (search)
            {
                model = model.Search(Request.QueryString);
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }
            if (export)
            {
                return model.ToExcelFile();
            }

            return View(model.ToPagedList(pageIndex));
        }

    }
}
