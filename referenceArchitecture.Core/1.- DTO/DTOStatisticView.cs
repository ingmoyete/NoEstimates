using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace NoEstimates.Core.DTO
{
    public class DTOStatisticView : DTOBase
    {
        /// <summary>
        /// Constructor to set the defaults.
        /// </summary>
        public DTOStatisticView()
        {
            AcumulativeGraph = new DTOAcumulativeStatistic();
            VelocityGraph = new DTOVelocityStatistic();
            CompletionGraph = new DTOCompletionStatistic();
        }

        public int ItemStatisticId { get; set; }
        public string ItemStatisticName { get; set; }
        public string ReturnUrl { get; set; }
        private int timeAxis = 1 ;
        public int TimeAxis
        {
            get { return timeAxis; }
            set { timeAxis  = value; }
        }


        // Acumulative
        public DTOAcumulativeStatistic AcumulativeGraph { get; set; }
        public string AcumulativeGraphJson { get { return getJsonString<DTOAcumulativeStatistic>(AcumulativeGraph); } }

        // Velocity
        public DTOVelocityStatistic VelocityGraph { get; set; }
        public string VelocityGraphJson { get { return getJsonString<DTOVelocityStatistic>(VelocityGraph); } }

        // Completion
        public DTOCompletionStatistic CompletionGraph { get; set; }
        public string CompletionGraphJson { get { return getJsonString<DTOCompletionStatistic>(CompletionGraph); } }
    }
}
