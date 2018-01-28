using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IServices.Infrastructure;
using IServices.ISysServices;
using Resources;
using Services;
using Services.Infrastructure;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
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

            builder.RegisterType<ApplicationDbContext>().As<DbContext>().InstancePerLifetimeScope();

            builder.RegisterType<UserInfo>().As<IUserInfo>();

            builder.RegisterType<Messenger>().As<ISignalMessenger>().InstancePerDependency();

            builder.RegisterType<OnTimedEvent>().As<IOnTimedEvent>();

            //builder.RegisterType<SmsService>().As<IIdentityMessageService>();


            //关联mvc
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            var container = builder.Build();


            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            // Set the dependency resolver for MVC.
            var resolver = new AutofacDependencyResolver(container);

            DependencyResolver.SetResolver(resolver);

            //自动处理多语言
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(false, typeof(Lang));

        }
    }
}
