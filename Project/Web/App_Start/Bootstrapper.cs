using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using EntityFramework.Audit;
using IServices.Infrastructure;
using IServices.ISysServices;
using Resources;
using Services.Infrastructure;
using Web.Helpers;
using Web.Helpers.ModelMetadataExtensions;
using Web.SignalR;

namespace Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.Load("Services"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
       
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();

            builder.RegisterType<UserInfo>().As<IUserInfo>();

            builder.RegisterType<Messenger>().As<ISignalMessenger>().InstancePerDependency();

            builder.RegisterType<OnTimedEvent>().As<IOnTimedEvent>();

            //builder.RegisterType<SmsService>().As<IIdentityMessageService>();

            //关联mvc
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //自动处理多语言
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(false, typeof(Lang));

        }
    }
}