using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NoEstimates.Core.DTO;
using referenceArchitecture.repository.Edmx.Interfaces;
using NoEstimates.repository._0.__Edmx;
using System.Data.Entity;
using referenceArchitecture.Core.Base.DTOBase;
using referenceArchitecture.Core.Exceptions;
using NoEstimates.repository.Core.Base;
using NoEstimates.Core.DataTableService;
using System.Linq.Expressions;

namespace NoEstimates.repository.ProjectRepository
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        /// <summary>
        /// Datatable service
        /// </summary>
        private IDataTableService dataTableService;

        /// <summary>
        /// Constructor used to inject dependencies.
        /// </summary>
        /// <param name="_dataTableService">Datatable dependency.</param>
        public ProjectRepository(IDataTableService _dataTableService)
        {
            this.dataTableService = _dataTableService;
        }

        /// <summary>
        /// Get json response for datatable as object.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dataTableParams"></param>
        /// <returns></returns>
        public DataTableJson<DTOProjectDataTable> getDataTableJson(IDbContext context, DataTableParams dataTableParams)
        {
            // Get the records as a list of DTOProjectDataTable
            var projectDataTable =   getAll<Project>(context)
                                    .Select(x => new DTOProjectDataTable
                                       {
                                           Name = x.Name,
                                           CreationDate = x.CreationDate,
                                           Description = x.Description,
                                           ProjectId = x.Id,
                                           NumberOfTasks = 0,
                                           Status = 0,
                                           ThereAreRequirements = false
                                    }).AsQueryable();

            // Get filtering of the global search
            var date = dataTableService.getDateTimeField(dataTableParams.sSearch, DateHp.OnlyDateFormat, "date", ':');
            Expression<Func<DTOProjectDataTable, bool>> filtering;
            if (!DateHp.isWholeMinDate(date))
            {
                filtering = (x => DbFunctions.TruncateTime(x.CreationDate) == date.Date);
            }
            else
            {
                filtering = (x => x.Name.Contains(dataTableParams.sSearch));
            }


            // Get iqueryable source
            var list = dataTableService.getIQueryableSource<DTOProjectDataTable>
            (
                dataTableParams,
                projectDataTable,
                filtering
            );  

            // Get status
            var listWithStatus = getStatusInListDTOProjectDataTable(context, list);

            return dataTableService.getJsonResponse<DTOProjectDataTable>(listWithStatus);
        }

        /// <summary>
        /// Create a project and save changes of the context.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">ProjectDTO.</param>
        public int createProjectAndSaveChanges(IDbContext context, DTOProject project)
        {
            // Get entity from DTO
            var projectEntity = Mapper.mapProjectEntity(project);

            return insertAndSaveChanges<Project>(context, projectEntity); 
        }

        /// <summary>
        /// Delete a project.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">Project DTO.</param>
        public void deleteProject(IDbContext context, DTOProject project)
        {
            delete<Project>(context, Mapper.mapProjectEntity(project));
        }

        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="id">Id of the project to retrieve.</param>
        /// <returns>A DTO Project corresponding to the "id".</returns>
        public DTOProject getProjectById(IDbContext context, DTOProject project)
        {
            // Get project by id
            var projectEntity = getById<Project>(context, Mapper.mapProjectEntity(project)).FirstOrDefault();

            return Mapper.mapProjectDTO(projectEntity);
        }        

        /// <summary>
        /// Get all the projects.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <returns>A collection with all the projects.</returns>
        public List<DTOProject> getAllProjects(IDbContext context)
        {
            var query = getAll<Project>(context).AsEnumerable()
                .Select(x =>
                    Mapper.mapProjectDTO(x)
                ).ToList();

            return query;
        }

        /// <summary>
        /// Update a project.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">Project DTO.</param>
        public void updateProject(IDbContext context, DTOProject project)
        {
            // Get old entity
            var oldProjectEntity = context.Projects.Where(x => x.Id == project.Id).FirstOrDefault<Project>();

            // Get new entity and keep the dates unchanged
            var newProjectEntity = Mapper.mapProjectEntity(project);

            update<Project>(context, newProjectEntity, oldProjectEntity);
        }

        /// <summary>
        /// Get a name of the project by the id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="id">Id of the name to retrieve.</param>
        /// <returns>Name of the project corresponding to the "id".</returns>
        public DTOProject getProjectByName(IDbContext context, string name)
        {
            var query = getAll<Project>(context).Where(x => x.Name == name).FirstOrDefault();

            return Mapper.mapProjectDTO(query);
        }

        /// <summary>
        /// Get a project by all fields except the id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">Project to filter by.</param>
        /// <returns>A collection that match the criteria.</returns>
        public List<DTOProject> getProjectByAllFieldsExceptId(IDbContext context, DTOProject project)
        {
            var query = context.Projects
                .Where(x =>
                        x.Description == project.Description
                        && x.Name == project.Name
                        && x.IsCompleted == project.IsCompleted

                ).AsEnumerable().Select(x => 
                    Mapper.mapProjectDTO(x)
                ).ToList();
                
            return query;
        }

        /// <summary>
        /// Get project statistics from db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="statistic">Statistic dto object that contains the id of the project.</param>
        /// <returns>A collection of DTOStatistic objects representing the statistic for each requirement.</returns>
        public List<DTOStatistic> getProjectStatistic(IDbContext context, DTOStatisticView statistic)
        {
            // Get requirement with total tasks
            var reqsWithTotalTasks = getReqWithTotalTasks(context, statistic);

            // Get requirements with complete tasks
            var reqsWithCompleteTasks = getReqWithCompleteTasks(context, statistic);

            // Get requirements with acumulativeT
            var reqsWithAcumulativeTime = getReqWithTaskAcumulativeTime(context, statistic);

            // Get total requirements, each one with totalTasks, completeTasks, AcumulativeTime and finalizationTime.
            var totalReqs = (from totalTaskRecord in reqsWithTotalTasks
                            join completeTaskRecord in reqsWithCompleteTasks
                                on totalTaskRecord.RequirementId equals completeTaskRecord.RequirementId into completeOuterJoin
                                from complete in completeOuterJoin.DefaultIfEmpty()
                            join timeRecord in reqsWithAcumulativeTime
                                on totalTaskRecord.RequirementId equals timeRecord.RequirementId into timerOuterJoin
                                from timer in timerOuterJoin.DefaultIfEmpty()
                        select new DTOProjectStatistic
                        {
                            RequirementId = totalTaskRecord.RequirementId,
                            TotalTasks = totalTaskRecord.TotalTasks,
                            TotalCompleteTasks = complete.TotalCompleteTasks,
                            AcumulativeTime = timer.AcumulativeTime,
                            FinalizationDate = timer.FinalizationDate,
                            Time = 0
                        }).ToList();

            // Get complete requirements, each one with totalTasks, completeTasks, AcumulativeTime and finalizationTime.
            var completeReqs = totalReqs.Where(x => x.TotalTasks == x.TotalCompleteTasks)
                                .OrderBy(x => x.FinalizationDate) // ORDER BY FINALIZATION DATE
                                .ToList();

            // Set the DTOStatistic collection
            int totalRequirements = totalReqs.Count;
            int completeRequirements = completeReqs.Count();
            var projectName = getAll<Project>(context).Where(x => x.Id == statistic.ItemStatisticId).Select(x => x.Name).FirstOrDefault();

            var statsFromDb = completeReqs.Select((value, index) => new DTOStatistic
                               {
                                   Id = index,
                                   Name = projectName,

                                   PercentageComplete = (double)(index + 1) / (double)totalRequirements * 100,
                                   DescendingItemComplete = totalRequirements - (index + 1),

                                   TotalItems = totalRequirements,
                                   CompleteItems =0,

                                   AcumulativeTimeInSeconds = value.AcumulativeTime,
                                   VelocityInItemPerSeconds = (double)(index + 1) / (double)value.AcumulativeTime,
                                   hp = Hp
                                   
                               }).ToList();

            // Get acumulative time in seconds
            for (int i = 0; i < statsFromDb.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    statsFromDb[i].AcumulativeTimeInSeconds += statsFromDb[j].AcumulativeTimeInSeconds;
                }
            }

            return statsFromDb;
        }

        #region Private Methods
        private List<DTOProjectStatistic> getReqWithTaskAcumulativeTime(IDbContext context, DTOStatisticView statistic)
        {
            // Get all the requirement with task time
            var query = (from req in getAll<Requirement>(context).Where(x => x.ProjectId == statistic.ItemStatisticId)

                 // left join Task
             join taskRecord in getAll<Task>(context)
                 on req.Id equals taskRecord.RequirementId

             // left join complete
             join completeRecord in getAll<Complete>(context)
                 on taskRecord.Id equals completeRecord.TaskId

             // left join complete
             join timerRecord in getAll<Timer>(context)
                on taskRecord.Id equals timerRecord.TaskId
             where completeRecord.IsComplete
             select new DTOProjectStatistic
             {
                 RequirementId = req.Id,
                 ProjectId = req.ProjectId,
                 Time = timerRecord.TimeInSeconds,
                 FinalizationDate = completeRecord.FinalizationDate
             }).ToList();


            // Loop throuh all the requirement and calculate finalization date and acumulative time 
            var reqsWithAcumulativeTime = new List<DTOProjectStatistic>();

            var reqsIdWithCompleteStatus = query.GroupBy(x => x.RequirementId).Select(x => new { Id = x.Key }).ToList();
            foreach (var req in reqsIdWithCompleteStatus)
            {
                var tasks = query.Where(x => x.RequirementId == req.Id).ToList();

                var latestFinalizationDate = tasks
                    .OrderByDescending(x => x.FinalizationDate)
                    .FirstOrDefault().FinalizationDate; // ORDER BY FINALIZATION DATE

                var acumulativeTime = tasks
                    .GroupBy(x => new { x.RequirementId })
                    .Select(x => x.Sum(z => z.Time))
                    .FirstOrDefault(); // ORDER BY FINALIZATION DATE

                reqsWithAcumulativeTime.Add(new DTOProjectStatistic
                {
                    RequirementId = req.Id,
                    AcumulativeTime = acumulativeTime,
                    FinalizationDate = latestFinalizationDate
                });
            }

            return reqsWithAcumulativeTime;
        }
        private List<DTOProjectStatistic> getReqWithTotalTasks(IDbContext context, DTOStatisticView statistic)
        {
            var query =
                    (from req in getAll<Requirement>(context).Where(x => x.ProjectId == statistic.ItemStatisticId)

                         // left join Task
                     join taskRecord in getAll<Task>(context)
                         on req.Id equals taskRecord.RequirementId into taskOuterJoin
                     from task in taskOuterJoin.DefaultIfEmpty()

                     group req by new
                     {
                         req.Id,
                         req.ProjectId
                     } into g
                     select new DTOProjectStatistic
                     {
                         RequirementId = g.Key.Id,
                         ProjectId = g.Key.ProjectId,
                         TotalTasks = g.Count(),
                     }).ToList();

            return query;
        }
        private List<DTOProjectStatistic> getReqWithCompleteTasks(IDbContext context, DTOStatisticView statistic)
        {

            var query = 
             (from req in getAll<Requirement>(context).Where(x => x.ProjectId == statistic.ItemStatisticId)

                 // left join Task
             join taskRecord in getAll<Task>(context)
                 on req.Id equals taskRecord.RequirementId into taskOuterJoin
             from task in taskOuterJoin.DefaultIfEmpty()

                 // left join complete
             join completeRecord in getAll<Complete>(context)
                 on task.Id equals completeRecord.TaskId into completeOuterJoin
             from complete in completeOuterJoin.DefaultIfEmpty()

             where complete.IsComplete
             group req by new
             {
                 req.Id,
                 req.ProjectId
             } into g
             select new DTOProjectStatistic
             {
                 RequirementId = g.Key.Id,
                 ProjectId = g.Key.ProjectId,
                 TotalCompleteTasks = g.Count(),
             }).ToList();

            return query;
        }
        private List<DTOProjectDataTable> getStatusInListDTOProjectDataTable(IDbContext context, IQueryable<DTOProjectDataTable> source)
        {
            // Get total number of tasks for each requirement
            var totals = (from project in source

                              // left join Requirement
                          join requirementRecord in getAll<Requirement>(context)
                          on project.ProjectId equals requirementRecord.ProjectId into reqOuterJoin
                          from requirement in reqOuterJoin.DefaultIfEmpty()

                              // left join Task
                          join taskRecord in getAll<Task>(context)
                              on requirement.Id equals taskRecord.RequirementId into taskOuterJoin
                          from task in taskOuterJoin.DefaultIfEmpty()
                          let projectCanBeDeleted = (requirement != null)
                          group project by new
                          {
                              ProjectId = project.ProjectId,
                              project.Name,
                              project.CreationDate,
                              project.Description,
                              project.NumberOfTasks,
                              ProjectIdFromRequirement = requirement.ProjectId
                          } into g
                          select new DTOProjectDataTable
                          {
                              Name = g.Key.Name,
                              CreationDate = g.Key.CreationDate,
                              Description = g.Key.Description,
                              ProjectId = g.Key.ProjectId,
                              NumberOfTasks = g.Count(),
                              Status = 0,
                              ThereAreRequirements = g.Key.ProjectId == g.Key.ProjectIdFromRequirement
                          }).AsQueryable();

            // Get total number of complete tasks for each requirement
            var completes = (from project in source
                                 // left join Requirement
                             join requirementRecord in getAll<Requirement>(context)
                             on project.ProjectId equals requirementRecord.ProjectId into reqOuterJoin
                             from requirement in reqOuterJoin.DefaultIfEmpty()

                                 // left join Task
                             join taskRecord in getAll<Task>(context)
                                 on requirement.Id equals taskRecord.RequirementId into taskOuterJoin
                             from task in taskOuterJoin.DefaultIfEmpty()

                                 // left join complete
                             join completeRecord in getAll<Complete>(context)
                                 on task.Id equals completeRecord.TaskId into completeOuterJoin
                             from complete in completeOuterJoin.DefaultIfEmpty()

                             where complete.IsComplete
                             group project by project.ProjectId into g
                             select new
                             {
                                 ProjectId = g.Key,
                                 ProjectCount = g.Count(),

                             }).AsQueryable();

            // Keep same ordering
            totals = dataTableService.CustomOrderBy<DTOProjectDataTable>(totals);

            // Get lists
            var totalList = totals.ToList();
            var completeList = completes.ToList();

            // Ge the Status for each requirement in a DTORequirementDataTable
            var union = (from totalRecord in totalList
                         join completeRecord in completeList on totalRecord.ProjectId equals completeRecord.ProjectId into completeOuterJoin
                            from complete in completeOuterJoin.DefaultIfEmpty()
                         select new DTOProjectDataTable
                         {
                             Name = totalRecord.Name,
                             CreationDate = totalRecord.CreationDate,
                             Description = totalRecord.Description,
                             ProjectId = totalRecord.ProjectId,
                             NumberOfTasks = totalRecord.NumberOfTasks,
                             ThereAreRequirements = totalRecord.ThereAreRequirements,
                             Status = complete == null ? 0 : Math.Round((double)complete.ProjectCount / (double)totalRecord.NumberOfTasks * 100)
                         }).ToList();


            return union;
        }
        #endregion
    }
}
