using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoEstimates.Test.Core.Base;
using System.Linq.Expressions;
using NoEstimates.Core.DTO;
using referenceArchitecture.Test.Core.Factory;
using System.Data.Entity;
using System.Linq;
using NoEstimates.Core.DataTableService;

namespace NoEstimates.Test._4.__RepositoryLayer
{
    [TestClass]
    public class ProjectRepositoryTest : BaseTest
    {
        [TestMethod]
        public void getJsonDataTable_with_1requirements_1tasks_1completes()
        {
            removeAll();
            int Nprojects = 5;
            int Nrequirements = 3;
            int Ntasks = 5;

            var projects = insertProjectsAndgetList(Nprojects);
            foreach (var project in projects)
            {
                var requirements = insertAndGetRequirementCollection(Nrequirements, project);

                foreach (var req in requirements)
                {
                    var taskList = insertTasksAndgetList(req, Ntasks);

                    for (int i = 0; i < getR(1, Ntasks); i++)
                    {
                        var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(taskList[i]));
                    }
                }
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
            var projectRespository = Container.createIProjectRepository();

            // Act
            var resultWithStatus = projectRespository.getDataTableJson(Container.createIDbContext(), dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTOProjectDataTable>(dataTableParams, getProjectDataTableIQueryable(), getProjectDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count > 0, "There was no status. There must be one at least");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getJsonDataTable_with_1requirements_1tasks_0completes()
        {
            removeAll();
            int Nprojects = 3;
            int Nrequirements = 4;
            int Ntasks = 10;

            var projects = insertProjectsAndgetList(Nprojects);
            foreach (var project in projects)
            {
                var requirements = insertAndGetRequirementCollection(Nrequirements, project);

                foreach (var req in requirements)
                {
                    var taskList = insertTasksAndgetList(req, Ntasks);
                }
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
            var projectRespository = Container.createIProjectRepository();

            // Act
            var resultWithStatus = projectRespository.getDataTableJson(Container.createIDbContext(), dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTOProjectDataTable>(dataTableParams, getProjectDataTableIQueryable(), getProjectDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count == 0, "All the status should be 0 %");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getJsonDataTable_with_1requirements_and_0tasks_0completes()
        {
            removeAll();
            int Nprojects = 3;
            int Nrequirements = 4;

            var projects = insertProjectsAndgetList(Nprojects);
            foreach (var project in projects)
            {
                var requirements = insertAndGetRequirementCollection(Nrequirements, project);
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
            var projectRespository = Container.createIProjectRepository();

            // Act
            var resultWithStatus = projectRespository.getDataTableJson(Container.createIDbContext(), dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTOProjectDataTable>(dataTableParams, getProjectDataTableIQueryable(), getProjectDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();

            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count == 0, "There was a status detected and it should be 0");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getJsonDataTable_with_0requirements_0tasks_0completes()
        {
            removeAll();

            var projects = insertProjectsAndgetList(3);

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
            var projectRespository = Container.createIProjectRepository();

            // Act
            var resultWithStatus = projectRespository.getDataTableJson(Container.createIDbContext(), dataTableParams);

            // Assert
            var orderedList = Container.createIDataTableService().getIQueryableSource<DTOProjectDataTable>(dataTableParams, getProjectDataTableIQueryable(), getProjectDataTableFiltering(dataTableParams)).ToList();
            var getAllWithStatusHigherThanZero = resultWithStatus.aaData.Where(x => x.Status > 0).ToList();
            Assert.IsTrue(resultWithStatus.aaData.Count == Container.createIDbContext().Projects.ToList().Count, "Not all the projects are returned.");
            Assert.IsTrue(resultWithStatus != null && resultWithStatus.aaData != null && resultWithStatus.aaData.Count > 0, "The json object is not well formated.");
            Assert.IsTrue(getAllWithStatusHigherThanZero.Count == 0, "There was a status detected and it should be 0");
            Assert.IsTrue(areSameObjectsCollection(orderedList.Cast<object>().ToList(), resultWithStatus.aaData.Cast<object>().ToList()), "Both collection do not have the same order.");

        }

        [TestMethod]
        public void getProjectStatistic_getStatistics_in_DTOStatisticView()
        {
            removeAll();
            int NRequirements = 6;
            int NumberOfCompleteReq = 4;
            int Ntasks = 6;
            int NFrom = 3; // <- This should be lower than NTasks

            var projects = insertProjectsAndgetList(10);
            foreach (var project in projects)
            {
                var requirements = insertAndGetRequirementCollection(NRequirements, project);
                for (int i = 0; i < requirements.Count; i++)
                {
                    var taskList = insertTasksAndgetList(requirements[i], Ntasks);

                    // Set complete requirements
                    if (i < NumberOfCompleteReq)
                    {
                        foreach (var task in taskList)
                        {
                            var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(task));
                            var timer = insertAndGetTimer(new DTOTimer { TaskId = task.Id, TimeInSeconds = getR(1500, 5500) });
                        }
                    }
                    // Set incomplete requirements
                    else
                    {
                        for (int j = 0; j < getR(NFrom, Ntasks); j++)
                        {
                            var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(taskList[j]));
                            var timer = insertAndGetTimer(new DTOTimer { TaskId = taskList[j].Id, TimeInSeconds = getR(1500, 3000) });
                        }
                    }

                }

            }
            // Arrange
            var statistic = new DTOStatisticView
            {
                ItemStatisticId = projects.FirstOrDefault().Id,
                hp = Container.createHp()
            };

            // Act
            Container.createIProjectRepository().getProjectStatistic(Container.createIDbContext(), statistic);

            // Assert
            //bool velocitiesAreOk = false;
            //bool totalTaskShouldBeEqualOrHigherThanCompleteTask = false;
            //foreach (var item in result)
            //{
            //    velocitiesAreOk = item.VelocityInTaskPerSeconds > 0;

            //    totalTaskShouldBeEqualOrHigherThanCompleteTask = item.TotalTasks == item.CompleteTasks || item.TotalTasks > item.CompleteTasks;
            //}
            //Assert.IsTrue(result != null && result.Count > 0, "It does not return a DTOStatistic collection");
            //Assert.IsTrue(velocitiesAreOk && totalTaskShouldBeEqualOrHigherThanCompleteTask, "Velocities are not ok.");

        }
        // Private methods ====================================================
        private IQueryable<DTOProjectDataTable> getProjectDataTableIQueryable()
        {
            // Get the records as a list of DTOProjectDataTable
            var projectDataTable = Container.createIDbContext().Projects
                                    .Select(x => new DTOProjectDataTable
                                    {
                                        Name = x.Name,
                                        CreationDate = x.CreationDate,
                                        Description = x.Description,
                                        ProjectId = x.Id,
                                        NumberOfTasks = 0,
                                        Status = 0
                                    }).AsQueryable();

            return projectDataTable;
        }

        private Expression<Func<DTOProjectDataTable, bool>> getProjectDataTableFiltering(DataTableParams dataTableParams)
        {
            var DateHp = Container.createIDateHp();
            var date = Container.createIDataTableService().getDateTimeField(dataTableParams.sSearch, DateHp.OnlyDateFormat, "date", ':');
            Expression<Func<DTOProjectDataTable, bool>> filtering;
            if (!DateHp.isWholeMinDate(date))
            {
                filtering = (x => DbFunctions.TruncateTime(x.CreationDate) == date.Date);
            }
            else
            {
                filtering = (x => x.Name.Contains(dataTableParams.sSearch));
            }

            return filtering;
        }
    }
}
