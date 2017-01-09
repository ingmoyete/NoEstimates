using NoEstimates.Core.DataTableService;
using NoEstimates.Core.DTO;
using NoEstimates.Core.Helpers.DateHelper;
using NoEstimates.service.ProjectService;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.service.Core.Base.BaseController.MVCWrapper;
using referenceArchitecture.ui.Core.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace NoEstimates.ui.Controllers
{
    public class ProjectsController : BaseController
    {
        /// <summary>
        /// Project service parameter.
        /// </summary>
        private IProjectService projectService;

        /// <summary>
        /// Constructor used to inject the project service.
        /// </summary>
        /// <param name="_projectService"></param>
        public ProjectsController(IDateHp _dateHp, IProjectService _projectService, IDataTableService _dataTableService, IDbContext _Context)
        {
            this.projectService = _projectService;
            projectService.register(this);
        }

        // AJAX-GET: DataTableAjaxHandler
        [HttpGet]
        public ActionResult DataTableAjaxHandler(DataTableParams dataTableParams)
        {
            return Json(projectService.getDataTableJson(dataTableParams), JsonRequestBehavior.AllowGet);
        }


        // GET: Projects
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        // POST-AJAX: Projects/CreateProject
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult CreateProject([Bind(Exclude = "Id")] DTOProject project)
        {
            int id = projectService.createProject(project);
            return Json(id);
        }

        // POST-AJAX: Projects/UpdateProject
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult UpdateProject(DTOProject project)
        {
            projectService.updateProject(project);
            return Json(true);
        }

        // POST-AJAX: Projects/DeleteProject
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult DeleteProject(DTOProject project)
        {
            projectService.deleteProject(project);
            return Json(true);
        }
    }
}