using InteractivePreGeneratedViews;
using NoEstimates.repository.Core.ChangeDb;
using NoEstimates.repository.Edmx.BaseContextAndPartialClasses;
using NoEstimates.ui.App_Start;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.ui.App_Start;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace referenceArchitecture.ui
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //todo ARCHITECTURE: put this in global.asax and change db connection.
            //PrecompileViewConfiguration.RegisterPrecompileViews();

        }
        protected void Application_Error()
        {
            //// Get excepcion and clear error
            //Exception exception = Server.GetLastError();
            //Server.ClearError();

            //// Get controller and action from appconfig and go to that route
            //var controllerAndAction = ConfigurationManager.AppSettings["controllerAndActionErrorRedirect"].Split(',');
            //Response.RedirectToRoute(new
            //{
            //    controller = controllerAndAction[0],
            //    action = controllerAndAction[1],
            //    exceptionMessage = exception.Message 
            //});
        }
    }
}
