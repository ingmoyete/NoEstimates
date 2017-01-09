using MathNet.Numerics;
using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.Core.Enums;
using NoEstimates.repository.RequirementsRepository;
using NoEstimates.repository.TaskRepository;
using NoEstimates.service.Core.Base;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.RequirementService
{
    public class RequirementService : BaseService, IRequirementService
    {

        /// <summary>
        /// Requirement repository.
        /// </summary>
        private IRequirementsRepository requirementRepository;

        /// <summary>
        /// Task repository.
        /// </summary>
        private ITaskRepository taskRepository;

        /// <summary>
        /// Constructor used to inject the requirement repository.
        /// </summary>
        public RequirementService(IRequirementsRepository _requirementRepository, ITaskRepository _taskRepository)
        {
            this.requirementRepository = _requirementRepository;
            this.taskRepository = _taskRepository;
        }

        /// <summary>
        /// Get json for datatable in requirements.
        /// </summary>
        /// <param name="requirement">Requirement that contains the projectId.</param>
        /// <param name="dataTableparms">Parameters sent from ajax.</param>
        /// <returns>A DataTableJson object.</returns>
        public DataTableJson<DTORequirementDataTable> getJsonDataTable(DTORequirements requirement, DataTableParams dataTableparms)
        {
            using (DbContext)
            {
                var query = requirementRepository.getJsonDataTable(DbContext, requirement, dataTableparms);
                return query;
            }
        }
        
        /// <summary>
        /// Get all the requirements.
        /// </summary>
        /// <returns>A collection of DTO requirements.</returns>
        public DTORequirementView getAllRequirements(DTORequirements requirement)
        {
            using (DbContext)
            {
                // Get requirements and project name
                var project = requirementRepository.getProjectNameAndIdByRequirement(DbContext, requirement);
                
                    // Return requirement view
                return new DTORequirementView
                {
                    ProjectId = requirement.ProjectId,
                    ProjectName = project.Name,
                };
            }
        }

        /// <summary>
        /// Insert a requirement in db.
        /// </summary>
        /// <param name="Requirement">Requirement to be inserted.</param>
        /// <returns>The id of the inserted record.</returns>
        public int insertRequirement(DTORequirements Requirement)
        {
            using (DbContext)
            {
                // Validate insert requirement
                if (!insertRequirementIsOk(Requirement)) return -1;

                // Set dates
                Requirement.CreationDate = DateTime.Now;
                Requirement.FinalizationDate = (DateTime)SqlDateTime.MinValue;

                // Insert requirement
                int id = requirementRepository.insertRequirementAndSaveChanges(DbContext, Requirement);
                return id;
            }
        }

        /// <summary>
        /// Update a requirement.
        /// </summary>
        /// <param name="requirement">Requirement to update.</param>
        public void updateRequirement(DTORequirements requirement)
        {
            using (DbContext)
            {
                // Validate requirement to update
                if (!updateRequirementIsOk(requirement)) return;

                // Set creation and finalization date
                var oldRequirement = requirementRepository.getRequirementById(DbContext, requirement);
                requirement.CreationDate = oldRequirement.CreationDate;
                requirement.FinalizationDate = oldRequirement.FinalizationDate;

                // Update requirement and save changes
                requirementRepository.updateRequirement(DbContext, requirement);
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a requirement.
        /// </summary>
        /// <param name="requirement">Requirement to be deleted.</param>
        public void deleteRequirement(DTORequirements requirement)
        {
            using (DbContext)
            {
                // Validate if the requirement can be deleted
                if (!deleteRequirementIsOk(requirement)) return;

                // Delete requirement
                requirementRepository.deleteRequirement(DbContext, requirement);
                DbContext.SaveChanges();
            }
        }

        #region Private Methods
        /// <summary>
        /// Check if a requirement can be deleted.
        /// </summary>
        /// <param name="requirement">Requirement to be checked.</param>
        /// <returns></returns>
        private bool deleteRequirementIsOk(DTORequirements requirement)
        {
            var existTasksForThisReq = taskRepository.tasksExistByRequirementId(DbContext, new DTOTask { RequirementId = requirement.Id });
            if (existTasksForThisReq) ControllerUI.ModelStateService.AddModelError(SummaryError, GlobalResources["deleteRequirement"]);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// Validate the update requirement method.
        /// </summary>
        /// <param name="dbContext">Context of the database.</param>
        /// <param name="requirement">Requirement to be validated.</param>
        /// <returns>True if validation is successful. Otherwise false.</returns>
        private bool updateRequirementIsOk(DTORequirements requirement)
        {
            addErrorIfNameExist(requirement);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// Validate the insert requirement method.
        /// </summary>
        /// <param name="dbContext">Context of the database.</param>
        /// <param name="requirement">Requirement to be validated.</param>
        /// <returns>True if validation is successful. Otherwise false.</returns>
        private bool insertRequirementIsOk(DTORequirements requirement)
        {
            addErrorIfNameExist(requirement);

            return ControllerUI.ModelStateService.IsValid;
        }

        /// <summary>
        /// Add error to modelState if the name already exists.
        /// </summary>
        /// <param name="dbContext">Context of the database.</param>
        /// <param name="requirement">The requirement that contains the name to be validated.</param>
        private void addErrorIfNameExist(DTORequirements requirement)
        {
            bool nameExist = requirementRepository.getRequirementByName(DbContext, requirement) != null;

            if (nameExist) ControllerUI.ModelStateService.AddModelError("Name", GlobalResources["nameEntity"]);
        }
        #endregion

    }
}
