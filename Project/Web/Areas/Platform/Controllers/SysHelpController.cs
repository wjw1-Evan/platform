using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.ISysServices;
using IServices.Infrastructure;
using Models.SysModels;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Web.Areas.Platform.Helpers;

//Todo： 测试消息系统功能，正式发布前请注释掉测试代码

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class SysHelpController : Controller
    {
        private readonly ISysHelpService _sysHelp;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysHelpClassService _iSysHelpClassService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysHelp"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="iSysHelpClassService"></param>
        public SysHelpController(ISysHelpService sysHelp, IUnitOfWork unitOfWork, ISysHelpClassService iSysHelpClassService)
        {
            _sysHelp = sysHelp;

            _unitOfWork = unitOfWork;
            _iSysHelpClassService = iSysHelpClassService;
        }

        //
        // GET: /Platform/SysHelp/        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model = _sysHelp.GetAll();

            var returnModel = model.Select(a => new
            {
                a.SysHelpClass.Name,
                a.Title,
                a.Sort,
                a.CreatedDate,
                a.Remark,
                a.Id
            }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                returnModel = returnModel.OrderBy(ordering, null);
            }

            return View(returnModel.ToPagedList(pageIndex));
        }


        // 导出全部数据
        // GET: /Platform/SysHelp/Report       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReportResult Report()
        {
            var model = _sysHelp.GetAll().Select(a => new
            {
                a.SysHelpClass.Name,
                a.Title,
                a.Content,
                a.Sort,
                a.CreatedDate,
                a.Remark
            });
            var report = new Report(model.ToReportSource());
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }


        //
        // GET: /Platform/SysHelp/Details/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(object id)
        {
            var item = _sysHelp.GetById(id);
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

        //
        // GET: /Platform/SysHelp/Edit/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var item = new SysHelp();
            if (!string.IsNullOrEmpty(id))
            {
                item = _sysHelp.GetById(id);
            }
            ViewBag.SysHelpClassId =
                _iSysHelpClassService.GetAll(a => a.Enable).ToSystemIdSelectList(item.SysHelpClassId);
            return View(item);
        }

        //
        // POST: /Platform/SysHelp/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> Edit(string id, SysHelp collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id);
                ViewBag.SysHelpClassId =
             _iSysHelpClassService.GetAll(a => a.Enable).ToSystemIdSelectList(collection.SysHelpClassId);
                return View(collection);
            }

            _sysHelp.Save(id, collection);


            //帮助文件更新后通知一下全体成员  测试发送信息用            
            await _unitOfWork.CommitAsync();
            //#region 测试系统消息
            ////测试推送给全部用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试全员消息",
            //    Content = collection.Content,
            //    SysBroadCastTypeId = "100100",
            //});
            ////测试推送给指定用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester4Register:" + collection.Content,
            //    AddresseeId = "jurogn,wangjisheng huangtiancheng tedasky Tester001 Tester4Register",
            //    SysBroadCastTypeId = "100200",
            //});

            ////测试推送给指定角色
            //await _iMessenger.SendSysBroadcast2Role("999", new SysBroadcast
            //{
            //    Title = "测试角色消息",
            //    Content = collection.Content,
            //    SysBroadCastTypeId = "001100",
            //});
            //#endregion

            //#region // 测试可屏蔽功能消息
            ////测试推送给全部用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试全员消息--问答消息--审核通知",
            //    Content = collection.Content,
            //    SysBroadCastTypeId = "520100",
            //});
            ////测试推送给指定用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--问答消息--提问收到新回答",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "520200",
            //});
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--问答消息--回答收到新评论",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "520300",
            //});
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--问答消息--回答被赞",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "520400",
            //});
            ////测试推送给指定角色
            //await _iMessenger.SendSysBroadcast2Role("999", new SysBroadcast
            //{
            //    Title = "测试角色消息--问答消息--评论收到新回复",
            //    Content = collection.Content,
            //    SysBroadCastTypeId = "520500",
            //});
            //#endregion
            ////Todo: 测试图文消息
            //#region 
            ////测试推送给全部用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试全员消息--论坛消息--审核通知",
            //    Content = collection.Content,
            //    Picture= "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url= "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    SysBroadCastTypeId = "530100",
            //});
            ////测试推送给指定用户
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--论坛消息--帖子收到新回帖",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    Picture = "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url = "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "530200",
            //});
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--论坛消息--帖子被赞",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    Picture = "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url = "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "530300",
            //});
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--论坛消息--回帖收到新评论",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    Picture = "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url = "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "530400",
            //});
            //await _iMessenger.SendSysBroadcast(new SysBroadcast
            //{
            //    Title = "测试个人消息--论坛消息--评论收到新回复",
            //    Content = "jurogn,wangjisheng,huangtiancheng,tedasky,Tester001,Tester002,Tester4Admin:" + collection.Content,
            //    Picture = "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url = "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    AddresseeId = "099ed574-7dcc-4119-b366-5bdc1de583f1 225754c5-b9c2-4893-b050-816f7dbcb043 4a2b56b5-ce5a-422f-bb14-b5bbb8568474 64bacf6f-ba3d-4305-9399-ff5566270ae4 Tester001 Tester002 Tester4Admin",
            //    SysBroadCastTypeId = "530500",
            //});
            ////测试推送给指定角色
            //await _iMessenger.SendSysBroadcast2Role("999", new SysBroadcast
            //{
            //    Title = "测试角色消息--论坛消息--评论或回复被赞",
            //    Content = collection.Content,
            //    Picture = "http://zhiweiblob.wjw1.com/img/115191f8713d4563962ac67e9ea73e23.png",
            //    Url = "http://zhiwei.wjw1.com/Cms/Index/Detail/9f06c5b2-261c-45cc-b53f-19c66b717aa5",
            //    SysBroadCastTypeId = "530600",
            //});
            //#endregion
            //Todo：测试活动消息
            return new EditSuccessResult(id);
        }


        //
        // POST: /Platform/SysHelp/Delete/5

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            _sysHelp.Delete(id);

            await _unitOfWork.CommitAsync();

            return new DeleteSuccessResult();
        }
    }
}