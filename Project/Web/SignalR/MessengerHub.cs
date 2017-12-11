using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IServices.Infrastructure;
using IServices.ISysServices;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Models.SysModels;


namespace Web.SignalR
{

    /// <summary>
    /// 用户弹出消息
    /// </summary>
    [HubName("messenger")]
    public class MessengerHub : Hub
    {
        private readonly IUserInfo _iUserInfoService;//= DependencyResolver.Current.GetService<IUserInfo>();
        private readonly ISysBroadcastService _iSysBroadcastService;//= DependencyResolver.Current.GetService<ISysBroadcastService>();
        private readonly ISysSignalROnlineService _iSysSignalROnlineService;//= DependencyResolver.Current.GetService<ISysSignalROnlineService>();
        private readonly IUnitOfWork _iUnitOfWork;//= DependencyResolver.Current.GetService<IUnitOfWork>();

        private const string GroupId = "Messenger";

        /// <summary>
        /// 
        /// </summary>
        public MessengerHub()
        {
            _iUserInfoService = DependencyResolver.Current.GetService<IUserInfo>();
            _iSysBroadcastService = DependencyResolver.Current.GetService<ISysBroadcastService>();
            _iSysSignalROnlineService = DependencyResolver.Current.GetService<ISysSignalROnlineService>();
            _iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            //未读 
            var item = _iSysBroadcastService.GetAll(a => (a.AddresseeId == null || a.AddresseeId == _iUserInfoService.UserId) && a.SysBroadcastReceiveds.All(b => b.CreatedBy != _iUserInfoService.UserId)).Count();

            if (item > 0)
            {
                Clients.Client(Context.ConnectionId).add("您有" + item + "条新的管理信息");
            }

            var date = DateTime.Now.AddDays(-1);

            _iSysSignalROnlineService.Delete(a => a.GroupId == GroupId && a.CreatedBy == _iUserInfoService.UserId);

            _iSysSignalROnlineService.Save(null, new SysSignalROnline { ConnectionId = Context.ConnectionId, GroupId = GroupId });

            _iUnitOfWork.Commit();

            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            _iSysSignalROnlineService.Delete(a => a.ConnectionId == Context.ConnectionId);

            _iUnitOfWork.Commit();

            return base.OnDisconnected(stopCalled);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISignalMessenger
    {
        /// <summary>
        /// 给某个用户弹出右下角消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        void SendMessage(string userId, string message);
        /// <summary>
        /// 给全体用户弹出右下角消息
        /// </summary>
        /// <param name="message"></param>
        void SendAll(string message);

        /// <summary>
        /// 发送给指定用户
        /// </summary>
        /// <param name="message"></param>
        Task<int> SendSysBroadcast(SysBroadcast message);

        /// <summary>
        /// 发送有模块权限的人
        /// </summary>
        /// <param name="area"></param>
        /// <param name="message"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        Task<int> SendSysBroadcastByAction(string action, string controller, string area, SysBroadcast message);
    }

    /// <summary>
    /// 
    /// </summary>
    public class Messenger : ISignalMessenger
    {
        private readonly ISysUserService _iSysUserService;// = DependencyResolver.Current.GetService<ISysUserService>();
        private readonly ISysRoleService _iSysRoleService;
        private readonly ISysBroadcastService _iSysBroadcastService;//= DependencyResolver.Current.GetService<ISysBroadcastService>();
    private readonly ISysSignalROnlineService _iSysSignalROnlineService;//= DependencyResolver.Current.GetService<ISysSignalROnlineService>();
        private readonly IUnitOfWork _iUnitOfWork;

        private const string GroupId = "Messenger";

        /// <summary>
        /// 
        /// </summary>
        public Messenger()
        {
            _iSysUserService = DependencyResolver.Current.GetService<ISysUserService>();
            _iSysRoleService = DependencyResolver.Current.GetService<ISysRoleService>();
            _iSysBroadcastService = DependencyResolver.Current.GetService<ISysBroadcastService>();
            _iSysSignalROnlineService = DependencyResolver.Current.GetService<ISysSignalROnlineService>();
            _iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public void SendMessage(string userId, string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();

            foreach (var item in _iSysSignalROnlineService.GetAll(a => a.GroupId == GroupId && a.CreatedBy == userId))
            {
                context.Clients.Client(item.ConnectionId).add(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendAll(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();
            context.Clients.All.add(message);
        }

        /// <summary>
        /// 推送消息，不用提前保存
        /// </summary>
        /// <param name="sysBroadcast"></param>
        /// <returns></returns>
        public async Task<int> SendSysBroadcast(SysBroadcast sysBroadcast)
        {
            var c4Sended = 0;

            _iSysBroadcastService.Save(null, sysBroadcast);//消息编辑时间即为推送时间

            await _iUnitOfWork.CommitAsync();

            var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();

            if (string.IsNullOrEmpty(sysBroadcast.AddresseeId))
            {
                //全体
                SendAll(sysBroadcast.Title);
                c4Sended++;
            }
            else
            {
                //推送给指定用户
                foreach (
                    var item in
                        _iSysSignalROnlineService.GetAll().Where(
                            a => a.GroupId == GroupId && sysBroadcast.AddresseeId.Contains(a.CreatedBy)))
                {
                    context.Clients.Client(item.ConnectionId).add(sysBroadcast.Title);
                    c4Sended++;
                }
            }
            return c4Sended;
        }
        /// <summary>
        /// 推送给有操作权限的用户
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="area"></param>
        /// <param name="sysBroadcast"></param>
        /// <returns></returns>
        public async Task<int> SendSysBroadcastByAction(string action, string controller, string area, SysBroadcast sysBroadcast)
        {
            var c4Sended = 0;

            var roles = _iSysRoleService.GetAll(a => a.SysRoleSysControllerSysActions.Any(b => b.SysControllerSysAction.SysAction.ActionName == action && b.SysControllerSysAction.SysController.ControllerName == controller && b.SysControllerSysAction.SysController.SysArea.AreaName == area));

            if (roles.Any())
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();

                var q4UserIds = _iSysUserService.GetAll(a => a.Roles.Any(b => roles.Any(c => c.Id == b.RoleId))).Select(a => a.Id);

                sysBroadcast.AddresseeId = string.Join(",", q4UserIds);

                _iSysBroadcastService.Save(null, sysBroadcast);

                await _iUnitOfWork.CommitAsync();

                foreach (var connection in _iSysSignalROnlineService.GetAll().Where(
                        a => a.GroupId == GroupId && q4UserIds.Any(q => q == a.CreatedBy && !a.Deleted)))
                {
                    context.Clients.Client(connection.ConnectionId).add(sysBroadcast.Title);
                    c4Sended++;
                }
            }
            return c4Sended;
        }



    }
}