using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.service.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.RequirementService
{
    public interface IRequirementService : IBaseService
    {
        void deleteRequirement(DTORequirements requirement);
        DataTableJson<DTORequirementDataTable> getJsonDataTable(DTORequirements requirement, DataTableParams dataTableparms);
        DTORequirementView getAllRequirements(DTORequirements requirement);
        int insertRequirement(DTORequirements Requirement);
        void updateRequirement(DTORequirements requirement);
    }
}
