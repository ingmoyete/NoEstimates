using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOTaskView : DTOBase
    {
        public DTOIdsNames IdsNames { get; set; }
        public List<DTOTaskPanel> ListTaskPanels { get; set; }

        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }
    }
}
