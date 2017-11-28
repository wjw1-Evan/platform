using IServices.Infrastructure;
using IServices.ISysServices;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SysLogController : Controller
    {
        private readonly ISysLogService _sysLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysLogService"></param>
        /// <param name="unitOfWork"></param>
        public SysLogController(ISysLogService sysLogService, IUnitOfWork unitOfWork)
        {
            _sysLogService = sysLogService;
        }

        //
        // GET: /Platform/SysLog/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <param name="export"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1, bool export = false, bool search = false)
        {
            var model =
                _sysLogService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.MachineName,
                                         a.Log,
                                         a.CreatedDateTime
                                     }).Search(keyword);
            if (search)
            {
                model = model.Search(Request.QueryString);
            }

            model = !string.IsNullOrEmpty(ordering) ? model.OrderBy(ordering, null) : model.OrderByDescending(a => a.CreatedDateTime);
            if (export)
            {
                return model.ToExcelFile();
            }
            return View(model.ToPagedList(pageIndex));
        }


    }


}
