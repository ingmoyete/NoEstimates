using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.Test.Core.Factory;
using NoEstimates.Core.DTO;
using System.Web.Mvc;
using System.Linq;
using System.Data.SqlTypes;
using System.Collections.Generic;
using NoEstimates.repository._0.__Edmx;
using NoEstimates.Test.Core.Base;
using NoEstimates.Core.DataTableService;
using System.Linq.Expressions;
using System.Data.Entity;

namespace NoEstimates.Test.ControllerLayer
{
    [TestClass]
    public class ProjectsControllerTest : BaseTest
    {
        private Random r = new Random();

        [TestMethod]
        public void CreateProject_creates_record_and_return_json()
        { 
            var projectToCreate = getRandomProject();

            // Arrange
            var projectsController = Container.createProjectsController();

            // Act
            var result = projectsController.CreateProject(projectToCreate) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var insertedRecord = Container.createIProjectRepository().getProjectById(Container.createIDbContext(), new DTOProject { Id = resultAsInt.Value });
            Assert.IsTrue(result != null, "It is not json result.");
            Assert.IsTrue(insertedRecord != null, "The record was not inserted in the db.");
            Assert.IsTrue
            (
                insertedRecord.Description == projectToCreate.Description
                && insertedRecord.Name == projectToCreate.Name
                && datesAreEqual(insertedRecord.CreationDate, projectToCreate.CreationDate)
                && datesAreEqual(insertedRecord.FinalizationDate, projectToCreate.FinalizationDate)
                && insertedRecord.IsCompleted == projectToCreate.IsCompleted,
                "The properties does not map."
            );
        }

        [TestMethod]
        public void DeleteProject_delete_record_and_return_true_json()
        {
            var projectToCreate = insertAndgetProject(getRandomProject());

            // Arrange
            var projectsController = Container.createProjectsController();

            // Act
            var result = projectsController.DeleteProject(projectToCreate) as JsonResult;

            // Assert
            var resultAsBool = result.Data as bool?;
            var deletedRecord = Container.createIProjectRepository().getProjectById(Container.createIDbContext(), new DTOProject { Id = projectToCreate.Id });
            Assert.IsTrue(result != null && resultAsBool.Value, "It is not json result and does not return true.");
            Assert.IsTrue(deletedRecord == null, "The record was not deleted in the db.");
        }

        [TestMethod]
        public void DeleteProject_does_not_delete_and_show_error_in_modelState_ThereAreRequirements()
        {
            var projectToCreate = insertAndgetProject(getRandomProject());
            var requirements = insertAndGetRequirementCollection(3, projectToCreate);

            // Arrange
            var projectsController = Container.createProjectsController();

            // Act
            var result = projectsController.DeleteProject(projectToCreate) as JsonResult;
            
            // Assert
            var resultAsBool = result.Data as bool?;
            var deletedRecord = Container.createIProjectRepository().getProjectById(Container.createIDbContext(), new DTOProject { Id = projectToCreate.Id });
            Assert.IsTrue(result != null && resultAsBool.Value, "It is not json result and does not return true.");
            Assert.IsTrue(deletedRecord != null, "The record was deleted in the db.");
        }

        [TestMethod]
        public void CreateProject_dont_creates_record_and_there_is_ModelstateError()
        {
            var projectToCreate = getRandomProject();

            // Arrange
            var projectsController = Container.createProjectsController();
            var projectsController2 = Container.createProjectsController();

            // Act
            var controllerResult = projectsController.CreateProject(projectToCreate) as JsonResult;
            var controllerResult2 = projectsController2.CreateProject(projectToCreate) as JsonResult;

            // Assert
            var insertedRecord = Container.createIProjectRepository().getProjectByAllFieldsExceptId(Container.createIDbContext(), projectToCreate);
            Assert.IsTrue(insertedRecord.Count == 1, "The record was inserted with an error.");
            Assert.IsTrue
            (
                projectsController2.ModelState["Name"].Errors.FirstOrDefault() != null,
                "There record was not error."
            );
        }

        [TestMethod]
        public void Index_return_allProjects()
        {
            // Arrange
            var projectService = Container.createIProjectService();
            var projectsController = Container.createProjectsController();

            // Act
            var serviceResult = projectService.getAllProjects();
            var controllerResult = projectsController.Index() as ViewResultBase;

            // Assert
            List<Project> allProjectsFromContext;
            using (var context = Container.createIDbContext())
            {
                allProjectsFromContext = context.Projects.ToList();
            }
            Assert.IsTrue(controllerResult != null && controllerResult.ViewName == "Index", "Index view is not returned.");
            Assert.IsTrue(allProjectsFromContext.Count == serviceResult.Count, "Project service get a different number of records that if you make the query directly to db");
            Assert.IsTrue
            (
                serviceResult.Count > 0,
                "There is no records in DB or maybe it is not returning all the records."
            );
        }

        [TestMethod]
        public void UpdateProject_updates_projects_and_return_json()
        {
            // Old Project
            var oldProject = Container.createIProjectRepository().getAllProjects(Container.createIDbContext()).FirstOrDefault();

            // New project
            var randonDescription = "DescUp-" + r.Next(1000).ToString();
            var randonName = "NamUp-" + r.Next(1000).ToString();
            var randonIsComplete = r.Next(0, 1) == 1;
            var newProject = new DTOProject
            {
                Id = oldProject.Id, // <-- Id of the old project
                Description = randonDescription,
                Name = randonDescription,
                IsCompleted = randonIsComplete,
                CreationDate = (DateTime)SqlDateTime.MinValue,
                FinalizationDate = (DateTime)SqlDateTime.MaxValue
            };
            
            // Arrange
            var projectsController = Container.createProjectsController();

            // Act
            var result = projectsController.UpdateProject(newProject) as JsonResult;

            // Assert
            var expectedOldRecord = Container.createIProjectRepository().getProjectByAllFieldsExceptId(Container.createIDbContext(), oldProject);
            var expectedNewRecord = Container.createIProjectRepository().getProjectByAllFieldsExceptId(Container.createIDbContext(), newProject);

            Assert.IsTrue(result != null, "It is not json result.");
            Assert.IsTrue(expectedOldRecord.Count == 0 && expectedNewRecord.Count >= 1, "The record was not updated.");
            Assert.IsTrue
            (
                expectedNewRecord.FirstOrDefault().Description == newProject.Description
                && expectedNewRecord.FirstOrDefault().Name == newProject.Name,
                "There record was updated successfully."
            );
        }

    }
}
