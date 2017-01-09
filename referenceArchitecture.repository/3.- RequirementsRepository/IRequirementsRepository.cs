using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.RequirementsRepository
{
    public interface IRequirementsRepository
    {
        bool requirementExistsByProjectId(IDbContext context, DTORequirements requirement);
        DataTableJson<DTORequirementDataTable> getJsonDataTable(IDbContext context, DTORequirements requirement, DataTableParams dataTableparms);
        DTOProject getProjectNameAndIdByRequirement(IDbContext context, DTORequirements requirement);
        DTORequirements getRequirementById(IDbContext context, DTORequirements requirement);
        DTORequirements getRequirementByName(IDbContext context, DTORequirements requirement);
        int insertRequirementAndSaveChanges(IDbContext context, DTORequirements requirement);
        void updateRequirement(IDbContext context, DTORequirements requirement);
        void deleteRequirement(IDbContext context, DTORequirements requirement);
        List<DTORequirements> getByAllFieldsExceptId(IDbContext context, DTORequirements requirement);
        List<DTOStatistic> getRequirementStatistics(IDbContext context, DTOStatisticView statistic);
    }
}
