using System.Linq;
using System.Web.Mvc;
using IServices.Infrastructure;
using Common;
using Web.Areas.Platform.Helpers;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using DoddleReport;
using DoddleReport.Web;
using Models.SysModels;
using IServices.ISysServices;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SysKeywordController : Controller
    {
        private readonly ISysKeywordService _sysKeywordService;
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysKeywordService"></param>
        /// <param name="iUnitOfWork"></param>
        public SysKeywordController(ISysKeywordService sysKeywordService, IUnitOfWork iUnitOfWork)
        {
            _sysKeywordService = sysKeywordService;
            _iUnitOfWork = iUnitOfWork;
        }

        //
        // GET: /Platform/SysKeyword/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model = _sysKeywordService.GetAll().Select(m => new { m.Keyword,  m.Type, m.Count, m.CreatedDate, m.UserCreatedBy, m.Id }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }

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
            var model = _sysKeywordService.GetAll().GroupBy(a => a.Keyword).Select(a => new
            {
                关键词 = a.Key,
                总数 = a.Count()
            }).OrderByDescending(a => a.总数).ThenBy(a => a.关键词);
            var report = new Report(model.ToReportSource());

            //report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _sysKeywordService.GetById(id);
            return View(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var model = new SysKeyword();
            if (!string.IsNullOrEmpty(id))
            {
                model = _sysKeywordService.GetById(id);
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysKeyword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _sysKeywordService.Save(id, model);

            await _iUnitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            _sysKeywordService.Delete(id);

            await _iUnitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }


}