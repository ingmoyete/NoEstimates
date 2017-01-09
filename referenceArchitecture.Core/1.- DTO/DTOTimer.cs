using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOTimer : DTOBase
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int TimeInSeconds { get; set; }

        public string TimerValue
        {
            get
            {
                TimeSpan t = TimeSpan.FromSeconds(TimeInSeconds);
                string timeInRightFormat = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    t.Hours,
                    t.Minutes,
                    t.Seconds);

                return timeInRightFormat; 
            }
        }
    }
}
