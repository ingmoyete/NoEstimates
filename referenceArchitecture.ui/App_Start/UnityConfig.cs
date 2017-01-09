using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.Core.Logger;
using referenceArchitecture.Core.Cache;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.Core.Helpers;
using NoEstimates.repository._0.__Edmx;
using NoEstimates.repository.Core.ChangeDb;
using NoEstimates.service.ProjectService;
using NoEstimates.repository.ProjectRepository;
using NoEstimates.service.RequirementService;
using NoEstimates.repository.RequirementsRepository;
using NoEstimates.repository.TaskRepository;
using NoEstimates.service.TaskService;
using NoEstimates.repository.Edmx.BaseContextAndPartialClasses;
using NoEstimates.service.Core.Base;
using NoEstimates.repository.Core.Mapper;
using NoEstimates.Core.DataTableService;
using NoEstimates.Core.Helpers.DateHelper;
using NoEstimates.service.StatisticService;

namespace referenceArchitecture.ui.App_Start
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
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // !! IMPORTANT WHEN IMPLEMENTING CONTROLLER DO NOT FORGET TO REGISTER SERVICE AND REPOSITORY
            // Core layer ==============
            container.RegisterType<ILogger, LoggerService>();
            container.RegisterType<ICache, CacheService>();
            container.RegisterType<IResource, ResourceCsvService>();
            container.RegisterType<Ihp, hp>();
            container.RegisterType<IBaseService, BaseService>();
            container.RegisterType<IDataTableService, DataTableService>();
            container.RegisterType<IDateHp, DateHp>();

            // Service layer ====================
            container.RegisterType<ITaskService, TaskService>();
            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IRequirementService, RequirementService>();
            container.RegisterType<IStatisticService, StatisticService>();

            // Repository layer ===========
            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<IRequirementsRepository, RequirementsRepository>();
            container.RegisterType<ITaskRepository, TaskRepository>();

            container.RegisterType<IChangeDbConnection, ChangeDbConnection>();
            container.RegisterType<IMapper, Mapper>();
            container.RegisterType<IDbContext, BaseContext>();
        }
    }
}
