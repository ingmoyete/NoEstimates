using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace referenceArchitecture.ui.Core.Filters
{
    public class CheckModelState : ActionFilterAttribute
    {
        public string GoToIfInvalidOnStart { get; set; }
        public string GoToIfInvalidOnEnd { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if model if valid before at the beginning of the action method
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                // If ajax request
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // Return bad request and the modelState as json
                    //todo ARCHITECTURE make the modelstate simpler otherwise it throws error
                    var errorsDictionary = filterContext.Controller.ViewData.ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    var json = new JavaScriptSerializer().Serialize(errorsDictionary);

                    // send 400 status code (Bad Request)
                    filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, json);
                }
                // If normal request
                else
                {
                    // Go to the view set in properties
                    filterContext.Result = new ViewResult
                    {
                        ViewName = GoToIfInvalidOnStart,
                        ViewData = filterContext.Controller.ViewData
                    };
                }
            }

            else
            {
                // Seguir con el proceso de ejecución
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Check if model if valid before at the beginning of the action method
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                // If ajax request
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // Return bad request and the modelState as json
                    //todo ARCHITECTURE make the modelstate simpler otherwise it throws error
                    var errorsDictionary = filterContext.Controller.ViewData.ModelState.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    var json = new JavaScriptSerializer().Serialize(errorsDictionary);

                    // send 400 status code (Bad Request)
                    filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.BadRequest, json);
                }
                // If normal request
                else
                {
                    // Go to the view set in properties
                    filterContext.Result = new ViewResult
                    {
                        ViewName = GoToIfInvalidOnEnd,
                        ViewData = filterContext.Controller.ViewData
                    };
                }
            }
            else
            {
                // Seguir con el proceso de ejecución
                base.OnActionExecuted(filterContext);
            }
        }
    }
}