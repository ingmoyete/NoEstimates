using referenceArchitecture.service.Core.Base.BaseController.MVCWrapper;
using referenceArchitecture.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.ui.Core.Filters;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.Core.Cache;
using DevTrends.MvcDonutCaching;

namespace referenceArchitecture.ui.Controllers
{
    [CustomHandleError(MensajeErrorActionResult = "Error occured in Home")]
    public class HomeController : BaseController
    {

        // GET: Get index page
        [HttpGet]
        [DonutOutputCache(CacheProfile = "htmlGlobalCache900")]
        public ActionResult Index()
        {
            return View();
        }
        
        // CHILDONLY: Get all users
        [ChildActionOnly]
        public ActionResult ListUsers()
        {
            return PartialView("PartialListUsers", null);
        }

        // GET: Get user form
        [HttpGet]
        [DonutOutputCache(CacheProfile = "htmlGlobalCache900")]
        public ActionResult Users()
        {
            return View("Users", new DTOExample { FirstField = "Nombre", SecondField = 15, ThirdField = DateTime.Now });
        }
    }
}