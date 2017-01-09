using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoEstimates.Core.DTO;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.Core.Resources;
using NoEstimates.repository.TaskRepository;
using NoEstimates.service.Core.Base;
using System.Data.SqlTypes;

namespace NoEstimates.service.TaskService
{
    public class TaskService : BaseService, ITaskService
    {        
        /// <summary>
        /// Task service.
        /// </summary>
        private ITaskRepository taskRepository;

        /// <summary>
        /// Constructor to iject resources, context, and task service.
        /// </summary>
        public TaskService(ITaskRepository _taskRepository)
        {
            this.taskRepository = _taskRepository;
        }

        // Task ==================================================
        /// <summary>
        /// Get all tasks with project and requirement names and ids.
        /// </summary>
        /// <param name="requirement">Requirement to filter by.</param>
        /// <returns>A DTO Task view with the collection of tasks, ids and names for both requirement and project.</returns>
        public DTOTaskView getTaskView(DTOTask task)
        {
            using (DbContext)
            {
                // Get the parts of the dto task view
                var idsAndNames = taskRepository.getIdsAndNamesForProjectAndRequirement(DbContext, task);
                var listDTOTaskPanel = taskRepository.getListTaskPanelByRequirementId(DbContext, task);
                var numberOfCompletedTasks = taskRepository.getCompletedTasksNumber(DbContext, task);
                var numberOfTotalTasks = taskRepository.getAllTasksNumber(DbContext, task);

                // Fill the dto task view
                return new DTOTaskView
                {
                    IdsNames = idsAndNames,
                    ListTaskPanels = listDTOTaskPanel,
                    CompletedTasks = numberOfCompletedTasks,
                    TotalTasks = numberOfTotalTasks 
                };
            }

        }

        /// <summary>
        /// Insert a task in db.
        /// </summary>
        /// <param name="task">Task to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertTask(DTOTask task)
        {
            using (DbContext)
            {
                // Validate insert task
                if (!taskToInsertIsOk(task)) return -1;

                // Set the creation date
                task.CreationDate = DateTime.Now;
                
                // Insert task and return id
                return taskRepository.insertTaskAndSaveChanges(DbContext, task);
            }
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        /// <param name="task">Task to be updated.</param>
        public void updateTask(DTOTask task)
        {
            using (DbContext)
            {
                // Validate task to update
                if (!taskToUpdateIsOk(task)) return;

                // Update task and save changes
                taskRepository.updateTask(DbContext, task);
                DbContext.SaveChanges();
            }
        }

        public void deleteWholeTask(DTOTask task)
        {
            using (DbContext)
            {
                // Get task, highlight, complete and timer
                var wholeTask = taskRepository.getTaskPanelByTaskId(DbContext, task);

                // Remove task, highlight, complete and timer
                if (wholeTask.Highlight.Id > 0) taskRepository.deleteHighlightColor(DbContext, wholeTask.Highlight);
                if (wholeTask.Complete.Id > 0) taskRepository.deleteComplete(DbContext, wholeTask.Complete);
                if (wholeTask.Timer.Id > 0) taskRepository.deleteTimer(DbContext, wholeTask.Timer);

                if (wholeTask.Task.Id > 0) taskRepository.deleteTask(DbContext, wholeTask.Task);

                // Save changes
                DbContext.SaveChanges();
            }
        }
        // Highlight ===========================================
        /// <summary>
        /// Insert or update a DTO highlight.
        /// </summary>
        /// <param name="highlight">A DTO Highlight to insert or update.</param>
        public int writeHighlight(DTOHighlightColor highlight)
        {
            using (DbContext)
            {
                int ret;

                // True if the highlight already exist in db. Otherwise false
                bool highlightExist = taskRepository.highlightExist(DbContext, highlight);

                // Update highlight if it already exist. Insert it if it does not.
                if (highlightExist)
                {
                    taskRepository.updateHighlightColor(DbContext, highlight);
                    DbContext.SaveChanges();
                    ret = highlight.Id;
                }
                else
                {
                    ret = taskRepository.insertHighlightColorAndSaveChanges(DbContext, highlight);
                }

                return ret;
            }
        }

        // Complete =============================================
        /// <summary>
        /// Insert or update a complete record in db.
        /// </summary>
        /// <param name="complete">Complete record to insert or update.</param>
        /// <returns>The id of the inserted/updated record.</returns>
        public int writeComplete(DTOComplete complete)
        {
            using (DbContext)
            {

                int ret;

                // Set finalization date
                complete.FinalizationDate = getFinalizationDate(complete);

                // True if complete exist. Otherwise false.
                bool completeExistInDb = taskRepository.completeExist(DbContext, complete);

                // Update complete if it already exist in db
                if (completeExistInDb)
                {
                    taskRepository.updateComplete(DbContext, complete);
                    DbContext.SaveChanges();
                    ret = complete.Id;
                }
                // Insert complete if it is not found in db
                else
                {
                    ret = taskRepository.insertCompleteAndSaveChanges(DbContext, complete);
                }

                return ret;
            }
        }

        // Timer =================================================
        /// <summary>
        /// Insert or update a timer in db.
        /// </summary>
        /// <param name="timer">Timer to be inserted or updated.</param>
        /// <returns>The id of the inserted/updated record.</returns>
        public int writeTimer(DTOTimer timer)
        {
            using (DbContext)
            {
                int ret;

                // True if timer exist. Otherwise false.
                bool timerAlreadyExist = taskRepository.timerExist(DbContext, timer);

                // Update timer if it already exist
                if (timerAlreadyExist)
                {
                    taskRepository.updateTimer(DbContext, timer);
                    DbContext.SaveChanges();
                    ret = timer.Id;
                }
                // Insert timer if it does not exist
                else
                {
                    ret = taskRepository.insertTimerAndSaveChanges(DbContext, timer);
                }

                // Return inserted/updated record
                return ret;
            }
        }

        // Validations ===========================================

        /// <summary>
        /// Validate if the task to insert is ok.
        /// </summary>
        /// <param name="tasks">Task to be inserted.</param>
        /// <returns>True if task is ok. Otherwise false.</returns>
        private bool taskToInsertIsOk(DTOTask tasks)
        {
            return true;
        }

        /// <summary>
        /// Validate if update is ok.
        /// </summary>
        /// <param name="task">Task to validate</param>
        /// <returns>True if validation successful. Otherwiser false.</returns>
        private bool taskToUpdateIsOk(DTOTask task)
        {
            return true;

        }

        #region Private Methods
        /// <summary>
        /// Get the finalization date for DTOComplete.
        /// </summary>
        /// <param name="complete">Complete DTO that contains the IsComplete parameter.</param>
        /// <returns>Current date if IsComplete is true. Otherwise it return the minumun datetime.</returns>
        private DateTime getFinalizationDate(DTOComplete complete)
        {
            var finalizationDate = (complete.IsComplete) ? DateTime.Now : (DateTime)SqlDateTime.MinValue;
            return finalizationDate;
        }
        #endregion
    }
}
