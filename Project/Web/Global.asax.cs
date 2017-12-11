using System;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Web.Optimization;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Services;
using Web.Helpers;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        private System.Threading.Timer _objTimer;
        private static readonly object Locko = new object();

        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {

            // 更新数据库到最新的版本
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Services.Migrations.Configuration>());

            //SqlDependency.Start(ApplicationDbContext.Create().Database.Connection.ConnectionString);

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // webapi
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // mvc
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Run();

            // Replace cache provider with Memcached provider
            //Locator.Current.Register<ICacheProvider>(() => new MemcachedProvider());
            //Locator.Current.Register<ICacheProvider>(() => new MemoryCacheProvider());

            // 计划任务 按照间隔时间执行

            var onTimedEvent = DependencyResolver.Current.GetService<IOnTimedEvent>();

            if (!int.TryParse(ConfigurationManager.AppSettings["TaskInterval"], out int taskInterval))
            {
                taskInterval = 60; //秒
            }

            //// 同步多服务器的启动时间
            //var startDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0).AddMinutes(1);

            //_objTimer = new System.Threading.Timer(onTimedEvent.Run, Locko, (startDateTime - DateTime.Now), new TimeSpan(0, 0, 0, taskInterval));

            _objTimer = new System.Threading.Timer(onTimedEvent.Run, Locko, new TimeSpan(0, 0, 0, taskInterval), new TimeSpan(0, 0, 0, taskInterval));

        }

        /// <summary>
        /// 
        /// </summary>
        protected void Application_End()
        {
            //SqlDependency.Stop(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            _objTimer.Dispose();
        }

        /// <summary>
        /// 按照用户Id更新缓存信息，不同用户Id分别缓存 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.ToLower() == "userid")
            {
                var userid = "wjw1";

                if (context.User.Identity.IsAuthenticated)
                {
                    userid = context.User.Identity.GetUserId();
                }

                context.Trace.Write("缓存过滤器拦截", userid);

                return userid;
            }
            return base.GetVaryByCustomString(context, arg);
        }
    }
}
