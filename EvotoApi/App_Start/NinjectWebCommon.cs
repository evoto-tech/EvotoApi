using System;
using System.Configuration;
using System.Web;
using EvotoApi;
using Management.Database.Interfaces;
using Management.Database.Stores;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace EvotoApi
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
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
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IBindingRoot kernel)
        {
            var m = ConfigurationManager.ConnectionStrings["ManagementConnectionString"].ConnectionString;
            //var r = ConfigurationManager.ConnectionStrings["RegistrarConnectionString"].ConnectionString;

            kernel.Bind<IManaVoteStore>()
                .To<ManaSqlVoteStore>()
                .WithConstructorArgument("connectionString", m);

            kernel.Bind<IManaUserStore>()
                .To<ManaSqlUserStore>()
                .WithConstructorArgument("connectionString", m);

            kernel.Bind<IManaUserLockoutStore>()
                .To<ManaUserLockoutStore>()
                .WithConstructorArgument("connectionString", m);

            kernel.Bind<IManaRefreshTokenStore>()
                .To<ManaSqlRefreshTokenStore>()
                .WithConstructorArgument("connectionString", m);
        }
    }
}