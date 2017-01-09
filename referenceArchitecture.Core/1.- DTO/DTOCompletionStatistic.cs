using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOCompletionStatistic : DTOBase
    {
        public double AverageRemainingTimeSeconds { get; set; }
        public double AverageRemainingTimeHours { get { return roundAvg(AverageRemainingTimeSeconds / HoursConversion); } }
        public double AverageRemainingTimeDays { get { return roundAvg(AverageRemainingTimeSeconds / DaysConversion); } }
        public double AverageRemainingTimeWeeks { get { return roundAvg(AverageRemainingTimeSeconds / WeeksConversion); } }
        public double AverageRemainingTimeMonths { get { return roundAvg(AverageRemainingTimeSeconds / MonthsConversion); } }

        public DTOAxisLimitJqueryFlot CompletionAxisLimits { get; set; }
        public List<DTOMarkingsJqueryFlot> CompletionMarkings { get; set; }

        // Completion serie =================
        public string CompletionSerieColor { get { return hp.getStringFromAppConfig("completion"); } }
        public string CompletionPointsColor { get { return hp.getStringFromAppConfig("pointColor"); } }

        public List<double[]> CompletionSerieSeconds { get; set; }
        public List<double[]> CompletionSerieHours { get { return getXYData(CompletionSerieSeconds, HoursConversion); } }
        public List<double[]> CompletionSerieDays { get { return getXYData(CompletionSerieSeconds, DaysConversion); } }
        public List<double[]> CompletionSerieWeeks { get { return getXYData(CompletionSerieSeconds, WeeksConversion); } }
        public List<double[]> CompletionSerieMonths { get { return getXYData(CompletionSerieSeconds, MonthsConversion); } }

        // Estimation serie ==============
        public string EstimationSerieColor { get { return hp.getStringFromAppConfig("estimation"); } }

        public double AverageCompletionTimeSeconds { get; set; }
        public double AverageCompletionTimeHours { get { return roundAvg(AverageCompletionTimeSeconds / HoursConversion); } }
        public double AverageCompletionTimeDays { get { return roundAvg(AverageCompletionTimeSeconds / DaysConversion); } }
        public double AverageCompletionTimeWeeks { get { return roundAvg(AverageCompletionTimeSeconds / WeeksConversion); } }
        public double AverageCompletionTimeMonths { get { return roundAvg(AverageCompletionTimeSeconds / MonthsConversion); } }

        public double R { get; set; }
        public List<double[]> EstimationSerieSeconds { get; set; }
        public List<double[]> EstimationSerieHours { get { return getXYData(EstimationSerieSeconds, HoursConversion); } }
        public List<double[]> EstimationSerieDays { get { return getXYData(EstimationSerieSeconds, DaysConversion); } }
        public List<double[]> EstimationSerieWeeks { get { return getXYData(EstimationSerieSeconds, WeeksConversion); } }
        public List<double[]> EstimationSerieMonths { get { return getXYData(EstimationSerieSeconds, MonthsConversion); } }
    }
}
