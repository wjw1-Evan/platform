﻿using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.ISysServices;
using Web.Helpers;
using System.Linq.Dynamic;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SysUserLogController : Controller
    {
        private readonly ISysUserLogService _sysUserLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysUserLogService"></param>
        public SysUserLogController(ISysUserLogService sysUserLogService)
        {
            _sysUserLogService = sysUserLogService;
        }

        //
        // GET: /Platform/SysUserLog/

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
                _sysUserLogService.GetAll()
                                  .Select(
                                      a =>
                                      new
                                      {
                                          a.SysUser.UserName,
                                          SysArea = a.SysControllerSysAction.SysController.SysArea.Name,
                                          SysController = a.SysControllerSysAction.SysController.Name,
                                          SysAction = a.SysControllerSysAction.SysAction.Name,
                                          a.RecordId,
                                          a.Url,
                                          a.Ip,
                                          CreatedDate= a.CreatedDate.Year +"/"+a.CreatedDate.Month+"/"+ a.CreatedDate.Day+" "+a.CreatedDate.Hour+":"+a.CreatedDate.Minute+":"+a.CreatedDate.Second
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
            var model = _sysUserLogService.GetAll().Select(
                                      a =>
                                      new
                                      {
                                          用户名 = a.SysUser.UserName,
                                          区域显示名称 = a.SysControllerSysAction.SysController.SysArea.Name,
                                          控制器显示名称 = a.SysControllerSysAction.SysController.Name,
                                          操作显示名称 = a.SysControllerSysAction.SysAction.Name,
                                          记录编号 = a.RecordId,
                                          a.Url,
                                          a.Ip,
                                          创建日期 = a.CreatedDate
                                      });
            var report = new Report(model.ToReportSource());

            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];
            return new ReportResult(report) { FileName = DateTimeLocal.Now.ToString(CultureInfo.InvariantCulture) };
        }
    }
}