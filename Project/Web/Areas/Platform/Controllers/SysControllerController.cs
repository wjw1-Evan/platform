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
    /// 
    /// </summary>
    public class SysControllerController : Controller
    {
        private readonly ISysActionService _sysActionService;
        private readonly ISysAreaService _sysAreaService;
        private readonly ISysControllerService _sysControllerService;
        private readonly ISysControllerSysActionService _sysControllerSysActionService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysActionService"></param>
        /// <param name="sysAreaService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="sysControllerService"></param>
        /// <param name="sysControllerSysActionService"></param>
        public SysControllerController(ISysActionService sysActionService, ISysAreaService sysAreaService,
                                       IUnitOfWork unitOfWork, ISysControllerService sysControllerService,
                                       ISysControllerSysActionService sysControllerSysActionService)
        {
            _sysActionService = sysActionService;
            _sysAreaService = sysAreaService;
            _unitOfWork = unitOfWork;
            _sysControllerService = sysControllerService;
            _sysControllerSysActionService = sysControllerSysActionService;
        }

        //
        // GET: /Platform/SysController/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _sysControllerService.GetAll()
                                     .Select(
                                         a =>
                                         new
                                         {
                                             SysArea = a.SysArea.Name,
                                             a.Name,
                                             a.ControllerName,
                                             a.ActionName,
                                             a.Parameter,
                                             a.SystemId,
                                             a.Display,
                                             a.Ico,
                                             a.Enable,
                                             a.TargetBlank,
                                             a.Id
                                         }).Search(keyword);


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
            var model = _sysControllerService.GetAll().Select(
                                         a =>
                                         new
                                         {
                                             SysArea = a.SysArea.Name,
                                             a.Name,
                                             a.ControllerName,
                                             a.ActionName,
                                             a.Parameter,
                                             a.SystemId,
                                             a.Display,
                                             a.Ico,
                                             a.Enable,
                                             a.TargetBlank,
                                             a.Id
                                         });
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }


        //
        // GET: /Platform/SysController/Details/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _sysControllerService.GetById(id);
            ViewBag.SysAreaId = item.SysArea.Name;
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

        //
        // GET: /Platform/SysController/Edit/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var item = new SysController();
            if (!string.IsNullOrEmpty(id))
            {
                item = _sysControllerService.GetById(id);
            }
            ViewBag.SysAreaId = new SelectList(_sysAreaService.GetAll(), "Id", "Name", item.SysAreaId);
            ViewBag.SysActionsId = new MultiSelectList(_sysActionService.GetAll(), "Id", "Name",
                                                       item.SysControllerSysActions?.Select(a => a.SysActionId));
            return View(item);
        }

        //
        // POST: /Platform/SysController/Edit/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysController collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }

            if (!string.IsNullOrEmpty(id))
            {
                //清除原有数据
                _sysControllerSysActionService.Delete(a => a.SysControllerId.Equals(id) && !collection.SysActionsId.Contains(a.SysActionId));
            }

            _sysControllerService.Save(id, collection);

            if (collection.SysActionsId != null)
            {
                foreach (
                    var actionid in
                        collection.SysActionsId.Where(
                            actionid =>
                            !_sysControllerSysActionService.GetAll()
                                                           .Where(b => b.SysControllerId.Equals(id))
                                                           .Select(b => b.SysActionId)
                                                           .Contains(actionid)))
                {
                    _sysControllerSysActionService.Save(null, new SysControllerSysAction
                    {
                        SysControllerId = collection.Id,
                        SysActionId = actionid
                    });
                }
            }

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        //
        // POST: /Platform/SysController/Delete/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _sysControllerService.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}