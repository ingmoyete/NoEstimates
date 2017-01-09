using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.Test.Core.Factory;
using referenceArchitecture.ui.Controllers;
using referenceArchitecture.Test.Core.Fakes;
using referenceArchitecture.ui.Core.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using referenceArchitecture.Core.Resources;
using System.Linq;

namespace referenceArchitecture.Test.Filters
{
    [TestClass]
    public class ResourceFilterTest
    {
        [TestMethod]
        public void OnActionExecuted_get_LocalAndGlobalResources_for_IndexActionResult()
        {
            string viewResultName = "testView";
            string actionName = "Index";
            string controllerName = "Home";

            // Arrange : Create actionExecutedContext
            var hp = Container.createHp();
            var homeController = Container.createHomeController();
            var actionExecutedContext = FilterActionFake.FakeActionExecutedContext(homeController, false, controllerName, actionName);

            ResourceFilter resourceFilter = new ResourceFilter(); // Instantiate filter and set properties
            resourceFilter.LocalResources = Container.createIResource();
            resourceFilter.hp = hp;

            // Act: Set view result and execute the onActionExecuted
            actionExecutedContext.Result = FilterActionFake.getViewResult(viewResultName, actionExecutedContext.Controller.ViewData);
            resourceFilter.OnActionExecuted(actionExecutedContext);

            // Assert
            var expectedlocalResource = Container.createIResource();
            expectedlocalResource.getResources(string.Format("{0}{1}", controllerName, actionName));
            
            var actualLocalResource = actionExecutedContext.Controller.ViewData[hp.getLocalResourceKey()] as IResource;
            
            Assert.IsTrue(compareTwoResources(expectedlocalResource, actualLocalResource), "Local resource is not set in viewdata.");

        }

        private bool compareTwoResources(IResource resources1, IResource resources2)
        {
            bool ret = true;
            foreach (var resource2 in resources2.Resources)
            {
                var query = resources1.Resources.Where(x => x.Key == resource2.Key && x.Value == resource2.Value);
                if (query.Count() == 0)
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }


    }
}
