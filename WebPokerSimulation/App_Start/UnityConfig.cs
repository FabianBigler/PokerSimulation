using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Infrastructure.Repositories;
using PokerSimulation.Core.Helpers;
using PokerSimulation.Core.Model;
using PokerSimulation.Core;

namespace WebPokerSimulation.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IRepository<PlayerEntity>, PlayerRepository>();
            container.RegisterType<IRepository<SessionEntity>, SessionRepository>();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IRepository<PlayedHandEntity>, PlayedHandRepository>();
            container.RegisterType<IRepository<GameActionEntity>, GameActionRepository>();
            //singleton
            container.RegisterType<ISessionScheduler, SessionScheduler>(new ContainerControlledLifetimeManager());                                    
        }
    }
}
