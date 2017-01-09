using NoEstimates.Core.DTO;
using NoEstimates.service.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.TaskService
{
    public interface ITaskService : IBaseService
    {
        // Task
        DTOTaskView getTaskView(DTOTask task);
        int insertTask(DTOTask task);
        void updateTask(DTOTask task);

        void deleteWholeTask(DTOTask task);

        // Highlight 
        int writeHighlight(DTOHighlightColor highlight);

        // Complete
        int writeComplete(DTOComplete complete);

        // Timer
        int writeTimer(DTOTimer timer);
    }
}
