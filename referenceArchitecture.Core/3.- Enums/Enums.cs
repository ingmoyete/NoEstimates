using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.Enums
{
    public enum HighlightColor
    {
        Blue = 1,
        Yellow = 2,
        Red = 3,
        Green = 4
    }

    public enum StatisticTimeAxis
    {
        Hours = 1,
        Days = 2,
        Weeks = 3,
        Months = 4
    }

    public enum StatisticGraph
    {
        Acumulative = 1,
        Velocity = 2,
        Completion = 3
    }
}
