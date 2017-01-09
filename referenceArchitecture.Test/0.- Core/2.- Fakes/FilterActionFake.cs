using Moq;
using referenceArchitecture.Test.Core.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace referenceArchitecture.Test.Core.Fakes
{
    public class FilterActionFake
    {
        /// <summary>
        /// Get a fake action executing context.
        /// </summary>
        /// <param name="controller">Controller to fake.</param>
        /// <param name="isAjaxRequest">If true, this method returns an ajax request.</param>
        /// <returns>A fake action executing context.</returns>
        public static ActionExecutingContext FakeActionExecutingContext(ControllerBase controller, bool isAjaxRequest = false, string controllerName = null, string actionName = null)
        {
            var request = mockHttpRequestBase(isAjaxRequest);

            var httpContext = mockHttpContextBase(request);

            var actionDescriptor = mockActionDescriptor(controllerName, actionName);

            var executingContext = new Mock<ActionExecutingContext>();
            executingContext.SetupGet(c => c.Controller).Returns(controller);
            executingContext.SetupGet(c => c.HttpContext).Returns(httpContext.Object);
            executingContext.SetupGet(c => c.ActionDescriptor).Returns(actionDescriptor.Object);

            return executingContext.Object;
        }

        /// <summary>
        /// Get a fake action executed context.
        /// </summary>
        /// <param name="controller">Controller to fake.</param>
        /// <param name="isAjaxRequest">If true, this method returns an ajax request.</param>
        /// <returns>A fake action executed context.</returns>
        public static ActionExecutedContext FakeActionExecutedContext(ControllerBase controller, bool isAjaxRequest = false, string controllerName = null, string actionName = null)
        {
            var request = mockHttpRequestBase(isAjaxRequest);

            var httpContext = mockHttpContextBase(request);

            var actionDescriptor = mockActionDescriptor(controllerName, actionName);
            
            var executingContext = new Mock<ActionExecutedContext>();
            executingContext.SetupGet(c => c.Controller).Returns(controller);
            executingContext.SetupGet(c => c.HttpContext).Returns(httpContext.Object);
            executingContext.SetupGet(c => c.ActionDescriptor).Returns(actionDescriptor.Object);

            return executingContext.Object;
        }

        /// <summary>
        /// Get a fake httpRequestBase.
        /// </summary>
        /// <param name="isAjaxRequest">If true, this method returns an ajax request.</param>
        /// <returns>a fake httpRequestBase.</returns>
        public static Mock<HttpRequestBase> mockHttpRequestBase(bool isAjaxRequest)
        {
            var request = new Mock<HttpRequestBase>();
            if (isAjaxRequest)
            {
                var headers = new WebHeaderCollection();
                headers.Add("X-Requested-With", "XMLHttpRequest");
                request.SetupGet(r => r.Headers).Returns(headers);
            }

            return request;
        }

        /// <summary>
        /// Get a fake httpContextBase.
        /// </summary>
        /// <param name="request">Request that the httpcontextBase is built from.</param>
        /// <returns>a fake httpContextBase.</returns>
        public static Mock<HttpContextBase> mockHttpContextBase(Mock<HttpRequestBase> request)
        {
            var httpContext = new Mock<HttpContextBase>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);

            return httpContext;
        }

        public static Mock<ActionDescriptor> mockActionDescriptor(string controllerName, string actionName)
        {
            var controllerDescriptor = new Mock<ControllerDescriptor>();
            var actionDescriptor = new Mock<ActionDescriptor>();
            actionDescriptor.SetupGet(c => c.ControllerDescriptor).Returns(controllerDescriptor.Object);
            actionDescriptor.SetupGet(c => c.ControllerDescriptor.ControllerName).Returns(controllerName);
            actionDescriptor.SetupGet(c => c.ActionName).Returns(actionName);

            return actionDescriptor;
        }

        public static ViewResult getViewResult(string viewName, ViewDataDictionary viewData)
        {
            return new ViewResult
            {
                ViewName = viewName,
                ViewData = viewData
            };
        }
        public static PartialViewResult getPartialViewResult(string viewName, ViewDataDictionary viewData)
        {
            return new PartialViewResult
            {
                ViewName = viewName,
                ViewData = viewData
            };
        }

    }
}
