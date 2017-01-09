using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOComplete: DTOBase
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public bool IsComplete { get; set; }
        public DateTime FinalizationDate { get; set; }

        public string IsCompleteColorClass { get { return "panel-green";  } }

        public string disableIfComplete { get { return IsComplete ? "disabled" : ""; } }
    }
}
