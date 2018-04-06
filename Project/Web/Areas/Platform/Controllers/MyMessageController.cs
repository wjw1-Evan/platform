using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using Web.Areas.Platform.Helpers;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 我的消息中心： Todo： 只显示后才操作类的消息
    /// </summary>
    public class MyMessageController : Controller
    {
        private readonly ISysBroadcastService _iSysBroadcastService;
        private readonly IUserInfo _iUserInfo;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysBroadcastReceivedService _iSysBroadcastReceivedService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iSysBroadcastService"></param>
        /// <param name="iUserInfo"></param>
        /// <param name="iUnitOfWork"></param>
        /// <param name="iSysBroadcastReceivedService"></param>
        public MyMessageController(ISysBroadcastService iSysBroadcastService, IUserInfo iUserInfo, IUnitOfWork iUnitOfWork, ISysBroadcastReceivedService iSysBroadcastReceivedService)
        {
            _iSysBroadcastService = iSysBroadcastService;
            _iUserInfo = iUserInfo;
            _iUnitOfWork = iUnitOfWork;
            _iSysBroadcastReceivedService = iSysBroadcastReceivedService;
        }

        // GET: Platform/MyMessage
        /// <summary>
        /// 我收到的消息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string keyword, int pageIndex = 1)
        {
            var model =_iSysBroadcastService.GetAll(a => (a.AddresseeId == null || a.AddresseeId.Contains(_iUserInfo.UserId)) && !a.SysBroadcastReceiveds.Any(b =>b.CreatedBy==_iUserInfo.UserId && b.Deleted)).OrderBy(a => a.SysBroadcastReceiveds.Any(r => r.CreatedBy == _iUserInfo.UserId)).ThenByDescending(a => a.CreatedDate).Search(keyword);

            return View(model.ToPagedList(pageIndex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string id)
        {
            var item = _iSysBroadcastService.GetById(id);

            //创建一条浏览记录
            _iSysBroadcastReceivedService.Add(new SysBroadcastReceived { SysBroadcastId = item.Id });

            await _iUnitOfWork.CommitAsync();

            if (!string.IsNullOrEmpty(item.Url))
            {
                return Redirect(item.Url);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            //创建一条删除记录
            _iSysBroadcastReceivedService.Add(new SysBroadcastReceived { SysBroadcastId = id, Deleted = true });

            await _iUnitOfWork.CommitAsync();

            return new DeleteSuccessResult(); ;
        }
    }
}