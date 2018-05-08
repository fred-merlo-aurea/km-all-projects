[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EmailMarketing.Site.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(EmailMarketing.Site.App_Start.NinjectWebCommon), "Stop")]

namespace EmailMarketing.Site.App_Start
{
    using System;
    using System.Web;

    using EmailMarketing.Site.Infrastructure.Abstract.Settings;
    using EmailMarketing.Site.Infrastructure.Concrete.Settings;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Mvc.FilterBindingSyntax;

    using EmailMarketing.Site.Infrastructure.Abstract;
    using EmailMarketing.Site.Infrastructure.Concrete;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // there can be only one!
            kernel.Bind<IPathProvider>().To<PathConfigurationProvider>().InSingletonScope();
            kernel.Bind<IBaseChannelProvider>().To<BaseChannelBusinessLayerProvider>().InSingletonScope();
            kernel.Bind<IAccountProvider>().To<AccountFrameworkEntitiesProvider>().InSingletonScope();
            kernel.Bind<IWebAuthenticationWrapper>().To<WebAuthenticationWrapper>().InSingletonScope();

            // created again for each request
            kernel.Bind<IUserSessionProvider>().To<UserSessionSessionStateProvider>().InRequestScope();
            kernel.Bind<IAuthenticationProvider>().To<AuthenticationFormsProvider>().InRequestScope().
                WithConstructorArgument<IWebAuthenticationWrapper>(kernel.Get<IWebAuthenticationWrapper>()).
                WithConstructorArgument<IAccountProvider>(kernel.Get<IAccountProvider>());
        }        
    }
}
