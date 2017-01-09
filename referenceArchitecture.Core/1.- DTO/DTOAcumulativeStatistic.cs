using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOAcumulativeStatistic : DTOBase
    {
        public string AcumulativeSerieColor { get { return hp.getStringFromAppConfig("acumulative"); } }
        public string AcumulativePointsColor { get { return hp.getStringFromAppConfig("pointColor"); } }
        public DTOAxisLimitJqueryFlot AcumulativeAxisLimits { get; set; }
        public List<DTOMarkingsJqueryFlot> AcumulativeMarkings { get; set; }

        public double AveragePercentageSeconds { get; set; }
        public double AveragePercentageHours { get { return roundAvg(AveragePercentageSeconds * HoursConversion); } }
        public double AveragePercentageDays { get { return roundAvg(AveragePercentageSeconds * DaysConversion); } }
        public double AveragePercentageWeeks { get { return roundAvg(AveragePercentageSeconds * WeeksConversion); } }
        public double AveragePercentageMonths { get { return roundAvg(AveragePercentageSeconds * MonthsConversion); } }

        public List<double[]> AcumulativeSerieSeconds { get; set; }
        public List<double[]> AcumulativeSerieHours { get { return getXYData(AcumulativeSerieSeconds, HoursConversion); } }
        public List<double[]> AcumulativeSerieDays { get { return getXYData(AcumulativeSerieSeconds, DaysConversion); } }
        public List<double[]> AcumulativeSerieWeeks { get { return getXYData(AcumulativeSerieSeconds, WeeksConversion); } }
        public List<double[]> AcumulativeSerieMonths { get { return getXYData(AcumulativeSerieSeconds, MonthsConversion); } }
    }
}
