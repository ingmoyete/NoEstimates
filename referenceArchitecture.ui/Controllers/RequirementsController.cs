using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.service.RequirementService;
using referenceArchitecture.service.Core.Base.BaseController.MVCWrapper;
using referenceArchitecture.ui.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoEstimates.ui.Controllers
{
    public class RequirementsController : BaseController
    {
        /// <summary>
        /// Requirement service.
        /// </summary>
        private IRequirementService requirementService;

        /// <summary>
        /// Constructor used to inject the requirement service.
        /// </summary>
        /// <param name="_requirementService">Requirement service dependency.</param>
        public RequirementsController(IRequirementService _requirementService)
        {
            this.requirementService = _requirementService;
            requirementService.register(this);
        }
        
        // GET: Requirements
        [HttpGet]
        public ActionResult Index(DTORequirements requirement)
        {
            return View("Index", requirementService.getAllRequirements(requirement));
        }

        // AJAX-GET: DataTableAjaxHandler
        [HttpGet]
        public ActionResult DataTableAjaxHandler(DTORequirements requirement, DataTableParams dataTableParams)
        {
            var data = requirementService.getJsonDataTable(requirement, dataTableParams);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // POST-AJAX: Requirements/CreateRequirement
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult CreateRequirement([Bind(Exclude = "Id")] DTORequirements requirement)
        {
            int id = requirementService.insertRequirement(requirement);
            return Json(id);
        }

        // POST-AJAX: Requirements/UpdateRequirement
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult UpdateRequirement(DTORequirements requirement)
        {
            requirementService.updateRequirement(requirement);
            return Json(true);
        }

        // POST-AJAX: Requirements/DeleteRequirement
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult DeleteRequirement(DTORequirements requirement)
        {
            requirementService.deleteRequirement(requirement);
            return Json(true);
        }
    }
}