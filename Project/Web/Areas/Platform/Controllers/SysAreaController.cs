using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using IServices.ISysServices;
using Models.SysModels;
using Web.Areas.Platform.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class SysAreaController : Controller
    {
        private readonly ISysAreaService _SysAreaService;
        private readonly IUnitOfWork _unitOfWork;

        public SysAreaController(ISysAreaService SysAreaService, IUnitOfWork unitOfWork)
        {
            _SysAreaService = SysAreaService;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Platform/SysArea/

        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _SysAreaService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.AreaName,
                                         a.SystemId,
                                         a.Enable,
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
        public ReportResult Report()
        {
            var model = _SysAreaService.GetAll().Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.AreaName,
                                         a.SystemId,
                                         a.Enable,
                                         a.Id
                                     });
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        //
        // GET: /Platform/SysArea/Details/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _SysAreaService.GetById(id);
            return View(item);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/SysArea/Edit/5

        public ActionResult Edit(string id)
        {
            var item = new SysArea();
            if (!string.IsNullOrEmpty(id))
            {
                item = _SysAreaService.GetById(id);
            }
            return View(item);
        }

        //
        // POST: /Platform/SysArea/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysArea collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }

            _SysAreaService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }


        //
        // POST: /Platform/SysArea/Delete/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _SysAreaService.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }


}