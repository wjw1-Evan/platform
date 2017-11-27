using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class SysDepartmentController : Controller
    {
        private readonly ISysDepartmentService _iDepartmentService;
        private readonly IUnitOfWork _unitOfWork;

        public SysDepartmentController(ISysDepartmentService iDepartmentService, IUnitOfWork unitOfWork)
        {
            _iDepartmentService = iDepartmentService;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Platform/iDepartment/

        public ActionResult Index(string keyword, string ordering, int pageIndex = 1, bool export = false, bool search = false)
        {
            var model =
                _iDepartmentService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.SystemId,
                                         a.Enable,
                                         a.CreatedDate,

                                         a.Remark,
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
        // GET: /Platform/iDepartment/Details/5

        public ActionResult Details(object id)
        {
            var item = _iDepartmentService.GetById(id);
            return View(item);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/iDepartment/Edit/5

        public ActionResult Edit(string id)
        {
            var item = new SysDepartment();
            if (!string.IsNullOrEmpty(id))
            {
                item = _iDepartmentService.GetById(id);
            }
            return View(item);
        }

        //
        // POST: /Platform/iDepartment/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysDepartment collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }

            _iDepartmentService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }


        //
        // POST: /Platform/iDepartment/Delete/5

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _iDepartmentService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }


}
