using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOVelocityStatistic : DTOBase
    {
        public string VelocitySerieColor { get { return hp.getStringFromAppConfig("velocity"); } }
        public string VelocityPointsColor { get { return hp.getStringFromAppConfig("pointColor"); } }
        public DTOAxisLimitJqueryFlot VelocityAxisLimits { get; set; }
        public List<DTOMarkingsJqueryFlot> VelocityMarkingsHours
        {
            get
            {
                return new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot {  YLine = AverageVelocityHours, IsY = true, Color = hp.getStringFromAppConfig("velocityMarkings")}
                };
            }
        }
        public List<DTOMarkingsJqueryFlot> VelocityMarkingsDays
        {
            get
            {
                return new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot {  YLine = AverageVelocityDays, IsY = true, Color = hp.getStringFromAppConfig("velocityMarkings")}
                };
            }
        }
        public List<DTOMarkingsJqueryFlot> VelocityMarkingsWeeks
        {
            get
            {
                return new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot {  YLine = AverageVelocityWeeks, IsY = true, Color = hp.getStringFromAppConfig("velocityMarkings")}
                };
            }
        }
        public List<DTOMarkingsJqueryFlot> VelocityMarkingsMonths
        {
            get
            {
                return new List<DTOMarkingsJqueryFlot>
                {
                    new DTOMarkingsJqueryFlot {  YLine = AverageVelocityMonths, IsY = true, Color = hp.getStringFromAppConfig("velocityMarkings")}
                };
            }
        }

        public double AverageVelocitySeconds { get; set; }
        public double AverageVelocityHours { get { return roundAvg(AverageVelocitySeconds * HoursConversion); } }
        public double AverageVelocityDays { get { return roundAvg(AverageVelocitySeconds * DaysConversion); } }
        public double AverageVelocityWeeks { get { return roundAvg(AverageVelocitySeconds * WeeksConversion); } }
        public double AverageVelocityMonths { get { return roundAvg(AverageVelocitySeconds * MonthsConversion); } }

        public List<double[]> VelocitySerieSeconds { get; set; }
        public List<double[]> VelocitySerieHours { get { return getXYData(VelocitySerieSeconds, HoursConversion, true); } }
        public List<double[]> VelocitySerieDays { get { return getXYData(VelocitySerieSeconds, DaysConversion, true); } }
        public List<double[]> VelocitySerieWeeks { get { return getXYData(VelocitySerieSeconds, WeeksConversion, true); } }
        public List<double[]> VelocitySerieMonths { get { return getXYData(VelocitySerieSeconds, MonthsConversion, true); } }

    }
}
