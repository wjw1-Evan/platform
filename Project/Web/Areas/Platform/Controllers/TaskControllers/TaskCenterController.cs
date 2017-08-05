using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using IServices.ISysServices;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysUserService _iSysUserService;
        private readonly IUserInfo _iUserInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iTaskCenterService"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="iSysUserService"></param>
        /// <param name="iUserInfo"></param>
        public TaskCenterController(ITaskCenterService iTaskCenterService, IUnitOfWork unitOfWork, ISysUserService iSysUserService, IUserInfo iUserInfo)
        {
            _iTaskCenterService = iTaskCenterService;
            _iUnitOfWork = unitOfWork;
            _iSysUserService = iSysUserService;
            _iUserInfo = iUserInfo;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model = _iTaskCenterService.GetAll(a => a.CreatedBy == _iUserInfo.UserId || a.TaskExecutorId == _iUserInfo.UserId).Select(a => new TaskCenterListModel { TaskType = a.TaskType.ToString(), Title = a.Title, Content = a.Content, Files = a.Files, TaskExecutor = a.TaskExecutor.UserName, UserName = a.UserCreatedBy.UserName, ScheduleEndTime = a.ScheduleEndTime, Id = a.Id, ActualEndTime = a.ActualEndTime, CreatedBy = a.CreatedBy, TaskExecutorId = a.TaskExecutorId, Duration = a.Duration, CreatedDate = a.CreatedDate }).Search(keyword);

            model = !string.IsNullOrEmpty(ordering) ? model.OrderBy(ordering, null) : model.OrderBy(a => a.ActualEndTime).ThenBy(a => a.ScheduleEndTime);

            return View(model.ToPagedList(pageIndex));
        }

        /// <summary>
        /// 导出全部数据
        /// </summary>
        /// <returns></returns>
        public ReportResult Report(string keyword)
        {
            var model = _iTaskCenterService.GetAll(a => a.CreatedBy == _iUserInfo.UserId || a.TaskExecutorId == _iUserInfo.UserId).Select(a => new { TaskType = a.TaskType.ToString(), a.Title, a.Content, a.Files, TaskExecutor = a.TaskExecutor.UserName, a.UserCreatedBy.UserName, a.ScheduleEndTime, a.Id }).Search(keyword);

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
        public async Task<ActionResult> Details(object id)
        {
            var item = _iTaskCenterService.GetById(id);
            return View(item);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/TaskCenter/Edit/5

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finished"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id, bool finished = false)
        {
            var item = _iTaskCenterService.GetById(id);

            if (finished)
            {
                if (item.TaskExecutorId != _iUserInfo.UserId)
                {
                    throw new Exception();
                }

                item.ActualEndTime = DateTimeOffset.Now.ToString();

                await _iUnitOfWork.CommitAsync();

                return new EditSuccessResult(id);
            }

            var model = new TaskCenterEditModel();

            if (!string.IsNullOrEmpty(id))
            {
                if (item.CreatedBy != _iUserInfo.UserId)
                {
                    throw new Exception();
                }

                Mapper.Initialize(a => a.CreateMap<TaskCenter, TaskCenterEditModel>());
                Mapper.Map(item, model);
            }

            ViewBag.TaskExecutorId = new SelectList(_iSysUserService.GetAll(), "Id", "UserName", model.TaskExecutorId);

            return View(model);
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, TaskCenterEditModel collection)
        {
            if (!ModelState.IsValid)
            {
                await Edit(id);
                return View(collection);
            }

            var item = new TaskCenter();

            if (!string.IsNullOrEmpty(id))
            {
                item = _iTaskCenterService.GetById(id);
            }

            TryUpdateModel(item);

            _iTaskCenterService.Save(id, item);

            await _iUnitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            var item = _iTaskCenterService.GetById(id);

            if (item.CreatedBy != _iUserInfo.UserId)
            {
                throw new Exception();
            }

            _iTaskCenterService.Delete(id);

            await _iUnitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}