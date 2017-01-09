using NoEstimates.Core.DTO;
using NoEstimates.service.RequirementService;
using NoEstimates.service.StatisticService;
using referenceArchitecture.service.Core.Base.BaseController.MVCWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NoEstimates.ui.Controllers
{
    public class StatisticController : BaseController
    {
        /// <summary>
        /// Statistic service.
        /// </summary>
        IStatisticService statisticService;

        /// <summary>
        /// Constructor to inject dependencies.
        /// </summary>
        /// <param name="_statisticService">Statistic service to be injected.</param>
        public StatisticController(IStatisticService _statisticService)
        {
            this.statisticService = _statisticService;
            statisticService.register(this);
        }

        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        // GET: Requirement
        public ActionResult Requirement(DTOStatisticView statistic)
        {
            return View("Index", statisticService.getRequirementStatistic(statistic));
        }

        // GET: Project
        public ActionResult Project(DTOStatisticView statistic)
        {
            return View("Index", statisticService.getProjectStatistic(statistic));
        }
    }
}