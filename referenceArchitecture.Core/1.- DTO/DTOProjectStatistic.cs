using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOProjectStatistic
    {
        public int RequirementId { get; set; }
        public int ProjectId { get; set; }
        public int TotalTasks { get; set; }
        public int TotalCompleteTasks { get; set; }
        public int Time { get; set; }
        public int AcumulativeTime { get; set; }
        public DateTime FinalizationDate { get; set; }
    }
}
