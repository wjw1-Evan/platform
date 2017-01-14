using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BootstrapSupport;
using IServices.ISysServices;
using Microsoft.AspNet.Identity.Owin;
using Web.Models;

namespace Web.Areas.Platform.Controllers
{

    public class ChangePasswordController : Controller
    {
        private readonly IUserInfo _iUserInfo;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ChangePasswordController( IUserInfo iUserInfo)
        {
            _iUserInfo = iUserInfo;
        }

        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        public ChangePasswordController( IUserInfo iUserInfo,ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _iUserInfo = iUserInfo;
            UserManager = userManager;
            SignInManager = signInManager;
        }
        

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
     
        public PartialViewResult Index()
        {
            var model = new ChangePasswordViewModel();
            return PartialView(model);
        }

        //
        // POST: /Account/ResetPassword
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Index(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            var result = await UserManager.ChangePasswordAsync(_iUserInfo.UserId, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData[Alerts.Success] = "修改成功";

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return PartialView(model);
        }

    }
}