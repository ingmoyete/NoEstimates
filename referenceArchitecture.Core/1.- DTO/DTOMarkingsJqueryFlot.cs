using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOMarkingsJqueryFlot
    {
        public double XLine { get; set; }
        public bool IsX { get; set; }

        public double YLine { get; set; }
        public bool IsY { get; set; }

        public string Color { get; set; }
    }
}
