using IServices.ISysServices;
using Models.SysModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Web.Areas.Platform.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class MenuController : Controller
    {
        private readonly ISysControllerService _sysControllerService;
        private readonly IUserInfo _iUserInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysControllerService"></param>
        /// <param name="iUserInfo"></param>
        public MenuController(ISysControllerService sysControllerService, IUserInfo iUserInfo)
        {
            _sysControllerService = sysControllerService;
            _iUserInfo = iUserInfo;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult Index()
        {
            var area = (string)RouteData.DataTokens["area"];

            var locations = _sysControllerService.GetAll(a =>
                   a.Display && a.Enable &&
                   a.SysControllerSysActions.Any(
                       b =>
                           b.SysRoleSysControllerSysActions.Any(
                               c =>
                                   c.SysRole.Users.Any(
                                       d => d.UserId == _iUserInfo.UserId))) &&
                   a.SysArea.AreaName.Equals(area)).ToList();

            var records = locations.Where(l => l.SystemId.Length == 3).OrderBy(l => l.SystemId)
                .Select(l => new Location
                {
                    id = l.SystemId,
                    text = l.Name,
                    url = Url.Action(l.ActionName, l.ControllerName),
                    children = GetChildren(locations, l.SystemId)
                }).ToList();


            return this.Json(records, JsonRequestBehavior.AllowGet);
        }

        private List<Location> GetChildren(List<SysController> locations, string parentId)
        {
            return locations.Where(l => l.SystemId.StartsWith(parentId) && l.SystemId.Length == parentId.Length + 3).OrderBy(l => l.SystemId)
                .Select(l => new Location
                {
                    id = l.SystemId,
                    text = l.Name,
                    url = Url.Action(l.ActionName, l.ControllerName),
                    children = GetChildren(locations, l.SystemId)
                }).ToList();
        }

        public class Location
        {
            public string id { get; set; }

            public string text { get; set; }

            public string url { get; set; }

            public virtual List<Location> children { get; set; }
        }

    }
}
