using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.service.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.ProjectService
{
    public interface IProjectService : IBaseService
    {
        void deleteProject(DTOProject project);
        DataTableJson<DTOProjectDataTable> getDataTableJson(DataTableParams dataTableParams);
        int createProject(DTOProject project);
        List<DTOProject> getAllProjects();
        void updateProject(DTOProject project);
    }
}
