using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.Test.Core.Factory;
using NoEstimates.Core.DTO;
using System.Data.SqlTypes;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using referenceArchitecture.repository.Edmx.Interfaces;
using NoEstimates.repository._0.__Edmx;
using System.Diagnostics;
using NoEstimates.Test.Core.Base;

namespace NoEstimates.Test.ControllerLayer
{
    [TestClass]
    public class TaskControllerTest : BaseTest
    {
        private Random r = new Random();

        [TestMethod]
        public void InsertTask_insert_task_and_return_json()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskToInsert = getRandomTask(requirement);

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.InsertTask(taskToInsert) as JsonResult;

            // Assert
            var idOfInsertedRecord = int.Parse(result.Data.ToString());
            var expectedTask = Container.createITaskRepository().getTaskById(Container.createIDbContext(), new DTOTask { Id = idOfInsertedRecord, RequirementId = requirement.Id });
            Assert.IsTrue(result != null, "It is not a json result");
            Assert.IsTrue
            (
                expectedTask.RequirementId == taskToInsert.RequirementId
                && expectedTask.Description == taskToInsert.Description
                && datesAreEqual(expectedTask.CreationDate, taskToInsert.CreationDate),
                "The record was no inserted."
            );
        }

        [TestMethod]
        public void DeleteTask_deletes_only_a_task_record_and_return_json_true()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskToInsert = insertTasksAndgetList(requirement, 1).FirstOrDefault();

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.DeleteTask(taskToInsert) as JsonResult;

            // Assert
            var dataAsBool = result.Data as bool?;
            var deletedTask = Container.createITaskRepository().getTaskPanelByTaskId(Container.createIDbContext(), new DTOTask { Id = taskToInsert.Id});
            Assert.IsTrue(result != null && dataAsBool.Value, "It is not a json result or does not return true");
            Assert.IsTrue(deletedTask == null, "It was not deleted.");
        }

        [TestMethod]
        public void DeleteTask_deletes_a_task_highlight_timer_complete_record_and_return_json_true()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskToInsert = insertTasksAndgetList(requirement, 1).FirstOrDefault();
            var complete = insertAndGetComplete(getRandomComplete(taskToInsert));
            var highlight = insertAndGetHighlight(getRandomHighlight(taskToInsert));
            var timer = insertAndGetTimer(getRandomTimer(taskToInsert));

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.DeleteTask(taskToInsert) as JsonResult;

            // Assert
            var dataAsBool = result.Data as bool?;
            var deletedTask = Container.createITaskRepository().getTaskPanelByTaskId(Container.createIDbContext(), new DTOTask { Id = taskToInsert.Id });
            Assert.IsTrue(result != null && dataAsBool.Value, "It is not a json result or does not return true");
            Assert.IsTrue(deletedTask == null, "It was not deleted.");
        }

        [TestMethod]
        public void Index_return_taskList_totalNumber_completedNumber_and_view()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            int numberOfTasks = 4;
            var taskList = insertTasksAndgetList(requirement, numberOfTasks);

            var task = taskList.OrderBy(x => x.Id).FirstOrDefault();
            var highlight = insertAndGetHighlight(getRandomHighlight(task));
            var complete = insertAndGetComplete(getRandomCompleteWithIsCompleteTrue(task));
            var timer = insertAndGetTimer(getRandomTimer(task));

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.Index(task) as ViewResultBase;

            // Assert
            var expectedTaskView = result.Model as DTOTaskView;
            var expectedTask = expectedTaskView.ListTaskPanels.FirstOrDefault().Task;
            var expectedHighlight = expectedTaskView.ListTaskPanels.FirstOrDefault().Highlight;
            var expectedTimer = expectedTaskView.ListTaskPanels.FirstOrDefault().Timer;
            var expectedComplete = expectedTaskView.ListTaskPanels.FirstOrDefault().Complete;
            Assert.IsTrue(result != null && result.ViewName == "Index", "It is not returning a index view");
            Assert.IsTrue(expectedTaskView.TotalTasks == numberOfTasks && expectedTaskView.CompletedTasks == 1, "The number of completed tasks or total tasks are wrong");
            Assert.IsTrue
            (
                expectedTask != null && expectedHighlight != null && expectedTimer != null && expectedComplete != null,
                "Task, Highlight, Timer, and complete were inserted"
            );
            Assert.IsTrue
            (
                areSameObjets(task, expectedTask)
                && areSameObjets(highlight, expectedHighlight)
                && areSameObjets(complete, expectedComplete)
                && areSameObjets(timer, expectedTimer),
                "The records does not match"
            );
        }

        [TestMethod]
        public void Index_return_taskList_outerJoin()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 1);


            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.Index(new DTOTask {RequirementId = requirement.Id }) as ViewResultBase;
        }

        [TestMethod]
        public void InsertHighlight_insert_highlight_and_return_idAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 3);
            var taskId = taskList.OrderBy(x => x.Id).FirstOrDefault().Id;

            var randonColor = r.Next(0, 4);
            DTOHighlightColor highlight = new DTOHighlightColor
            {
                Color = randonColor,
                TaskId = taskId,
            };

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteHighlight(highlight) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedHighlight = Container.createIDbContext().Highlights.Where(x => x.Id == resultAsInt).ToList().FirstOrDefault(); 
            Assert.IsTrue(result != null && resultAsInt > 0 , "It is not returning a json result");
            Assert.IsTrue(expectedHighlight != null, "The record was not inserted.");
            Assert.IsTrue
            (
                highlight.Color == expectedHighlight.Color
                && highlight.TaskId == expectedHighlight.TaskId,
                "The record inserted does not correspond with the one created."
            );

        }

        [TestMethod]
        public void InsertComplete_insert_Complete_and_return_idAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 3);
            var taskId = taskList.OrderBy(x => x.Id).FirstOrDefault().Id;

            DTOComplete complete = getRandomComplete(taskList.FirstOrDefault());

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteComplete(complete) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedComplete = Container.createIDbContext().Completes.Where(x => x.TaskId == taskId).ToList().FirstOrDefault();
            Assert.IsTrue(result != null, "It is not returning a json result");
            Assert.IsTrue(expectedComplete != null, "The record was not inserted.");
            Assert.IsTrue
            (
                complete.IsComplete == expectedComplete.IsComplete
                && complete.FinalizationDate.CompareTo(expectedComplete.FinalizationDate) == 0
                && complete.TaskId == expectedComplete.TaskId,
                "The record inserted does not correspond with the one created."
            );

        }

        [TestMethod]
        public void InsertTimer_insert_Timer_and_return_idAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 3);
            var taskId = taskList.OrderBy(x => x.Id).FirstOrDefault().Id;

            var randonSeconds = r.Next(90000);
            DTOTimer timer = new DTOTimer
            {
                TimeInSeconds = randonSeconds,
                TaskId = taskId
            };

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteTimer(timer) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedTimer = Container.createIDbContext().Timers.Where(x => x.TaskId == taskId).ToList().FirstOrDefault();
            Assert.IsTrue(result != null, "It is not returning a json result");
            Assert.IsTrue(expectedTimer != null, "The record was not inserted.");
            Assert.IsTrue
            (
                timer.TimeInSeconds == expectedTimer.TimeInSeconds
                && timer.TaskId == expectedTimer.TaskId,
                "The record inserted does not correspond with the one created."
            );

        }

        [TestMethod]
        public void UpdateTimer_update_Timer_and_return_trueAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 1);
            var task = taskList.OrderBy(x => x.Id).FirstOrDefault();

            var oldTimer = insertAndGetTimer(getRandomTimer(task));

            var randonSecons = r.Next(90000);
            var newTimer = new DTOTimer
            {
                TaskId = oldTimer.TaskId,
                Id = oldTimer.Id,
                TimeInSeconds = randonSecons
            };

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteTimer(newTimer) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedNewTimer = Container.createIDbContext().Timers.Where(x => x.TaskId == newTimer.TaskId).ToList().FirstOrDefault();
            Assert.IsTrue(result != null && resultAsInt.Value > 0, "It is not returning a json result");
            Assert.IsTrue(expectedNewTimer != null, "The record does not exist.");
            Assert.IsTrue
            (
                newTimer.TimeInSeconds == expectedNewTimer.TimeInSeconds,
                "The record was not updated."
            );

        }

        [TestMethod]
        public void UpdateHighlight_update_highlight_and_return_trueAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 1);
            var task = taskList.OrderBy(x => x.Id).FirstOrDefault();

            var oldHighlight = insertAndGetHighlight(getRandomHighlight(task));

            var randonColor = r.Next(0, 4);
            var newHighLight = new DTOHighlightColor
            {
                TaskId = oldHighlight.TaskId,
                Id = oldHighlight.Id,
                Color = randonColor
            };

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteHighlight(newHighLight) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedNewHighlight = Container.createIDbContext().Highlights.Where(x => x.TaskId == newHighLight.TaskId).ToList().FirstOrDefault();
            Assert.IsTrue(result != null && resultAsInt.Value > 0, "It is not returning a json result");
            Assert.IsTrue(expectedNewHighlight != null, "The record does not exist.");
            Assert.IsTrue
            (
                (int)newHighLight.Color == expectedNewHighlight.Color,
                "The record was not updated."
            );

        }

        [TestMethod]
        public void UpdateComplete_update_complete_and_return_trueAsJson()
        {
            var project = insertAndgetProject(getRandomProject());
            var requirement = insertAndgetRequirement(getRandomRequirement(project));
            var taskList = insertTasksAndgetList(requirement, 3);
            var oldComplete = insertAndGetComplete(getRandomComplete(taskList.FirstOrDefault()));

            var newComplete = getRandomComplete(taskList.FirstOrDefault());
            newComplete.Id = oldComplete.Id;

            // Arrange
            var taskController = Container.createTasksController();

            // Act
            var result = taskController.WriteComplete(newComplete) as JsonResult;

            // Assert
            var resultAsInt = result.Data as int?;
            var expectedNewComplete = Container.createIDbContext().Completes.Where(x => x.TaskId == newComplete.TaskId).FirstOrDefault();
            Assert.IsTrue(result != null && resultAsInt.Value > 0, "It is not returning a json result");
            Assert.IsTrue(expectedNewComplete != null, "The record does not exist.");
            Assert.IsTrue
            (
                newComplete.IsComplete == expectedNewComplete.IsComplete
                && datesAreEqual(newComplete.FinalizationDate, expectedNewComplete.FinalizationDate),
                "The record was not updated."
            );

        }

    }
}
