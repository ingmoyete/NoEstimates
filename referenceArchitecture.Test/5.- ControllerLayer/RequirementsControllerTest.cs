using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoEstimates.Core.DTO;
using System.Data.SqlTypes;
using referenceArchitecture.Test.Core.Factory;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using NoEstimates.Test.Core.Base;
using NoEstimates.Core.DataTableService;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace NoEstimates.Test._5.__ControllerLayer
{
    [TestClass]
    public class RequirementsControllerTest : BaseTest
    {
        [TestMethod]
        public void CreateRequirement_creates_record_and_return_json()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirementToInsert = insertAndgetRequirement(getRandomRequirement(project));

            // Arrange
            var requirementController = Container.createRequirementsController();

            // Act
            var result = requirementController.CreateRequirement(requirementToInsert) as JsonResult;

            // Assert
            var expectedRecord = Container.createIRequirementsRepository().getByAllFieldsExceptId(Container.createIDbContext(), requirementToInsert);
            Assert.IsTrue(result != null, "It is not a json result");
            Assert.IsTrue(expectedRecord.Count == 1, "The record was not inserted or there ar more than one record in the db.");
            Assert.IsTrue
            (
                expectedRecord.FirstOrDefault().Name == requirementToInsert.Name
                && expectedRecord.FirstOrDefault().Description == requirementToInsert.Description
                && expectedRecord.FirstOrDefault().IsComplete == requirementToInsert.IsComplete,
                "The record was not inserted."
            );
        }

        [TestMethod]
        public void Index_return_view_and_all_requirements_for_a_project()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirements = insertAndGetRequirementCollection(3, project);

            // Arrange
            var requirementController = Container.createRequirementsController();
            
            // Act
            var result = requirementController.Index(requirements.FirstOrDefault()) as ViewResultBase;

            // Assert
            var model = result.Model as DTORequirementView;
            Assert.IsTrue(result != null && result.ViewName == "Index", "It is not a view result or does not return Index");
            //Assert.IsTrue(model.ListOfRequirements.Count == requirements.Count, "The requirements collection was not retrieved.");

        }

        [TestMethod]
        public void DeleteRequirement_delete_record_and_return_true_json()
        {
            var projectToCreate = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(projectToCreate));
            //var tasks = insertTasksAndgetList(requirement, 5);

            // Arrange
            var requirementController = Container.createRequirementsController();

            // Act
            var result = requirementController.DeleteRequirement(requirement) as JsonResult;

            // Assert
            var resultAsBool = result.Data as bool?;
            var deletedRecord = Container.createIRequirementsRepository().getRequirementById(Container.createIDbContext(), new DTORequirements { ProjectId = requirement.ProjectId, Id = requirement.Id });
            Assert.IsTrue(result != null && resultAsBool.Value, "It is not json result and does not return true.");
            Assert.IsTrue(deletedRecord == null, "The record was not deleted in the db.");
        }

        [TestMethod]
        public void DeleteRequirement_does_not_delete_and_show_error_in_modelState_ThereAreRequirements()
        {
            var projectToCreate = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(projectToCreate));
            var tasks = insertTasksAndgetList(requirement, 5);

            // Arrange
            var requirementController = Container.createRequirementsController();

            // Act
            var result = requirementController.DeleteRequirement(requirement) as JsonResult;

            // Assert
            var resultAsBool = result.Data as bool?;
            var deletedRecord = Container.createIProjectRepository().getProjectById(Container.createIDbContext(), new DTOProject { Id = projectToCreate.Id });
            Assert.IsTrue(result != null && resultAsBool.Value, "It is not json result and does not return true.");
            Assert.IsTrue(deletedRecord != null, "The record was deleted in the db.");
        }
    }
}
