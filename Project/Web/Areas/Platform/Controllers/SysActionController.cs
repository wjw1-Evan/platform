using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Web.Areas.Platform.Helpers;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public class SysActionController : Controller
    {
        private readonly ISysActionService _sysActionService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysActionService"></param>
        /// <param name="unitOfWork"></param>
        public SysActionController(ISysActionService sysActionService, IUnitOfWork unitOfWork)
        {
            _sysActionService = sysActionService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _sysActionService.GetAll()
                                 .Select(
                                     a =>
                                     new { a.Name, a.ActionName, a.SystemId,a.Enable, a.Id }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }

            return View(model.ToPagedList(pageIndex));
        }

        /// <summary>
        /// 导出全部数据
        /// </summary>
        /// <returns></returns>
        public ReportResult Report(string keyword)
        {
            var model =
             _sysActionService.GetAll()
                              .Select(
                                  a =>
                                  new { a.Name, a.ActionName, a.SystemId, a.Enable, a.Id }).Search(keyword);

            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            var item = _sysActionService.GetById(id);
            return View(item);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/SysAction/Edit/5

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var item = new SysAction();
            if (!string.IsNullOrEmpty(id))
            {
                item = _sysActionService.GetById(id);
            }
            return View(item);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, SysAction collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }

            _sysActionService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            _sysActionService.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}