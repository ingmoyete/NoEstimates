using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.ProjectRepository
{
    public interface IProjectRepository
    {
        List<DTOStatistic> getProjectStatistic(IDbContext context, DTOStatisticView statistic);
        DataTableJson<DTOProjectDataTable> getDataTableJson(IDbContext context, DataTableParams dataTableParams);

        int createProjectAndSaveChanges(IDbContext context, DTOProject project);

        List<DTOProject> getAllProjects(IDbContext context);

        void deleteProject(IDbContext context, DTOProject project);

        void updateProject(IDbContext context, DTOProject project);

        DTOProject getProjectById(IDbContext context, DTOProject project);

        DTOProject getProjectByName(IDbContext context, string name);

        List<DTOProject> getProjectByAllFieldsExceptId(IDbContext context, DTOProject project);
    }
}
