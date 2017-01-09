using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOStatistic : DTOBase
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int AcumulativeTimeInSeconds { get; set; }
        public double PercentageComplete { get; set; }
        public int DescendingItemComplete { get; set; }

        public double VelocityInItemPerSeconds { get; set; }
        public double VelocityInPercentagePerSeconds { get {return PercentageComplete / AcumulativeTimeInSeconds; } }

        public int TotalItems { get; set; }
        public int CompleteItems { get; set; }
    }
}
