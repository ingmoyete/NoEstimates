using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOProjectDataTable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public double Status { get; set; }
        public int ProjectId { get; set; }

        public int NumberOfTasks { get; set; }
        public bool ThereAreRequirements { get; set; }
    }
}
