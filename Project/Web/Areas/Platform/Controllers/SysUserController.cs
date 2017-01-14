using System;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using IServices.ISysServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Models.SysModels;
using Web.Areas.Platform.Helpers;
using Web.Areas.Platform.Models;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    ///     用户管理
    /// </summary>
    public class SysUserController : Controller
    {
        private readonly ISysDepartmentService _iDepartmentService;
        private readonly ISysEnterpriseService _iSysEnterpriseService;
        private readonly ISysEnterpriseSysUserService _iSysEnterpriseSysUserService;
        private readonly IUserInfo _iUserInfo;
        private readonly ISysRoleService _sysRoleService;
        private readonly ISysUserService _sysUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        private ApplicationUserManager _userManager;


        /// <summary>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="sysUserService"></param>
        /// <param name="sysRoleService"></param>
        /// <param name="iDepartmentService"></param>
        /// <param name="iUserInfo"></param>
        /// <param name="iSysEnterpriseSysUserService"></param>
        /// <param name="iSysEnterpriseService"></param>
        /// <param name="iSysDepartmentSysUserService"></param>
        public SysUserController(IUnitOfWork unitOfWork, ISysUserService sysUserService, ISysRoleService sysRoleService,
            ISysDepartmentService iDepartmentService, ISysEnterpriseService iSysEnterpriseService, IUserInfo iUserInfo,
            ISysEnterpriseSysUserService iSysEnterpriseSysUserService,
            ISysDepartmentSysUserService iSysDepartmentSysUserService)
        {
            _unitOfWork = unitOfWork;
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
            _iDepartmentService = iDepartmentService;
            _iSysEnterpriseService = iSysEnterpriseService;
            _iUserInfo = iUserInfo;
            _iSysEnterpriseSysUserService = iSysEnterpriseSysUserService;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
        }

        /// <summary>
        /// </summary>
        /// <param name="userManager"></param>
        public SysUserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        /// <summary>
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        /// <summary>
        ///     用户列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model = _sysUserService.GetAll().Select(a => new
            {
                Department =
                a.SysDepartmentSysUsers.FirstOrDefault(c => c.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId)
                    .SysDepartment.Name,
                a.UserName,
                a.FullName,
                PhoneNumber = a.PhoneNumberConfirmed ? a.PhoneNumber : "",
                Email = a.EmailConfirmed ? a.Email : "",
                a.CreatedDate,
                a.UpdatedDate,
                a.Id
            }).OrderByDescending(a => a.CreatedDate).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
                model = model.OrderBy(ordering, null);

            return View(model.ToPagedList(pageIndex));
        }

        /// <summary>
        ///     数据导出
        /// </summary>
        /// <returns></returns>
        public ReportResult Report()
        {
            var model = _sysUserService.GetAll().Select(a => new
            {
                Department =
                a.SysDepartmentSysUsers.FirstOrDefault(c => c.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId)
                    .SysDepartment.Name,
                a.UserName,
                a.FullName,
                PhoneNumber = a.PhoneNumberConfirmed ? a.PhoneNumber : "",
                Email = a.EmailConfirmed ? a.Email : "",
                a.CreatedDate,
                a.UpdatedDate,
                a.Id
            }).OrderByDescending(a => a.CreatedDate);
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];
            //return new ReportResult(report) { FileName = DateTimeLocal.Now.ToString(CultureInfo.InvariantCulture) };
            return new ReportResult(report);
        }

        /// <summary>
        ///     查看用户详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _sysUserService.GetById(id);

            var config = new MapperConfiguration(a => a.CreateMap<SysUser, SysUserEditModel>());

            var aa = config.CreateMapper().Map<SysUserEditModel>(item);

            ViewBag.SysEnterprisesId = string.Join(",",
                item.SysEnterpriseSysUsers.Select(a => a.SysEnterprise.EnterpriseName).ToArray());


            ViewBag.DepartmentId =
                item.SysDepartmentSysUsers.FirstOrDefault(c => c.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId)?
                    .SysDepartment.Name;

            return View(aa);
        }

        /// <summary>
        ///     新建用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        /// <summary>
        ///     编辑用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var item = new SysUser();
            if (!string.IsNullOrEmpty(id))
                item = _sysUserService.GetById(id);
            ViewBag.SysEnterprisesId =
                new MultiSelectList(
                    _iSysEnterpriseService.GetAll(
                        a => a.SysEnterpriseSysUsers.Any(b => b.SysUserId == _iUserInfo.UserId)), "Id", "EnterpriseName",
                    item.SysEnterpriseSysUsers?.Select(a => a.SysEnterpriseId));
            ViewBag.SysRolesId = new MultiSelectList(_sysRoleService.GetAll(), "Id", "RoleName",
                item.Roles?.Select(a => a.RoleId));

            ViewBag.DepartmentId =
                _iDepartmentService.GetAll()
                    .ToSystemIdSelectList(
                        item.SysDepartmentSysUsers.FirstOrDefault(
                            c => c.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId)?.SysDepartmentId);

            var config = new MapperConfiguration(a => a.CreateMap<SysUser, SysUserEditModel>());

            var aa = config.CreateMapper().Map<SysUserEditModel>(item);


            return View(aa);
        }

        /// <summary>
        ///     保存用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysUserEditModel collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                ViewBag.SysEnterprisesId =
                    new MultiSelectList(
                        _iSysEnterpriseService.GetAll(
                            a => a.SysEnterpriseSysUsers.Any(b => b.SysUserId == _iUserInfo.UserId)), "Id",
                        "EnterpriseName", collection.SysEnterprisesId);
                ViewBag.SysRolesId = new MultiSelectList(_sysRoleService.GetAll(), "Id", "RoleName",
                    collection.SysRolesId);

                ViewBag.DepartmentId = _iDepartmentService.GetAll().ToSystemIdSelectList(collection.DepartmentId);
                return View(collection);
            }

            var config = new MapperConfiguration(a => a.CreateMap<SysUserEditModel, SysUser>());

            //config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            if (!string.IsNullOrEmpty(id))
            {
                //更新用户信息
                var item = _sysUserService.GetById(id);

                item.UpdatedDate = DateTimeLocal.Now;

                mapper.Map(collection, item);

                //处理角色
                //item.Roles.Clear();


                foreach (var role in _sysRoleService.GetAll().ToList())
                    await UserManager.RemoveFromRoleAsync(item.Id, role.Name);

                foreach (var roleId in collection.SysRolesId)
                    item.Roles.Add(new IdentityUserRole {RoleId = roleId, UserId = item.Id});

                _iSysDepartmentSysUserService.Delete(
                    a => a.SysUserId == item.Id && a.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId);

                _iSysDepartmentSysUserService.Save(null,
                    new SysDepartmentSysUser {SysDepartmentId = collection.DepartmentId, SysUserId = item.Id});

                //处理关联企业
                //限制编辑自己的关联企业
                if (item.Id != _iUserInfo.UserId)
                {
                    foreach (
                        var ent in _iSysEnterpriseSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).ToList())
                        _iSysEnterpriseSysUserService.Delete(
                            a => a.SysEnterpriseId == ent.SysEnterpriseId && a.SysUserId == item.Id);

                    foreach (var entId in collection.SysEnterprisesId)
                        item.SysEnterpriseSysUsers.Add(new SysEnterpriseSysUser
                        {
                            SysEnterpriseId = entId,
                            SysUserId = item.Id
                        });
                }

                await _unitOfWork.CommitAsync();

                if (!string.IsNullOrEmpty(collection.Password))
                {
                    UserManager.RemovePassword(id);
                    var re = UserManager.AddPassword(id, collection.Password);

                    if (!re.Succeeded)
                        foreach (var error in re.Errors)
                            ModelState.AddModelError("", error);
                }
            }
            else
            {
                collection.Id = Guid.NewGuid().ToString();
                var item = mapper.Map<SysUserEditModel, SysUser>(collection);

                item.SysDepartmentSysUsers.Add(new SysDepartmentSysUser
                {
                    SysUserId = item.Id,
                    SysDepartmentId = collection.DepartmentId
                });

                foreach (var roleId in collection.SysRolesId)
                    item.Roles.Add(new IdentityUserRole {RoleId = roleId, UserId = item.Id});

                foreach (var entId in collection.SysEnterprisesId)
                    item.SysEnterpriseSysUsers.Add(new SysEnterpriseSysUser
                    {
                        SysEnterpriseId = entId,
                        SysUserId = item.Id
                    });

                //创建用户
                var re = await UserManager.CreateAsync(item, collection.Password);

                if (!re.Succeeded)
                    foreach (var error in re.Errors)
                        ModelState.AddModelError("", error);
            }

            if (!ModelState.IsValid)
            {
                Edit(id);
                ViewBag.SysRolesId = new MultiSelectList(_sysRoleService.GetAll(), "Id", "Name", collection.SysRolesId);

                ViewBag.DepartmentId = _iDepartmentService.GetAll().ToSystemIdSelectList(collection.DepartmentId);
                return View(collection);
            }

            return new EditSuccessResult(id);
        }

        /// <summary>
        ///     删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            _sysUserService.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}