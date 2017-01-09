using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTORequirementView : DTOBase
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }
}
