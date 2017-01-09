using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using NoEstimates.Core.DTO;
using referenceArchitecture.Test.Core.Factory;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Linq;
using NoEstimates.Test.Core.Base;
using NoEstimates.Core.DataTableService;

namespace NoEstimates.Test._4.__RepositoryLayer
{
    [TestClass]
    public class RequirementRepositoryTest : BaseTest
    {
        [TestMethod]
        public void getJsonDataTable_with_1tasks_1completes()
        {
            removeAll();
            int Ntasks = 5;

            var projects = insertProjectsAndgetList(1);
            var requirement = insertAndGetRequirementCollection(1, projects.FirstOrDefault()).FirstOrDefault();
            var taskList = insertTasksAndgetList(requirement, Ntasks);

            for (int i = 0; i < getR(1, Ntasks); i++)
            {
                var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(taskList[i]));
            }

            // Arrange
            DataTableParams dataTableParams = new DataTableParams
            {
                iColumns = 7,
                iDisplayLength = 10,
                iDisplayStart = 0,
                iSortCol_0 = 0,
                iSortingCols = 1,
                sColumns = "Name,CreationDate,Status,Tasks,Statistic,Edit,Remove",
                sEcho = "1",
                sSearch = null,
                sSortDir_0 = "desc"
            };

            // Act
            var resultWithStatus = Container.createIRequirementsRepository().getJsonDataTable(Container.createIDbContext(), requirement, dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTORequirementDataTable>(dataTableParams, getRequirementDataTableIQueryable(requirement), getRequirementDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count > 0, "There was no status. There must be one at least");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getJsonDataTable_with_1tasks_0completes()
        {
            removeAll();
            int Ntasks = 3;

            var projects = insertProjectsAndgetList(1);
            var requirement = insertAndGetRequirementCollection(1, projects.FirstOrDefault()).FirstOrDefault();
            var taskList = insertTasksAndgetList(requirement, Ntasks);

            // Arrange
            DataTableParams dataTableParams = new DataTableParams
            {
                iColumns = 7,
                iDisplayLength = 10,
                iDisplayStart = 0,
                iSortCol_0 = 0,
                iSortingCols = 1,
                sColumns = "Name,CreationDate,Status,Tasks,Statistic,Edit,Remove",
                sEcho = "1",
                sSearch = null,
                sSortDir_0 = "desc"
            };

            // Act
            var resultWithStatus = Container.createIRequirementsRepository().getJsonDataTable(Container.createIDbContext(), requirement, dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTORequirementDataTable>(dataTableParams, getRequirementDataTableIQueryable(requirement), getRequirementDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count == 0, "There is one status. Must not be");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getJsonDataTable_with_0tasks_0completes()
        {
            removeAll();

            var projects = insertProjectsAndgetList(1);
            var requirement = insertAndGetRequirementCollection(1, projects.FirstOrDefault()).FirstOrDefault();

            // Arrange
            DataTableParams dataTableParams = new DataTableParams
            {
                iColumns = 7,
                iDisplayLength = 10,
                iDisplayStart = 0,
                iSortCol_0 = 0,
                iSortingCols = 1,
                sColumns = "Name,CreationDate,Status,Tasks,Statistic,Edit,Remove",
                sEcho = "1",
                sSearch = null,
                sSortDir_0 = "desc"
            };

            // Act
            var resultWithStatus = Container.createIRequirementsRepository().getJsonDataTable(Container.createIDbContext(), requirement, dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTORequirementDataTable>(dataTableParams, getRequirementDataTableIQueryable(requirement), getRequirementDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count == 0, "There is one status. Must not be");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getRequirementStatistics_getStatistics_in_DTOStatisticView()
        {
            removeAll();
            int Ntasks = 10;

            var projects = insertProjectsAndgetList(1);
            var requirement = insertAndGetRequirementCollection(1, projects.FirstOrDefault()).FirstOrDefault();
            var taskList = insertTasksAndgetList(requirement, Ntasks);

            for (int i = 0; i < getR(5, Ntasks); i++)
            {
                var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(taskList[i]));
                var timer = insertAndGetTimer(new DTOTimer { TaskId = taskList[i].Id, TimeInSeconds = getR(1500, 3000) });
            }

            // Arrange
            var statistic = new DTOStatisticView
            {
                ItemStatisticId = requirement.Id,
                hp = Container.createHp()
            };

            // Act
            var result = Container.createIRequirementsRepository().getRequirementStatistics(Container.createIDbContext(), statistic);
            
            // Assert
            bool velocitiesAreOk = false;
            bool totalTaskShouldBeEqualOrHigherThanCompleteTask = false;
            foreach (var item in result)
            {
                velocitiesAreOk =  item.VelocityInItemPerSeconds > 0;

                totalTaskShouldBeEqualOrHigherThanCompleteTask = item.TotalItems == item.CompleteItems || item.TotalItems > item.CompleteItems;
            }
            Assert.IsTrue(result != null && result.Count > 0, "It does not return a DTOStatistic collection");
            Assert.IsTrue(velocitiesAreOk && totalTaskShouldBeEqualOrHigherThanCompleteTask, "Velocities are not ok.");

        }


        // Private Methods =====================================
        private IQueryable<DTORequirementDataTable> getRequirementDataTableIQueryable(DTORequirements requirement)
        {
            // Get requirement as a list of DTORequirementDataTable
            var requirementsDataTable = Container.createIDbContext().Requirements.Where(x => x.ProjectId == requirement.ProjectId)
                                .Select(x => new DTORequirementDataTable
                                {
                                    RequirementId = x.Id,
                                    ProjectId = x.ProjectId,
                                    CreationDate = x.CreationDate,
                                    Description = x.Description,
                                    Name = x.Name,
                                    NumberOfTasks = 0,
                                }).AsQueryable();
            return requirementsDataTable;
        }

        private Expression<Func<DTORequirementDataTable, bool>> getRequirementDataTableFiltering(DataTableParams dataTableParams)
        {
            var DateHp = Container.createIDateHp();
            var dataTableService = Container.createIDataTableService();

            Expression<Func<DTORequirementDataTable, bool>> filtering;
            var date = dataTableService.getDateTimeField(dataTableParams.sSearch, DateHp.OnlyDateFormat, "date", ':');
            if (!DateHp.isWholeMinDate(date))
            {
                filtering = (x => DbFunctions.TruncateTime(x.CreationDate) == date.Date);
            }
            else
            {
                filtering = (x =>
                                x.Name.Contains(dataTableParams.sSearch)
                                || SqlFunctions.StringConvert((decimal)x.Status).Trim().Contains(dataTableParams.sSearch));
            }

            return filtering;
        }
    }
}
