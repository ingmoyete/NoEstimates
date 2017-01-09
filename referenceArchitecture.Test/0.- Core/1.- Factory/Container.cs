using NoEstimates.repository._0.__Edmx;
using NoEstimates.repository.ProjectRepository;
using NoEstimates.service.ProjectService;
using NoEstimates.ui.Controllers;
using referenceArchitecture.Core.Cache;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Logger;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.ui.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using NoEstimates.repository.Core.ChangeDb;
using NoEstimates.repository.RequirementsRepository;
using NoEstimates.service.RequirementService;
using NoEstimates.repository.TaskRepository;
using NoEstimates.service.TaskService;
using NoEstimates.repository.Edmx.BaseContextAndPartialClasses;
using NoEstimates.repository.Core.Mapper;
using NoEstimates.Core.DataTableService;
using NoEstimates.Core.Helpers.DateHelper;
using NoEstimates.service.StatisticService;

namespace referenceArchitecture.Test.Core.Factory
{
    /// <summary>
    /// Class that contains methods to create instances of implementations. 
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Create a instance of the IResource implementation.
        /// </summary>
        /// <returns>An insance of IResource.</returns>
        public static IResource createIResource()
        {
            return new ResourceCsvService(createICache(), createILogger(), createHp());
        }

        /// <summary>
        /// Create an instante of the ICache implementation.
        /// </summary>
        /// <returns>An instance of ICache.</returns>
        public static ICache createICache()
        {
            return new CacheService(createHp());
        }

        /// <summary>
        /// Create an instante of the ILogger implementation.
        /// </summary>
        /// <returns>An instance of ILogger.</returns>
        public static ILogger createILogger()
        {
            return new LoggerService(createHp());
        }

        /// <summary>
        /// Create an instance of the Ihp implementation.
        /// </summary>
        /// <returns>An instance of Ihp.</returns>
        public static Ihp createHp()
        {
            return new hp();
        }

        /// <summary>
        /// Create and return an instance of the IChangeDbConnection implementation.
        /// </summary>
        public static IChangeDbConnection createIChangeDbConnection()
        {
            return new ChangeDbConnection(createHp());
        }

        /// <summary>
        /// Create an instance of the IDbContext.
        /// </summary>
        /// <returns>instance of the IDbContext.</returns>
        public static IDbContext createIDbContext()
        {
            var context = new BaseContext(createIChangeDbConnection());

            createIChangeDbConnection().changeConnectionString("Database1Entities", context);

            return context;
        }

        /// <summary>
        /// Create and return an instance of IMapper.
        /// </summary>
        /// <returns></returns>
        public static IMapper createIMapper()
        {
            return new Mapper();
        }
               
        public static ControllerBase createHomeController()
        {
            return new DashBoardController();
        }

        /// <summary>
        /// Create and return an instance of IProjectRepository.
        /// </summary>
        /// <returns></returns>
        public static IProjectRepository createIProjectRepository()
        {
            var projectRepository = new ProjectRepository(createIDataTableService());

            projectRepository.Mapper = createIMapper();
            projectRepository.DateHp = createIDateHp();
            projectRepository.Hp = createHp();

            return projectRepository;
        }

        /// <summary>
        /// Create and return an instance of IProjectService.
        /// </summary>
        /// <returns></returns>
        public static IProjectService createIProjectService()
        {
            var projectService = new ProjectService(createIProjectRepository(), createIRequirementsRepository());

            projectService.GlobalResources = createIResource();
            projectService.DbContext = createIDbContext();

            return projectService;
        }

        /// <summary>
        /// Create and return an instance of IStatisticService
        /// </summary>
        /// <returns></returns>
        public static IStatisticService createIStatisticService()
        {
            var statisticService = new StatisticService(createIRequirementsRepository(), createIProjectRepository());

            statisticService.GlobalResources = createIResource();
            statisticService.DbContext = createIDbContext();

            return statisticService;
        }

        /// <summary>
        /// Create and return an instance of projectsController.
        /// </summary>
        /// <returns></returns>
        public static ProjectsController createProjectsController()
        {
            return new ProjectsController(createIDateHp(), createIProjectService(), createIDataTableService(), createIDbContext() );
        }

        /// <summary>
        /// Create and return an instance of IDataTableService.
        /// </summary>
        /// <returns></returns>
        public static IDataTableService createIDataTableService()
        {
            return new DataTableService(createIDateHp());
        }

        public static IDateHp createIDateHp()
        {
            return new DateHp(createHp());
        }
        /// <summary>
        /// Create and return an instance of IRequirementsRepository.
        /// </summary>
        /// <returns></returns>
        public static IRequirementsRepository createIRequirementsRepository()
        {
            var requirementRepository = new RequirementsRepository(createIDataTableService());

            requirementRepository.Mapper = createIMapper();
            requirementRepository.DateHp = createIDateHp();
            requirementRepository.Hp = createHp();

            return requirementRepository;
        }

        /// <summary>
        /// Create and return an instance of IRequirementService.
        /// </summary>
        /// <returns></returns>
        public static IRequirementService createIRequirementService()
        {
            var requirementService = new RequirementService(createIRequirementsRepository(), createITaskRepository());

            requirementService.DbContext = createIDbContext();
            requirementService.GlobalResources = createIResource();
            requirementService.Hp = createHp();

            return requirementService;
        }

        /// <summary>
        /// Create and return an instance of RequirementsController.
        /// </summary>
        /// <returns></returns>
        public static RequirementsController createRequirementsController()
        {
            return new RequirementsController(createIRequirementService());
        }

        /// <summary>
        /// Create and return an instance of ITaskRepository.
        /// </summary>
        /// <returns></returns>
        public static ITaskRepository createITaskRepository()
        {
            var taskRepository = new TaskRepository();

            taskRepository.Mapper = createIMapper();
            taskRepository.DateHp = createIDateHp();
            taskRepository.Hp = createHp();

            return taskRepository;
        }

        /// <summary>
        /// Create and return an instance of ITaskService.
        /// </summary>
        /// <returns></returns>
        public static ITaskService createITaskService()
        {
            var taskService = new TaskService(createITaskRepository());
            taskService.DbContext = createIDbContext();
            taskService.GlobalResources = createIResource();
            return taskService;
        }

        /// <summary>
        /// Create and return an instance of TasksController.
        /// </summary>
        /// <returns></returns>
        public static TasksController createTasksController()
        {
            return new TasksController(createITaskService());
        }

    }
}
