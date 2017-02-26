using System;
using System.Configuration;
using System.Web;
using Blockchain;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using Registrar.Api;
using Registrar.Database.Interfaces;
using Registrar.Database.Stores;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace Registrar.Api
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        public static IKernel Kernel;

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
            Kernel = new StandardKernel();
            try
            {
                Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(Kernel);
                return Kernel;
            }
            catch
            {
                Kernel.Dispose();
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
            var r = ConfigurationManager.ConnectionStrings["RegistrarConnectionString"].ConnectionString;

            kernel.Bind<IRegiUserStore>()
                .To<RegiSqlUserStore>()
                .WithConstructorArgument("connectionString", r);
            kernel.Bind<IRegiUserLockoutStore>()
                .To<RegiUserLockoutStore>()
                .WithConstructorArgument("connectionString", r);
            kernel.Bind<IRegiRefreshTokenStore>()
                .To<RegiSqlRefreshTokenStore>()
                .WithConstructorArgument("connectionString", r);
            kernel.Bind<IRegiBlockchainStore>()
                .To<RegiSqlBlockchainStore>()
                .WithConstructorArgument("connectionString", r);

            kernel.Bind<MultiChainHandler>().To<MultiChainHandler>().InSingletonScope();
        }
    }
}