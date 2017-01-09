using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOIdsNames : DTOBase
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int RequirementId { get; set; }
        public string RequirementName { get; set; }
    }
}
