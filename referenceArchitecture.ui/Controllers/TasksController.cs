 using NoEstimates.Core.DTO;
using NoEstimates.service.TaskService;
using referenceArchitecture.service.Core.Base.BaseController.MVCWrapper;
using referenceArchitecture.ui.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoEstimates.ui.Controllers
{
    public class TasksController : BaseController
    {
        /// <summary>
        /// Task service.
        /// </summary>
        private ITaskService taskService;

        /// <summary>
        /// Constructor used to inject the task service.
        /// </summary>
        public TasksController(ITaskService _taskService)
        {
            this.taskService = _taskService;
            this.taskService.register(this);
        }

        // GET: Tasks
        [HttpGet]
        public ActionResult Index(DTOTask task)
        {
            return View("Index", taskService.getTaskView(task));
        }

        // POST: InsertTask
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult InsertTask([Bind(Exclude = "Id")] DTOTask task)
        {
            int id = taskService.insertTask(task);
            return Json(id);
        }

        // POST: UpdateTask
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult UpdateTask(DTOTask task)
        {
            taskService.updateTask(task);
            return Json(true);
        }

        // POST: DeleteTask
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult DeleteTask(DTOTask task)
        {
            taskService.deleteWholeTask(task);
            return Json(true);
        }

        // POST: WriteHighlight
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult WriteHighlight(DTOHighlightColor highlight)
        {
            int id = taskService.writeHighlight(highlight);
            return Json(id);
        }

        // POST: WriteComplete
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult WriteComplete(DTOComplete complete)
        {
            int id = taskService.writeComplete(complete);
            return Json(id);
        }

        // POST: WriteTimer
        [HttpPost, CheckModelState, ValidateAntiForgeryToken]
        public ActionResult WriteTimer(DTOTimer timer)
        {
            int id = taskService.writeTimer(timer);
            return Json(id);
        }
    }
}