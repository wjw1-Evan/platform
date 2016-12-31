using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IServices.Infrastructure;
using IServices.ISysServices;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{

    public class IndexController : Controller
    {
        private readonly ISysControllerService _sysControllerService;
        private readonly IUserInfo _iUserInfo;
        private readonly ISysHelpService _iSysHelpService;
        private readonly ISysEnterpriseSysUserService _iSysEnterpriseSysUserService;
        private readonly ISysUserService _iSysUserService;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysEnterpriseService _iSysEnterpriseService;

        public IndexController(ISysControllerService sysControllerService, IUserInfo iUserInfo, ISysHelpService iSysHelpService, ISysEnterpriseSysUserService iSysEnterpriseSysUserService, ISysUserService iSysUserService, IUnitOfWork iUnitOfWork, ISysEnterpriseService iSysEnterpriseService)
        {
            _sysControllerService = sysControllerService;
            _iUserInfo = iUserInfo;
            _iSysHelpService = iSysHelpService;
            _iSysEnterpriseSysUserService = iSysEnterpriseSysUserService;
            _iSysUserService = iSysUserService;
            _iUnitOfWork = iUnitOfWork;
            _iSysEnterpriseService = iSysEnterpriseService;
        }


        // GET: Platform/Index
        public async Task<ActionResult> Index(string changesysenterprise, string loadPage = "")
        {
            var ent = _iSysEnterpriseSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId);

            if (!string.IsNullOrEmpty(changesysenterprise))
            {
                if (ent.Any(a => a.SysEnterpriseId.Equals(changesysenterprise)))
                {
                    var user = _iSysUserService.GetById(_iUserInfo.UserId);
                    user.CurrentEnterpriseId = changesysenterprise;
                    await _iUnitOfWork.CommitAsync();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.sysEnterpriseSysUser = ent;

            ViewBag.UserId = _iUserInfo.UserId;
            ViewBag.UserName = _iUserInfo.UserName;
            ViewBag.OffsiderbarHelp = _iSysHelpService.GetAll().ToList();
            ViewBag.LoadPage = loadPage;

            ViewBag.sysEnterprises = new SelectList(ent.Select(a=>a.SysEnterprise), "Id", "EnterpriseName", _iUserInfo.EnterpriseId);

            return View();
        }

        public ActionResult Menu()
        {
            var area = (string)RouteData.DataTokens["area"];

            var model = _sysControllerService.GetAll(a =>
                                   a.Display && a.Enable &&
                                   a.SysControllerSysActions.Any(
                                       b =>
                                       b.SysRoleSysControllerSysActions.Any(
                                           c =>
                                           c.SysRole.Users.Any(
                                               d => d.UserId == _iUserInfo.UserId))) &&
                                   a.SysArea.AreaName.Equals(area)).ToList();

            //.FromCache(tags: new[] { "SysController" });禁用缓存，权限变更后不能清理别的用户的缓存数据    

            return View(model);
        }



    }
}