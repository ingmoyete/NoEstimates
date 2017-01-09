using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTORequirementDataTable
    {   
        public int RequirementId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Status { get; set; }
        public DateTime CreationDate { get; set; }

        public bool ThereAreTasks { get; set; }
        public int NumberOfTasks { get; set; }
    }
}
