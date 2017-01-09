using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.ui.Core.Filters;
using referenceArchitecture.ui.Controllers;
using System.Web.Mvc;
using System.Reflection;
using Moq;
using System.Web;
using System.Web.Routing;
using referenceArchitecture.Test.Core.Factory;
using referenceArchitecture.Test.Core.Fakes;
using System.Net;

namespace referenceArchitecture.Test.Filters
{
    [TestClass]
    public class CheckModelStateTest
    {
        [TestMethod]
        public void CheckModelState_onStart_is_Ok()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";

            // Arrange
            checkModelState.GoToIfInvalidOnStart = viewName;
            var actionExecuting = FilterActionFake.FakeActionExecutingContext(Container.createHomeController());
            
            // Act
            checkModelState.OnActionExecuting(actionExecuting);


            // Assert
            var result = actionExecuting.Result as ViewResultBase;
            Assert.IsTrue(result == null, "It does not return the right view.");
        }

        [TestMethod]
        public void CheckModelState_onEnd_is_Ok()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";

            // Arrange
            checkModelState.GoToIfInvalidOnEnd = viewName;
            var FakeActionExecutedContext = FilterActionFake.FakeActionExecutedContext(Container.createHomeController());

            // Act
            checkModelState.OnActionExecuted(FakeActionExecutedContext);


            // Assert
            var result = FakeActionExecutedContext.Result as ViewResultBase;
            Assert.IsTrue(result == null, "It does not return the right view.");
        }

        [TestMethod]
        public void CheckModelState_return_invalidView_onStart()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";
            string testErrorMessage = "testError";

            // Arrange
            checkModelState.GoToIfInvalidOnStart = viewName;
            var actionExecuting = FilterActionFake.FakeActionExecutingContext(Container.createHomeController());

            // Act
            actionExecuting.Controller.ViewData.ModelState.AddModelError(testErrorMessage, testErrorMessage);
            checkModelState.OnActionExecuting(actionExecuting);

            // Assert
            var result = actionExecuting.Result as ViewResultBase;
            Assert.AreEqual("invalidViewName", result.ViewName, "It does not return the right view.");
            Assert.IsTrue(result.ViewData.ModelState[testErrorMessage].Errors.Count > 0, "The error is not build.");
        }

        [TestMethod]
        public void CheckModelState_return_invalidView_onEnd()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";
            string testErrorMessage = "testError";

            // Arrange
            checkModelState.GoToIfInvalidOnEnd = viewName;
            var actionExecuted = FilterActionFake.FakeActionExecutedContext(Container.createHomeController());

            // Act
            actionExecuted.Controller.ViewData.ModelState.AddModelError(testErrorMessage, testErrorMessage);
            checkModelState.OnActionExecuted(actionExecuted);

            // Assert
            var result = actionExecuted.Result as ViewResultBase;
            Assert.AreEqual("invalidViewName", result.ViewName, "It does not return the right view.");
            Assert.IsTrue(result.ViewData.ModelState[testErrorMessage].Errors.Count > 0, "The error is not build.");
        }

        [TestMethod]
        public void CheckModelState_return_json_onStart()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";
            string testErrorMessage = "testError";

            // Arrange
            checkModelState.GoToIfInvalidOnEnd = viewName;
            var actionExecuted = FilterActionFake.FakeActionExecutedContext(Container.createHomeController(), true);

            // Act
            actionExecuted.Controller.ViewData.ModelState.AddModelError(testErrorMessage, testErrorMessage);
            checkModelState.OnActionExecuted(actionExecuted);

            // Assert
            var result = actionExecuted.Result as HttpStatusCodeResult;
            Assert.IsTrue(result != null && result.StatusCode == (int)HttpStatusCode.BadRequest, "It does not return a bad request.");
            Assert.IsTrue(actionExecuted.Controller.ViewData.ModelState[testErrorMessage].Errors.Count > 0, "The error is not build.");
        }

        [TestMethod]
        public void CheckModelState_return_json_onEnd()
        {
            CheckModelState checkModelState = new CheckModelState();
            string viewName = "invalidViewName";
            string testErrorMessage = "testError";

            // Arrange
            checkModelState.GoToIfInvalidOnStart = viewName;
            var FakeActionExecutingContext = FilterActionFake.FakeActionExecutingContext(Container.createHomeController(), true);

            // Act
            FakeActionExecutingContext.Controller.ViewData.ModelState.AddModelError(testErrorMessage, testErrorMessage);
            checkModelState.OnActionExecuting(FakeActionExecutingContext);

            // Assert
            var result = FakeActionExecutingContext.Result as HttpStatusCodeResult;
            Assert.IsTrue(result != null && result.StatusCode == (int)HttpStatusCode.BadRequest, "It does not return a bad request.");
            Assert.IsTrue(FakeActionExecutingContext.Controller.ViewData.ModelState[testErrorMessage].Errors.Count > 0, "The error is not build.");
        }
    }
}
