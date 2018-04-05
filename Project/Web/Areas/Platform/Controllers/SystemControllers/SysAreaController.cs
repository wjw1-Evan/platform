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

        public ActionResult Index(string keyword, string ordering, bool export = false, int pageIndex = 1, bool search = false)
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
