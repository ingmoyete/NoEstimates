using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoEstimates.Test.Core.Base;
using System.Linq;
using NoEstimates.Core.DTO;
using referenceArchitecture.Test.Core.Factory;

namespace NoEstimates.Test._3.__ServiceLayer
{
    [TestClass]
    public class RequirementServiceTest : BaseTest
    {
        [TestMethod]
        public void getAcumulativeStatisticForReq_text()
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
            var result = Container.createIStatisticService().getRequirementStatistic(statistic);

            // Assert

        }
    }
}
