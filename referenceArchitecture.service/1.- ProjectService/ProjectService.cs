using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.repository._0.__Edmx;
using NoEstimates.repository.ProjectRepository;
using NoEstimates.repository.RequirementsRepository;
using NoEstimates.service.Core.Base;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.ProjectService
{
    public class ProjectService : BaseService , IProjectService
    {
        /// <summary>
        /// Project repository.
        /// </summary>
        private IProjectRepository projectsRepository;

        /// <summary>
        /// Requirement repository.
        /// </summary>
        private IRequirementsRepository requirementRepository;

        /// <summary>
        /// Constructor used to inject projects repository, Resources, and context.
        /// </summary>
        /// <param name="_exampleRepository">Example repository to be injected.</param>
        public ProjectService(IProjectRepository _projectsRepository, IRequirementsRepository _requirementRepository)
        {
            this.projectsRepository = _projectsRepository;
            this.requirementRepository = _requirementRepository;
        }

        /// <summary>
        /// Get all projects.
        /// </summary>
        /// <returns>A collection with all projects.</returns>
        public List<DTOProject> getAllProjects()
        {
            // Create context
            using (DbContext)
            {
                var allProjects = projectsRepository.getAllProjects(DbContext);

                return allProjects;
            }
        }

        /// <summary>
        /// Create project.
        /// </summary>
        /// <param name="project">DTO Project to create.</param>
        public int createProject(DTOProject project)
        {
            using (DbContext)
            {
                // Validate the project
                if (!projectToInsertIsOk(project)) return -1;

                // Set the date fields
                project.CreationDate = DateTime.Now;
                project.FinalizationDate = (DateTime)SqlDateTime.MinValue;

                // Create project
                int id = projectsRepository.createProjectAndSaveChanges(DbContext, project);

                // Return id
                return id;
            }
        }

        /// <summary>
        /// Update a project.
        /// </summary>
        /// <param name="project">Project object that contains the new information to be updated.</param>
        public void updateProject(DTOProject project)
        {
            using (DbContext)
            {
                // Validate the project
                if (!projectToUpdateIsOk(project)) return;

                // Set dates
                var oldProject = projectsRepository.getProjectById(DbContext, project);
                project.CreationDate = oldProject.CreationDate;
                project.FinalizationDate = oldProject.FinalizationDate;

                // Update project
                projectsRepository.updateProject(DbContext, project);
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Get the dataTablejson dto.
        /// </summary>
        /// <param name="dataTableParams">Parameters of the datatable.</param>
        /// <returns>A DataTableJson.</returns>
        public DataTableJson<DTOProjectDataTable> getDataTableJson(DataTableParams dataTableParams)
        {
            using (DbContext)
            {
                var dataTableJson = projectsRepository.getDataTableJson(DbContext, dataTableParams);
                return dataTableJson;
            }
        }

        /// <summary>
        /// Remove a project.
        /// </summary>
        /// <param name="project">DTO project to be removed.</param>
        public void deleteProject(DTOProject project)
        {
            using (DbContext)
            {
                // Check if the project can be delted
                if (!projectToDeleteIsOk(project)) return;

                // Delete project and save changes
                projectsRepository.deleteProject(DbContext, project);
                DbContext.SaveChanges();
            }
        }

        #region Private Methods
        /// <summary>
        /// Check if the project to insert is ok.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">Project to be inserted.</param>
        /// <returns>True if project to insert is ok, otherwise false.</returns>
        private bool projectToInsertIsOk(DTOProject project)
        {
            checkIfNameAlreadyExist(project);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// Check if the project to insert is ok.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">Project to be inserted.</param>
        /// <returns>True if project to insert is ok, otherwise false.</returns>
        private bool projectToUpdateIsOk(DTOProject project)
        {
            checkIfNameAlreadyExist(project);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// Check if the project can be deleted.
        /// </summary>
        /// <param name="project">Project to be checked.</param>
        /// <returns>True if project can be delted. Otherwise false.</returns>
        private bool projectToDeleteIsOk(DTOProject project)
        {
            // Add error if there is requirements
            var requirementsExist = requirementRepository.requirementExistsByProjectId(DbContext, new DTORequirements { ProjectId = project.Id });
            if (requirementsExist) ControllerUI.ModelStateService.AddModelError(SummaryError, GlobalResources["deleteProject"]);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// If name already exist, an error is added to the modelstate.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        /// <param name="project">A project object.</param>
        private void checkIfNameAlreadyExist(DTOProject project)
        {
            // Check if name already exists
            bool nameAlreadyExist = projectsRepository.getProjectByName(DbContext, project.Name) != null;
            if (nameAlreadyExist) ControllerUI.ModelStateService.AddModelError("Name", GlobalResources["nameEntity"]);
        }

        #endregion
    }
}
