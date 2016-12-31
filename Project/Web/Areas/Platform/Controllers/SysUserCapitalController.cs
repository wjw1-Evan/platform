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
    /// 用户资金流水记录
    /// </summary>
    public class SysUserCapitalController : Controller
    {
        private readonly ISysUserCapitalService _capitalService;
        private readonly IUnitOfWork _unitOfWork;
        private ISysUserService _iSysUserService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capitalService"></param>
        /// <param name="unitOfWork"></param>
        public SysUserCapitalController(ISysUserCapitalService capitalService, IUnitOfWork unitOfWork, ISysUserService iSysUserService)
        {
            _capitalService = capitalService;
            _unitOfWork = unitOfWork;
            _iSysUserService = iSysUserService;
           
        }

        //
        // GET: /Platform/Capital/

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
                _capitalService.GetAll()
                                 .Select(
                                     a =>
                                     new { a.PayUser.UserName, a.CapitalType, a.TradeNo, a.TotalFee,a.DateOfCollection, a.ExpiryDate, a.Success, a.CreatedDate, a.Remark,a.Id }).Search(keyword);

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
        public ReportResult Report()
        {
            var model = _capitalService.GetAll().ToList();
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        //
        // GET: /Platform/Capital/Details/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _capitalService.GetById(id);
            ViewBag.PayUserId = item.PayUser.UserName;
            return View(item);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/Capital/Edit/5

        public ActionResult Edit(string id)
        {
            var item = new SysUserCapital();
            if (!string.IsNullOrEmpty(id))
            {
                item = _capitalService.GetById(id);
            }

            //列表全部用户
            ViewBag.PayUserId = new SelectList(
                _iSysUserService.GetAll(), "Id", "UserName", item.PayUserId);


            return View(item);
        }

        //
        // POST: /Platform/Capital/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysUserCapital collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }
            
           
            _capitalService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        //
        // POST: /Platform/Capital/Delete/5

        [HttpDelete]
        public ActionResult Delete(object id)
        {
            _capitalService.Delete(id);
            _unitOfWork.Commit();
            return new DeleteSuccessResult();
        }
    }
}