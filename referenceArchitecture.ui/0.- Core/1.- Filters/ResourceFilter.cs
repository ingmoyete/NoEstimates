using Microsoft.Practices.Unity;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace referenceArchitecture.ui.Core.Filters
{
    public class ResourceFilter : ActionFilterAttribute
    {
        #region Properties
        /// <summary>
        /// Resources object to get strings from csv.
        /// </summary>
        private IResource localResources = DependencyResolver.Current.GetService<IResource>();

        public IResource LocalResources
        {
            get { return localResources; }
            set { localResources = value; }
        }

        /// <summary>
        /// Helper to get variables from app.config.
        /// </summary>
        private Ihp _hp = DependencyResolver.Current.GetService<Ihp>();
        public Ihp hp
        {
            get { return _hp; }
            set { _hp = value; }
        }
        #endregion

        /// <summary>
        /// Build a resources from csv file and save them in viewData when the an action is executed.
        /// </summary>
        /// <param name="filterContext">Filter context of the action result.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Get the view result from filter context result
            var viewResult = filterContext.Result;
            
            // Only if viewResult or a partial view(not json, redirect or post)
            if (viewResult is ViewResultBase)
            {
                // Set local resources for this page (controllerName + ActionName)
                setLocalResources(filterContext);

                // Save both resources in ViewData (See webViewPageBase)
                filterContext.Controller.ViewData.Add(hp.getLocalResourceKey(), LocalResources);
            }
        }

        private void setLocalResources(ActionExecutedContext filterContext)
        {
            // Get csv file name as Controller name + actionResult
            string page =
            (
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
                + filterContext.ActionDescriptor.ActionName
            ).ToUpper();

            // Get resources from IOC and get csv.
            LocalResources.getResources(page);
        }

    }
}