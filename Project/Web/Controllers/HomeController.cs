using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Index", new {area = "Platform"});
        }

        
    }
}
