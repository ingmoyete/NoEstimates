using NoEstimates.Core.DTO;
using NoEstimates.repository.ProjectRepository;
using NoEstimates.repository.RequirementsRepository;
using NoEstimates.service.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.StatisticService
{
    public class StatisticService : BaseService, IStatisticService
    {
        /// <summary>
        /// Requirement repository.
        /// </summary>
        private IRequirementsRepository requirementRepository;

        /// <summary>
        /// Project repository.
        /// </summary>
        private IProjectRepository projectRepository;

        /// <summary>
        /// Constructor used to inject dependencies.
        /// </summary>
        /// <param name="_requirementRepository">Requirement repository dependency.</param>
        /// <param name="_projectRepository">Project repository dependency.</param>
        public StatisticService(IRequirementsRepository _requirementRepository, IProjectRepository _projectRepository)
        {
            this.requirementRepository = _requirementRepository;
            this.projectRepository = _projectRepository;
        }

        /// <summary>
        /// Get all the statistic for one requirement.
        /// </summary>
        /// <param name="statistic">Dto that contains the id of the requirement.</param>
        /// <returns>A DTOStatisticView containing all the statistics for on requirement.</returns>
        public DTOStatisticView getRequirementStatistic(DTOStatisticView statistic)
        {
            using (DbContext)
            {
                // Get statistics from db
                var statsFromDb = requirementRepository.getRequirementStatistics(DbContext, statistic);

                // Do not continue if there is not tasks.
                var requirementName = requirementRepository.getRequirementById(DbContext, new DTORequirements { Id = statistic.ItemStatisticId }).Name;
                if (statsFromDb.Count == 0) return new DTOStatisticView { ItemStatisticName = requirementName, ReturnUrl = statistic.ReturnUrl };

                // Get and return DTOStatisticView
                var ret = getDTOStatisticView(statsFromDb, statistic);
                return ret;
            }
        }

        public DTOStatisticView getProjectStatistic(DTOStatisticView statistic)
        {
            using (DbContext)
            {
                // Get statistics from db
                var statsFromDb = projectRepository.getProjectStatistic(DbContext, statistic);

                // Do not continue if there is not tasks.
                var projectName = projectRepository.getProjectById(DbContext, new DTOProject { Id = statistic.ItemStatisticId }).Name;
                if (statsFromDb.Count == 0) return new DTOStatisticView { ItemStatisticName = projectName, ReturnUrl = statistic.ReturnUrl };

                // Get and return DTOStatisticView
                var ret = getDTOStatisticView(statsFromDb, statistic);
                return ret;
            }
        }

        #region Private Methods
        private DTOStatisticView getDTOStatisticView(List<DTOStatistic> statsFromDb, DTOStatisticView statistic)
        {
            // Get acumulative graph components
            var acumulative = getAcumulativeGraphComponents(statsFromDb);

            // Get velocity graph components
            var velocity = getVelocityGraphComponents(statsFromDb, statistic);

            // Get completion graph componenets
            var completion = getCompletionGraphComponents(statsFromDb);

            // Set the properties of DTOStatisticView
            var retStatistic = statsFromDb
            .GroupBy(x => x)
            .Select(x => new DTOStatisticView
            {
                ItemStatisticName = x.Key.Name,
                ItemStatisticId = x.Key.Id,
                TimeAxis = statistic.TimeAxis,
                ReturnUrl = statistic.ReturnUrl,

                    // Acumulative
                    AcumulativeGraph = acumulative,

                    // Velocity
                    VelocityGraph = velocity,

                    // Completion
                    CompletionGraph = completion,

                hp = Hp

            }).FirstOrDefault();

            // Return the statistic just built or a new one if it was null
            return (retStatistic != null) ? retStatistic : new DTOStatisticView();
        }
        /// <summary>
        /// Get velocity graph components such as marking, axis, and series.
        /// </summary>
        /// <param name="statsFromDb">DTOStatistic collection from db.</param>
        /// <param name="statistic">DTOStatisticView that contains information about the id of the item.</param>
        /// <returns>A DTOVelocityStatistic object that corresponds to the velocity graph.</returns>
        private DTOVelocityStatistic getVelocityGraphComponents(List<DTOStatistic> statsFromDb, DTOStatisticView statistic)
        {
            // Get serie
            var velocitySerie = (from velocityRecord in statsFromDb.Select(z => new { Id = z.Id, Velocity = z.VelocityInItemPerSeconds })
                                 join timeRecord in statsFromDb.Select(z => new { Id = z.Id, z.AcumulativeTimeInSeconds })
                                     on velocityRecord.Id equals timeRecord.Id
                                 select new double[] { timeRecord.AcumulativeTimeInSeconds, velocityRecord.Velocity }).ToList();

            // Get average
            var averageVelocity = statsFromDb.Average(y => y.VelocityInItemPerSeconds);

            // Map dto an return it.
            return new DTOVelocityStatistic
            {
                AverageVelocitySeconds = averageVelocity,
                VelocitySerieSeconds = velocitySerie
            };
        }

        /// <summary>
        /// Get acumulative graph components such as marking, axis, and series.
        /// </summary>
        /// <param name="statsFromDb">DTOStatistic collection from db.</param>
        /// <returns>A DTOAcumulativeStatistic object that corresponds to the acumulative graph.</returns>
        private DTOAcumulativeStatistic getAcumulativeGraphComponents(List<DTOStatistic> statsFromDb)
        {
            // Get serie
            var acumulativeSerie = (from percentageRecord in statsFromDb.Select(z => new { Id = z.Id, Percentage = z.PercentageComplete })
                                    join timeRecord in statsFromDb.Select(z => new { Id = z.Id, z.AcumulativeTimeInSeconds })
                                        on percentageRecord.Id equals timeRecord.Id
                                    select new double[] { timeRecord.AcumulativeTimeInSeconds, percentageRecord.Percentage }).ToList();

            // Get average
            var acumulativeAverage = statsFromDb.Average(x => x.VelocityInPercentagePerSeconds);

            // Get markings
            var acumulativeMarkings = new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot
                    {
                        IsY = true,
                        YLine = Hp.getIntegerFromAppConfig("acumulativeMarkingHorizontalLine"),
                        Color = Hp.getStringFromAppConfig("acumulativeMarkings")
                    }
                };

            // Get axis
            var acumulativeAxis = new DTOAxisLimitJqueryFlot
            {
                ThereIsY = true,
                FromY = Hp.getIntegerFromAppConfig("acumulativeAxisFromY"),
                ToY = Hp.getIntegerFromAppConfig("acumulativeAxisToY")
            };

            // Map DTO and return it
            return new DTOAcumulativeStatistic
            {
                AveragePercentageSeconds = acumulativeAverage,
                AcumulativeSerieSeconds = acumulativeSerie,
                AcumulativeMarkings = acumulativeMarkings,
                AcumulativeAxisLimits = acumulativeAxis
            };
        }

        /// <summary>
        /// Get completion graph components such as marking, axis, and series.
        /// </summary>
        /// <param name="statsFromDb">DTOStatistic collection from db.</param>
        /// <returns>A DTOCompletionStatistic object that corresponds to the completion graph.</returns>
        private DTOCompletionStatistic getCompletionGraphComponents(List<DTOStatistic> statsFromDb)
        {
            var totalTasks = statsFromDb.Count > 0 ? statsFromDb.FirstOrDefault().TotalItems : 0;

            // Get completion serie
            var completionSerieSeconds = (from completionRecord in statsFromDb.Select(z => new { Id = z.Id, CompletedTasks = z.DescendingItemComplete })
                                          join timeRecord in statsFromDb.Select(z => new { Id = z.Id, z.AcumulativeTimeInSeconds })
                                              on completionRecord.Id equals timeRecord.Id
                                          select new double[] { timeRecord.AcumulativeTimeInSeconds, completionRecord.CompletedTasks }).ToList();

            // Get completion axis
            var completionAxis = new DTOAxisLimitJqueryFlot
            {
                ThereIsY = true,
                FromY = 0,
                ToY = totalTasks
            };

            // Get completion markings
            var completionMarkings = new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot
                    {
                        IsY = true,
                        YLine = totalTasks * 0.5,
                        Color = Hp.getStringFromAppConfig("completionMarkings")}
                };

            // Get estimation serie by using regression
            var xData = completionSerieSeconds.Select(x => x[0]).ToArray();
            var yData = completionSerieSeconds.Select(x => x[1]).ToArray();
            var regressionParameters = Hp.getRegressionParameters(xData, yData);

            var averageFinishTimeSeconds = -regressionParameters.Interception / regressionParameters.Slope;
            var averageRemainingTimeSeconds = averageFinishTimeSeconds - xData.Max();
            List<double[]> estimationLineSeconds = new List<double[]>
            {
                new double[] { xData.Min(), xData.Min() * regressionParameters.Slope + regressionParameters.Interception },
                new double[] { averageFinishTimeSeconds, 0 }
            };

            // Map DTO and return it
            return new DTOCompletionStatistic
            {
                CompletionSerieSeconds = completionSerieSeconds,
                EstimationSerieSeconds = estimationLineSeconds,
                AverageCompletionTimeSeconds = averageFinishTimeSeconds,
                AverageRemainingTimeSeconds = averageRemainingTimeSeconds,
                CompletionAxisLimits = completionAxis,
                CompletionMarkings = completionMarkings,
                R = Math.Round(regressionParameters.R, 2)
            };
        }
        #endregion

    }
}
