using NoEstimates.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using referenceArchitecture.repository.Edmx.Interfaces;
using NoEstimates.repository._0.__Edmx;
using referenceArchitecture.Core.Exceptions;
using NoEstimates.repository.Core.Base;
using NoEstimates.Core.Enums;

namespace NoEstimates.repository.TaskRepository
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        // Task ================================================
        /// <summary>
        /// Get the number of completed tas for a requirement.
        /// </summary>
        /// <param name="context">context of the database.</param>
        /// <param name="task">task that contains the requirement id to filter by.</param>
        /// <returns>An integer containing the number of completed tasks.</returns>
        public int getCompletedTasksNumber(IDbContext context, DTOTask task)
        {
            var query = (from taskRecord in getAllByReqId(context, task)
                         join completeRecord in getAll<Complete>(context) on taskRecord.Id equals completeRecord.TaskId
                         where completeRecord.IsComplete
                         select task.Id).Count();
            return query;
        }

        /// <summary>
        /// Get the number of total task for a requirement.
        /// </summary>
        /// <param name="context">context of the database.</param>
        /// <param name="task">task that contains the requirement id to filter by.</param>
        /// <returns>An integer containing the number of tasks for a requirement.</returns>
        public int getAllTasksNumber(IDbContext context, DTOTask task)
        {
            var query = getAllByReqId(context, task).Count();
            return query;
        }
        /// <summary>
        /// Check if tasks exist by requirement id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task tha contains the project id to check the existance of the task.</param>
        /// <returns>True if tasks exists for that project id. Otherwise false.</returns>
        public bool tasksExistByRequirementId(IDbContext context, DTOTask task)
        {
            // Throw exception if requirement id is not valid
            throwExceptionIfForeignKeyInvalid(task.RequirementId);

            var query = getAll<Task>(context).Where(x => x.RequirementId == task.RequirementId).Any();

            return query;
        }

        /// <summary>
        /// Get all the tasks with the complete status, highlight, and timer.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requirement to filter by.</param>
        /// <returns>A Collection of DTOTaskPanel.</returns>
        public List<DTOTaskPanel> getListTaskPanelByRequirementId(IDbContext context, DTOTask task)
        {
            // Throw exception if keys are invalid
            throwExceptionIfForeignKeyInvalid(task.RequirementId);

                // Get defaults
                var defaults = new DTOTaskPanel();

            var query =  (from taskRecord in getAll<Task>(context)

                                 // left join highlight
                                 join highlightRecord in getAll<Highlight>(context)
                                     on taskRecord.Id equals highlightRecord.TaskId into joinHighlight
                                 from highLight in joinHighlight.DefaultIfEmpty()
                                 
                                 // left join complete
                                 join completeRecord in getAll<Complete>(context)
                                     on taskRecord.Id equals completeRecord.TaskId into joinComplete
                                 from complete in joinComplete.DefaultIfEmpty()

                                 // left join timer
                                 join timerRecord in getAll<Timer>(context)
                                     on taskRecord.Id equals timerRecord.TaskId into joinTimer
                                 from timer in joinTimer.DefaultIfEmpty()

                                 where taskRecord.RequirementId == task.RequirementId
                                 select new
                                 {
                                     Task = new { TaskId = taskRecord.Id , Description = taskRecord.Description, CreationDate = taskRecord.CreationDate },
                                     Highlight = new
                                     {
                                         Color = (highLight == null) ? defaults.Highlight.Color : highLight.Color,
                                         Id = (highLight == null) ? defaults.Highlight.Id : highLight.Id
                                     },
                                     Complete = new
                                     {
                                         IsComplete = (complete == null) ? defaults.Complete.IsComplete : complete.IsComplete,
                                         Id = (complete == null) ? defaults.Complete.Id : complete.Id
                                     },   
                                     Timer = new
                                     {
                                        TimeInSeconds = (timer == null) ? defaults.Timer.TimeInSeconds : timer.TimeInSeconds,
                                        Id = (timer == null) ? defaults.Timer.Id : timer.Id,
                                     }

                                 // Map to the DTOTaskPanel
                                 }).AsEnumerable().Select(x => new DTOTaskPanel {
                                     Timer = new DTOTimer { TimeInSeconds = x.Timer.TimeInSeconds, Id = x.Timer.Id},
                                     Complete = new DTOComplete { IsComplete = x.Complete.IsComplete, Id = x.Complete.Id},
                                     Highlight = new DTOHighlightColor {  Color = x.Highlight.Color, Id = x.Highlight.Id},
                                     Task = new DTOTask { Description = x.Task.Description, Id = x.Task.TaskId, CreationDate = x.Task.CreationDate}
                                 }).OrderByDescending(x => x.Task.CreationDate).ToList();

            return query;
        }

        /// <summary>
        /// Get a DTO task panel by task id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task that contains id to find by.</param>
        /// <returns>A single DTO task panel.</returns>
        public DTOTaskPanel getTaskPanelByTaskId(IDbContext context, DTOTask task)
        {
            // Throw exception if keys are invalid
            throwExceptionIfPrimaryKeyInvalid(task.Id);

            // Get defaults
            var defaults = new DTOTaskPanel();

            var query = (from taskRecord in getAll<Task>(context)

                             // left join highlight
                         join highlightRecord in getAll<Highlight>(context)
                             on taskRecord.Id equals highlightRecord.TaskId into joinHighlight
                         from highLight in joinHighlight.DefaultIfEmpty()

                             // left join complete
                         join completeRecord in getAll<Complete>(context)
                             on taskRecord.Id equals completeRecord.TaskId into joinComplete
                         from complete in joinComplete.DefaultIfEmpty()

                             // left join timer
                         join timerRecord in getAll<Timer>(context)
                             on taskRecord.Id equals timerRecord.TaskId into joinTimer
                         from timer in joinTimer.DefaultIfEmpty()

                         where taskRecord.Id == task.Id
                         select new
                         {
                             Task = new { TaskId = taskRecord.Id, Description = taskRecord.Description, CreationDate = taskRecord.CreationDate },
                             Highlight = new
                             {
                                 Color = (highLight == null) ? defaults.Highlight.Color : highLight.Color,
                                 Id = (highLight == null) ? defaults.Highlight.Id : highLight.Id
                             },
                             Complete = new
                             {
                                 IsComplete = (complete == null) ? defaults.Complete.IsComplete : complete.IsComplete,
                                 Id = (complete == null) ? defaults.Complete.Id : complete.Id
                             },
                             Timer = new
                             {
                                 TimeInSeconds = (timer == null) ? defaults.Timer.TimeInSeconds : timer.TimeInSeconds,
                                 Id = (timer == null) ? defaults.Timer.Id : timer.Id,
                             }

                             // Map to the DTOTaskPanel
                         }).AsEnumerable().Select(x => new DTOTaskPanel
                         {
                             Timer = new DTOTimer { TimeInSeconds = x.Timer.TimeInSeconds, Id = x.Timer.Id },
                             Complete = new DTOComplete { IsComplete = x.Complete.IsComplete, Id = x.Complete.Id },
                             Highlight = new DTOHighlightColor { Color = x.Highlight.Color, Id = x.Highlight.Id },
                             Task = new DTOTask { Description = x.Task.Description, Id = x.Task.TaskId, CreationDate = x.Task.CreationDate }
                         }).OrderByDescending(x => x.Task.CreationDate).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// Remove a task from db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task to remove.</param>
        public void deleteTask(IDbContext context, DTOTask task)
        {
            delete<Task>(context, Mapper.mapEntityTask(task));
        }

        /// <summary>
        /// Get all tasks by a requirement id. It also gets the id and name of the project and requirement.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="requirement">Requeriment to filter by.</param>
        /// <returns>A DTO Task view.</returns>
        public DTOIdsNames getIdsAndNamesForProjectAndRequirement(IDbContext context, DTOTask task)
        {
            // Throw exception if keys are invalid
            throwExceptionIfForeignKeyInvalid(task.RequirementId);

            // Get ids and names of project and requirement
            var dtoIdsNames = (from projectRecord in getAll<Project>(context)
                               join requirementRecord in getAll<Requirement>(context) 
                                    on projectRecord.Id equals requirementRecord.ProjectId
                               where requirementRecord.Id == task.RequirementId
                               select new DTOIdsNames
                               {
                                   ProjectId = projectRecord.Id,
                                   ProjectName = projectRecord.Name,
                                   RequirementId = requirementRecord.Id,
                                   RequirementName = requirementRecord.Name
                               }).FirstOrDefault();


            return dtoIdsNames;
        }

        /// <summary>
        /// Get a DTO task by id.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task to filter by.</param>
        /// <returns>A DTO Task matching the criteria.</returns>
        public DTOTask getTaskById(IDbContext context, DTOTask task)
        {
            var query = getById<Task>(context, Mapper.mapEntityTask(task)).FirstOrDefault();
            return  Mapper.mapDTOTask(query);
        }

        /// <summary>
        /// Insert a task in db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertTaskAndSaveChanges(IDbContext context, DTOTask task)
        {
            // Get entity from dto
            var entity = Mapper.mapEntityTask(task);

            return  insertAndSaveChanges<Task>(context, entity);
        }

        /// <summary>
        /// Update a task in db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="task">Task to be updated.</param>
        public void updateTask(IDbContext context, DTOTask task)
        {
            // Get old entity
            var oldEntity = getById<Task>(context, Mapper.mapEntityTask(task)).FirstOrDefault();

            // Get new entity and set the dates to the ones in old entity
            var newEntity = Mapper.mapEntityTask(task);
            newEntity.RequirementId = oldEntity.RequirementId;
            newEntity.CreationDate = oldEntity.CreationDate;

            // Update
            update<Task>(context, newEntity, oldEntity);
        }

        // Highlight ================================================
        /// <summary>
        /// Check if highlight exist.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="highlight">Highlight to check for existence</param>
        /// <returns>True if highlight exist. Otherwise false.</returns>
        public bool highlightExist(IDbContext context, DTOHighlightColor highlight)
        {
            var query = getAll<Highlight>(context)
                .Where(x => x.TaskId == highlight.TaskId)
                .Any();

            return query;
        }

        /// <summary>
        /// Insert a color for a task and save changes.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="highlight">Highlight DTO to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertHighlightColorAndSaveChanges(IDbContext context, DTOHighlightColor highlight)
        {
            // Throw exception if fk is invalid
            throwExceptionIfForeignKeyInvalid(highlight.TaskId);

            // Get entity from DTO
            var entity = Mapper.mapEntityHighlight(highlight);

            return insertAndSaveChanges<Highlight>(context, entity);
        }

        /// <summary>
        /// Update a color for a task.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="highlight">Highlight DTO to be updated.</param>
        public void updateHighlightColor(IDbContext context, DTOHighlightColor highlight)
        {
            // Get old entity
            var oldEntity = context.Highlights.Where(x => x.Id == highlight.Id).FirstOrDefault();

            // Get new entity
            var newEntity = Mapper.mapEntityHighlight(highlight);

            // Update changes
            update<Highlight>(context, newEntity, oldEntity);
        }

        /// <summary>
        /// Delete a highlight entity.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="highlight">Highlight DTO to be deleted</param>
        public void deleteHighlightColor(IDbContext context, DTOHighlightColor highlight)
        {
            // Delete entity
            delete<Highlight>(context, Mapper.mapEntityHighlight(highlight));
        }

        // Complete ================================================
        /// <summary>
        /// Check if complete exist in db.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="complete">DTO to check for existence.</param>
        /// <returns>True if complete exist in db. Otherwise false.</returns>
        public bool completeExist(IDbContext context, DTOComplete complete)
        {
            var query = getAll<Complete>(context)
                .Where(x => x.TaskId == complete.TaskId)
                .Any();

            return query;
        }

        /// <summary>
        /// Insert a complete status for a task and save changes.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="complete">Complete DTO to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertCompleteAndSaveChanges(IDbContext context, DTOComplete complete)
        {
            // Throw exception if fk is invalid
            throwExceptionIfForeignKeyInvalid(complete.TaskId);

            // Get entity from DTO
            var entity = Mapper.mapEntityComplete(complete);

            return insertAndSaveChanges<Complete>(context, entity);
        }

        /// <summary>
        /// Update a complete status for a task.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="complete">A complete DTO to be updated.</param>
        public void updateComplete(IDbContext context, DTOComplete complete)
        {
            // Get old entity
            var oldEntity = context.Completes.Where(x => x.Id == complete.Id).FirstOrDefault();

            // Get new entity
            var newEntity = Mapper.mapEntityComplete(complete);
            newEntity.TaskId = oldEntity.TaskId;

            // Update changes
            context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Delete a complete entity.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="complete">Complete DTO to be deleted</param>
        public void deleteComplete(IDbContext context, DTOComplete complete)
        {
            // Delete entity
            delete<Complete>(context, Mapper.mapEntityComplete(complete));
        }

        // Timer ====================================================
        /// <summary>
        /// Check if the timer record exist in db.
        /// </summary>
        /// <param name="context">context of the database.</param>
        /// <param name="timer">Timer record to check for existence.</param>
        /// <returns>True if timer record exist. Otherwise false.</returns>
        public bool timerExist(IDbContext context, DTOTimer timer)
        {
            var query = getAll<Timer>(context)
                        .Where(x => x.TaskId == timer.TaskId)
                        .Any();

            return query; 
        }
        /// <summary>
        /// Insert a timer and save changes.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="timer">Timer DTO to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertTimerAndSaveChanges(IDbContext context, DTOTimer timer)
        {
            // Throw exception if fk is invalid
            throwExceptionIfForeignKeyInvalid(timer.TaskId);

            // Get entity from dto
            var entity = Mapper.mapEntityTimer(timer);

            return insertAndSaveChanges<Timer>(context, entity);
        }

        /// <summary>
        /// Update timer.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="timer">Timer DTO to be updated.</param>
        public void updateTimer(IDbContext context, DTOTimer timer)
        {
            // Get old entity
            var oldEntity = context.Timers.Where(x => x.Id == timer.Id).FirstOrDefault();

            // Get new entity
            var newEntity = Mapper.mapEntityTimer(timer);
            oldEntity.TaskId = oldEntity.TaskId;

            // Update changes
            update<Timer>(context, newEntity, oldEntity);
        }

        /// <summary>
        /// Delete a timer entity.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="timer">Timer DTO to be deleted</param>
        public void deleteTimer(IDbContext context, DTOTimer timer)
        {
            // Delete entity
            delete<Timer>(context, Mapper.mapEntityTimer(timer));
        }
        
        /// <summary>
        /// Get all the task by requirement id as iqueryable.
        /// </summary>
        /// <param name="context">context of the database.</param>
        /// <param name="task">Task that contains the requirement id fk.</param>
        /// <returns>An iqueryable collection.</returns>
        private IQueryable<Task> getAllByReqId(IDbContext context, DTOTask task)
        {
            throwExceptionIfForeignKeyInvalid(task.RequirementId);

            var query = getAll<Task>(context).Where(x => x.RequirementId == task.RequirementId).AsQueryable();
            return query;
        }
    }
}
