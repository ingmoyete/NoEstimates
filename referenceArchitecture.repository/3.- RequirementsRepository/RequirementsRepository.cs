using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.repository._0.__Edmx;
using NoEstimates.repository.Core.Base;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NoEstimates.repository.RequirementsRepository
{
    public class RequirementsRepository : BaseRepository, IRequirementsRepository
    {
        /// <summary>
        /// Data table service.
        /// </summary>
        private IDataTableService dataTableService;

        /// <summary>
        /// Constructor used to inject dependencies.
        /// </summary>
        /// <param name="_dataTableService">Datatable service to inject.</param>
        public RequirementsRepository(IDataTableService _dataTableService)
        {
            this.dataTableService = _dataTableService;
        }

        /// <summary>
        /// Check if a requirement exist.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requiremet to check in db.</param>
        /// <returns>True if the requirement exist. Otherwise false.</returns>
        public bool requirementExistsByProjectId(IDbContext context, DTORequirements requirement)
        {
            var query = getAll<Requirement>(context).Where(x => x.ProjectId == requirement.ProjectId).Any();

            return query;
        }

        /// <summary>
        /// Get json datatable for requirements.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement that contains the projectId</param>
        /// <param name="dataTableparms"></param>
        /// <returns></returns>
        public DataTableJson<DTORequirementDataTable> getJsonDataTable(IDbContext context, DTORequirements requirement, DataTableParams dataTableparms)
        {
            // Get requirement as a list of DTORequirementDataTable
            var requirementsDataTable = getAllByProjectId(context, requirement)
                                .Select(x => new DTORequirementDataTable
                                {
                                    RequirementId = x.Id,
                                    ProjectId = x.ProjectId,
                                    CreationDate = x.CreationDate,
                                    Description = x.Description,
                                    Name = x.Name,
                                    NumberOfTasks = 0,
                                    ThereAreTasks = false
                                }).AsQueryable();

            // Get the filtering
            Expression<Func<DTORequirementDataTable, bool>> filtering = getFilteringForReqDataTable(dataTableparms);

            // Get json
            var iqueryableSource = dataTableService.getIQueryableSource<DTORequirementDataTable>
            (
                dataTableparms,
                requirementsDataTable,
                filtering
            );

            // Get status
            var listWithStatus = getStatusInListDTORequirementDataTable(context, iqueryableSource);

            // Return json response
            return dataTableService.getJsonResponse<DTORequirementDataTable>(listWithStatus); 
        }

        /// <summary>
        /// Get project name by project id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement that contains the projectId to filterby.</param>
        /// <returns>The name of the project</returns>
        public DTOProject getProjectNameAndIdByRequirement(IDbContext context, DTORequirements requirement)
        {
            var query = getAll<Project>(context)
                        .Where(x => x.Id == requirement.ProjectId)
                        .Select(x => new { x.Name, x.Id })
                        .AsEnumerable().Select(x => new DTOProject
                        {
                            Name = x.Name,
                            Id = x.Id
                        }).FirstOrDefault();

            return query;

        }

        /// <summary>
        /// Get requirement by name.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement that contains the name field to filter by.</param>
        /// <returns>A DTO requirement.</returns>
        public DTORequirements getRequirementByName(IDbContext context, DTORequirements requirement)
        {
            var query = getAllByProjectId(context, requirement)
                        .Where(x => x.Name == requirement.Name && x.ProjectId == requirement.ProjectId).FirstOrDefault();

            return Mapper.mapDTORequirements(query);
        }

        /// <summary>
        /// Get a requirement by id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="id">Id of the requirement to get.</param>
        /// <returns>A requirement DTO.</returns>
        public DTORequirements getRequirementById(IDbContext context, DTORequirements requirement)
        {
            var query = getById<Requirement>(context, Mapper.mapEntityRequirements(requirement)).FirstOrDefault();

            return Mapper.mapDTORequirements(query);
        }

        /// <summary>
        /// Insert a requirement in the db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement DTO to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertRequirementAndSaveChanges(IDbContext context, DTORequirements requirement)
        {
            throwExceptionIfForeignKeyInvalid(requirement.ProjectId);

            // Get entity from DTO
            var requirementEntity = Mapper.mapEntityRequirements(requirement);

            // Return id
            return insertAndSaveChanges<Requirement>(context, requirementEntity);
        }

        /// <summary>
        /// Update a requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement to update.</param>
        public void updateRequirement(IDbContext context, DTORequirements requirement)
        {
            // Old entity
            var oldEntity = context.Requirements.Where(x => x.Id == requirement.Id).FirstOrDefault();

            // New entity and set fk and dates
            var newEntity = Mapper.mapEntityRequirements(requirement);
            newEntity.ProjectId = oldEntity.ProjectId;
            newEntity.CreationDate = oldEntity.CreationDate;
            newEntity.FinalizationDate = oldEntity.FinalizationDate;

            // Update entity
            update<Requirement>(context, newEntity, oldEntity);
        }

        /// <summary>
        /// Delete a requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement to remove.</param>
        public void deleteRequirement(IDbContext context, DTORequirements requirement)
        {
            // Delete entity
            delete<Requirement>(context, Mapper.mapEntityRequirements(requirement));
        }

        /// <summary>
        /// Get all the requirement DTO by all fields except for the id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement DTO.</param>
        /// <returns>A collection of requirement DTO matching the criteria.</returns>
        public List<DTORequirements> getByAllFieldsExceptId(IDbContext context, DTORequirements requirement)
        {
            var query = getAllByProjectId(context, requirement)
                .Where(x =>
                    x.Description == requirement.Description
                    && x.Name == requirement.Name
                    && x.IsComplete == requirement.IsComplete
                ).AsEnumerable().Select(x => 
                    Mapper.mapDTORequirements(x)
                ).ToList();

            return query;
        }

        /// <summary>
        /// Get statistics for a requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="statistic">Statistic dto that contains the id of the requirement.</param>
        /// <returns>A collection of DTOStatistic with all the needed information to build the statistic.</returns>
        public List<DTOStatistic> getRequirementStatistics(IDbContext context, DTOStatisticView statistic)
        {
            // Get name of the requirement
            var name = getAll<Requirement>(context).Where(x => x.Id == statistic.ItemStatisticId).Select(x => x.Name).FirstOrDefault();

            // Get the total tasks and complete tasks for a requirement
            var completeTasks = getTimeForEachCompleteTaskByReqId(context, statistic).Count();
            var totalTasks = getTotalTasksByReqId(context, statistic).Select(x => x.Id).Count();

            // Get time for each complete task
            var query = getTimeForEachCompleteTaskByReqId(context, statistic)
                                .AsEnumerable().Select((value, index) => new DTOStatistic
                                {
                                    Id = index,
                                    Name = name,

                                    PercentageComplete = (double)(index + 1) / (double)totalTasks * 100,
                                    DescendingItemComplete = totalTasks - (index + 1),

                                    CompleteItems = completeTasks,
                                    TotalItems = totalTasks,

                                    AcumulativeTimeInSeconds = value,
                                    VelocityInItemPerSeconds = (double)(index + 1) / (double)value,
                                    hp = Hp 
                                }).ToList();

            // Get acumulative time in seconds
            for (int i = 0; i < query.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    query[i].AcumulativeTimeInSeconds += query[j].AcumulativeTimeInSeconds;
                }
            }

            return query;
        }

        #region Private Methods
        /// <summary>
        /// Get an iqueryable collection with the time for each task.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="statistic">DTOStatisticView object that contains the id to filter by.</param>
        /// <returns>An iqueryable collection.</returns>
        private IQueryable<int> getTimeForEachCompleteTaskByReqId(IDbContext context, DTOStatisticView statistic)
        {
            var completeTasks = (from taskRecord in getTotalTasksByReqId(context, statistic)
                                 join completeRecord in getAll<Complete>(context) on taskRecord.Id equals completeRecord.TaskId
                                 join timerRecord in getAll<Timer>(context) on taskRecord.Id equals timerRecord.TaskId
                                 where completeRecord.IsComplete
                                 orderby completeRecord.FinalizationDate ascending // ORDER BY FINALIZATION DATE
                                 select timerRecord.TimeInSeconds).AsQueryable();
            return completeTasks;
        }

        /// <summary>
        /// Get all the tasks (complete or not) for one requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="statistic">DTOStatisticView object that contains the requirement id to filter by.</param>
        /// <returns>An iqueryable collection.</returns>
        private IQueryable<Task> getTotalTasksByReqId(IDbContext context, DTOStatisticView statistic)
        {
            // Get all task by requirement id
            var allTasksByReqId = getAll<Task>(context).Where(x => x.RequirementId == statistic.ItemStatisticId).AsQueryable();
            return allTasksByReqId;
        }

        /// <summary>
        /// Get all the requirements as iqueryable and by project id.
        /// </summary>
        /// <param name="context">context of the database.</param>
        /// <param name="requirement">Requirement that contains the project id fk.</param>
        /// <returns>An iqueryable collection.</returns>
        private IQueryable<Requirement> getAllByProjectId(IDbContext context, DTORequirements requirement)
        {
            throwExceptionIfForeignKeyInvalid(requirement.ProjectId);

            return getAll<Requirement>(context).Where(x => x.ProjectId == requirement.ProjectId).AsQueryable();
        }

        /// <summary>
        /// Get the filtering for the requirement data table.
        /// </summary>
        /// <param name="dataTableparms">Datatable parameters.</param>
        /// <returns>An filtering expression.</returns>
        private Expression<Func<DTORequirementDataTable, bool>> getFilteringForReqDataTable(DataTableParams dataTableparms)
        {
            Expression<Func<DTORequirementDataTable, bool>> filtering;
            var date = dataTableService.getDateTimeField(dataTableparms.sSearch, DateHp.OnlyDateFormat, "date", ':');
            if (!DateHp.isWholeMinDate(date))
            {
                filtering = (x => DbFunctions.TruncateTime(x.CreationDate) == date.Date);
            }
            else
            {
                filtering = (x =>
                                x.Name.Contains(dataTableparms.sSearch));
            }

            return filtering;
        }

        /// <summary>
        /// Get the percentage of complete task for each requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement DTO that contains the requirement id.</param>
        /// <returns>The percentage of the complete task.</returns>
        private List<DTORequirementDataTable> getStatusInListDTORequirementDataTable(IDbContext context, IQueryable<DTORequirementDataTable> source)
        {
            // Get total number of tasks for each requirement
            var totals = (from req in source

                              // left join Task
                          join taskRecord in getAll<Task>(context)
                              on req.RequirementId equals taskRecord.RequirementId into taskOuterJoin
                          from task in taskOuterJoin.DefaultIfEmpty()

                          group req by new
                          {
                              req.RequirementId,
                              req.ProjectId,
                              req.CreationDate,
                              req.Description,
                              req.Name,
                              req.NumberOfTasks,
                              RequirementIdInTask = task.RequirementId
                          } into g
                          select new DTORequirementDataTable
                          {
                              RequirementId = g.Key.RequirementId,
                              ProjectId = g.Key.ProjectId,
                              CreationDate = g.Key.CreationDate,
                              Description = g.Key.Description,
                              Name = g.Key.Name,
                              NumberOfTasks = g.Count(),
                              ThereAreTasks = g.Key.RequirementIdInTask == g.Key.RequirementId
                          }).AsQueryable();

            // Get total number of complete tasks for each requirement
            var completes = (from req in source

                                 // left join Task
                             join taskRecord in getAll<Task>(context)
                                 on req.RequirementId equals taskRecord.RequirementId into taskOuterJoin
                             from task in taskOuterJoin.DefaultIfEmpty()

                                 // left join complete
                             join completeRecord in getAll<Complete>(context)
                                 on task.Id equals completeRecord.TaskId into completeOuterJoin
                             from complete in completeOuterJoin.DefaultIfEmpty()

                             where complete.IsComplete
                             group req by req.RequirementId into g
                             select new
                             {
                                 ReqId = g.Key,
                                 ReqIdCount = g.Count(),

                             }).AsQueryable();

            // Set inital ordering
            totals = dataTableService.CustomOrderBy<DTORequirementDataTable>(totals);

            // Get total and complete collection as enumerables
            var totalsEnum = totals.ToList();
            var completesEnum = completes.ToList();

            // Ge the Status for each requirement in a DTORequirementDataTable
            var union = (from totalRecord in totalsEnum
                         join completeRecord in completesEnum 
                            on totalRecord.RequirementId equals completeRecord.ReqId into completeOuterJoin
                         from complete in completeOuterJoin.DefaultIfEmpty()

                         select new DTORequirementDataTable
                         {
                             RequirementId = totalRecord.RequirementId,
                             ProjectId = totalRecord.ProjectId,
                             CreationDate = totalRecord.CreationDate,
                             Description = totalRecord.Description,
                             Name = totalRecord.Name,
                             NumberOfTasks = totalRecord.NumberOfTasks,
                             ThereAreTasks = totalRecord.ThereAreTasks,
                             Status = (complete == null) ? 0 : Math.Round((double)complete.ReqIdCount / (double)totalRecord.NumberOfTasks * 100)
                         }).ToList();


            return union;
        }
        #endregion
    }
}
