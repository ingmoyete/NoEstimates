using NoEstimates.Core.DTO;
using NoEstimates.repository.Core.Base;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.TaskRepository
{
    public interface ITaskRepository
    {
        // Task
        int getAllTasksNumber(IDbContext context, DTOTask task);
        int getCompletedTasksNumber(IDbContext context, DTOTask task);

        DTOTaskPanel getTaskPanelByTaskId(IDbContext context, DTOTask task);
        bool tasksExistByRequirementId(IDbContext context, DTOTask task);
        DTOIdsNames getIdsAndNamesForProjectAndRequirement(IDbContext context, DTOTask task);
        List<DTOTaskPanel> getListTaskPanelByRequirementId(IDbContext context, DTOTask task);

        DTOTask getTaskById(IDbContext context, DTOTask task);

        int insertTaskAndSaveChanges(IDbContext context, DTOTask task);

        void updateTask(IDbContext context, DTOTask task);

        void deleteTask(IDbContext context, DTOTask task);

        // Highlight
        bool highlightExist(IDbContext context, DTOHighlightColor highlight);

        int insertHighlightColorAndSaveChanges(IDbContext context, DTOHighlightColor highlight);

        void updateHighlightColor(IDbContext context, DTOHighlightColor highlight);

        void deleteHighlightColor(IDbContext context, DTOHighlightColor highlight);

        // Complete
        bool completeExist(IDbContext context, DTOComplete complete);

        int insertCompleteAndSaveChanges(IDbContext context, DTOComplete complete);

        void updateComplete(IDbContext context, DTOComplete complete);

        void deleteComplete(IDbContext context, DTOComplete complete);

        // Timer
        bool timerExist(IDbContext context, DTOTimer timer);

        int insertTimerAndSaveChanges(IDbContext context, DTOTimer timer);

        void updateTimer(IDbContext context, DTOTimer timer);

        void deleteTimer(IDbContext context, DTOTimer timer);
    }
}
