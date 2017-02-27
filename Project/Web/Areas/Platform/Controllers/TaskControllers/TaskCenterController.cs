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
using IServices.ITaskServices;
using Models.TaskModels;
using Web.Areas.Platform.Helpers;
using Web.Areas.Platform.Models;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 任务中心
    /// </summary>
    public class TaskCenterController : Controller
    {
        private readonly ITaskCenterService _iTaskCenterService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iTaskCenterService"></param>
        /// <param name="unitOfWork"></param>
        public TaskCenterController(ITaskCenterService iTaskCenterService, IUnitOfWork unitOfWork)
        {
            _iTaskCenterService = iTaskCenterService;
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
                _iTaskCenterService.GetAll()
                                 .Select(
                                     a =>
                                     new TaskCenterListModel { TaskType = a.TaskType.ToString(), Title= a.Title, Content= a.Content, Files=a.Files, TaskExecutor= a.TaskExecutor.UserName, UserName= a.UserCreatedBy.UserName, ScheduleEndTime= a.ScheduleEndTime, Id= a.Id, ActualEndTime=a.ActualEndTime, CreatedBy = a.CreatedBy, TaskExecutorId = a.TaskExecutorId, Duration=a.Duration }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }
            else
            {
                model = model.OrderBy(a=>a.ActualEndTime).ThenBy(a => a.ScheduleEndTime);
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
             _iTaskCenterService.GetAll()
                              .Select(
                                  a =>
                                  new { TaskType = a.TaskType.ToString(), a.Title, a.Content, a.Files, TaskExecutor = a.TaskExecutor.UserName, a.UserCreatedBy.UserName, a.ScheduleEndTime, a.Id }).Search(keyword);

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
            var item = _iTaskCenterService.GetById(id);
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
        // GET: /Platform/TaskCenter/Edit/5

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var item = new TaskCenter();
            if (!string.IsNullOrEmpty(id))
            {
                item = _iTaskCenterService.GetById(id);
            }

            // 根据类型选择不同的编辑视图
            // 1、任务发布人


            // 2、任务执行人

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
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(string id, TaskCenter collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }

            _iTaskCenterService.Save(id, collection);

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
            _iTaskCenterService.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}