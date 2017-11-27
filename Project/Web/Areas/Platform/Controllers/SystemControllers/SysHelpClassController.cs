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
    public class SysHelpClassController : Controller
    {
        private readonly ISysHelpClassService _iSysHelpClassService;
        private readonly IUnitOfWork _unitOfWork;

        public SysHelpClassController(ISysHelpClassService iSysHelpClassService, IUnitOfWork unitOfWork)
        {
            _iSysHelpClassService = iSysHelpClassService;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Platform/iSysHelpClass/

        public ActionResult Index(string keyword, string ordering, int pageIndex = 1, bool export = false, bool search = false)
        {
            var model =
                _iSysHelpClassService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.SystemId,
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
        // GET: /Platform/iSysHelpClass/Details/5

        public ActionResult Details(object id)
        {
            var item = _iSysHelpClassService.GetById(id);
            return View(item);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/iSysHelpClass/Edit/5

        public ActionResult Edit(string id)
        {
            var item = new SysHelpClass();
            if (!string.IsNullOrEmpty(id))
            {
                item = _iSysHelpClassService.GetById(id);
            }
            return View(item);
        }

        //
        // POST: /Platform/iSysHelpClass/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysHelpClass collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }

            _iSysHelpClassService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }


        //
        // POST: /Platform/iSysHelpClass/Delete/5

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _iSysHelpClassService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }


}
