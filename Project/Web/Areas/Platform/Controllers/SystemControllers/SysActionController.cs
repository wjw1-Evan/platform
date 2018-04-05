using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;
using Web.Helpers;

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
        /// <param name="export"></param>
        /// <param name="pageIndex"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, bool export = false, int pageIndex = 1, bool search = false)
        {
            var model =
                _sysActionService.GetAll()
                                 .Select(
                                     a =>
                                     new { a.Name, a.ActionName, a.SystemId, a.Enable, a.Id }).Search(keyword);
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
