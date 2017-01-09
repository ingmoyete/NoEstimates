using Microsoft.Practices.Unity;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Logger;
using referenceArchitecture.Core.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace referenceArchitecture.ui.Core.Filters
{
    public class CustomHandleError : FilterAttribute, IExceptionFilter
    {
        // Key of the error to be shown for each controller
        private string errorKey = "ApplicationException";

        /// <summary>
        /// Global Resources object to get strings from csv.
        /// </summary>
        private IResource globalResources = DependencyResolver.Current.GetService<IResource>();
        public IResource GlobalResources
        {
            get
            {
                globalResources.getResources(globalResources.GlobalResourceFileName);
                return globalResources;
            }
        }

        /// <summary>
        /// Logger object to log exceptions in disk.
        /// </summary>
        private ILogger logger = DependencyResolver.Current.GetService<ILogger>();
        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        /// <summary>
        /// Mensaje de error generico que muestra el ActionResult cuando hay una excepcion no controlada
        /// </summary>
        public string MensajeErrorActionResult { get; set; }

        /// <summary>
        /// Metodo que se ejecuta cuando una excepcion es lanzada en un controlador
        /// </summary>
        /// <param name="filterContext">Contexto del filtro</param>
        public void OnException(ExceptionContext filterContext)
        {
            //todo ARCHITECTURE : add this set error message and regisrter the custom handle error global filters
            // Set Error message if it is null
            if (MensajeErrorActionResult == null)
            {
                MensajeErrorActionResult = string.Format(GlobalResources["exceptionError"], filterContext.RouteData.Values["controller"], filterContext.RouteData.Values["action"]);
            }

            // Get logger from property
            var logger = Logger;

            // Set view result, message, exception and view name
            var viewResult = new ViewResult();
            var message = filterContext.Exception.Message;
            var exception = filterContext.Exception;
            viewResult.ViewName = "Error";

            // DbEntityValidationException-Catch =====================
            if (filterContext.Exception is DbEntityValidationException)
            {
                DbEntityValidationException ex = (DbEntityValidationException)filterContext.Exception;

                #region Excepcion DbEntityValidationException
                List<string> validationErrors = new List<string>();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    foreach (var error in failure.ValidationErrors)
                    {
                        if (!validationErrors.Contains(error.PropertyName + ":" + error.ErrorMessage))
                        {
                            validationErrors.Add(error.PropertyName + ":" + error.ErrorMessage);
                        }
                    }
                }

                if (validationErrors.Count == 0)
                {
                    validationErrors.Add(ex.Message);
                }

                string errorEntity = string.Join(",", validationErrors.ToArray());

                #endregion

                logger.writeLog(errorEntity);
                viewResult.ViewData.ModelState.AddModelError(errorKey, MensajeErrorActionResult);
            }
            // Exception-Catch =======================================
            else if (filterContext.Exception is Exception)
            {
                logger.writeLog(exception);
                viewResult.ViewData.ModelState.AddModelError(errorKey, MensajeErrorActionResult);
            }

            // No se relanza excepcion
            filterContext.ExceptionHandled = true;

            // Se de vuelve la partialView con el ModelCustomErro
            filterContext.Result = viewResult;
        }
    }
}