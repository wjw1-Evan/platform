using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using Web.Helpers;
using System.Linq.Dynamic;
using IServices.ISysServices;

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
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1, bool search = false)
        {
            var model =
                _sysLogService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Log,
                                         a.CreatedDateTime
                                     }).Search(keyword);
            if (search)
            {
                model = model.Search(Request.QueryString);
            }

            model = !string.IsNullOrEmpty(ordering) ? model.OrderBy(ordering, null) : model.OrderByDescending(a => a.CreatedDateTime);

            return View(model.ToPagedList(pageIndex));
        }

        // 导出全部数据
        // GET: /Platform/SysHelp/Report       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReportResult Report()
        {
            var model = _sysLogService.GetAll().Select(
                                     a =>
                                     new
                                     {
                                         a.Log,
                                         a.CreatedDateTime
                                     });
            var report = new Report(model.ToReportSource());
         
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }
        
    }


}