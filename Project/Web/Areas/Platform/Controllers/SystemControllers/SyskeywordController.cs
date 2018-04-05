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
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1, bool export = false, bool search = false)
        {
            var model = _sysKeywordService.GetAll().Select(m => new { m.Keyword, m.Type, m.Count, m.CreatedDate, m.UserCreatedBy, m.Id }).Search(keyword);
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
            return View(model.PageResult(pageIndex));
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
