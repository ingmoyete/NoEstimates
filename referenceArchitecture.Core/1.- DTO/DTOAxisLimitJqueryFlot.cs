using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOAxisLimitJqueryFlot
    {
        public bool ThereIsX { get; set; }
        public double FromX { get; set; }
        public double ToX { get; set; }

        public bool ThereIsY { get; set; }
        public double FromY { get; set; }
        public double ToY { get; set; }
    }
}
