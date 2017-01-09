using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.Helpers.DateHelper
{
    public interface IDateHp
    {
        DateTime WholeMinDate { get; }
        string WholeDateFormat { get; }
        string OnlyDateFormat { get; }
        string OnlyTimeFormat { get; }

        bool isWholeMinDate(DateTime date);
        bool isValidDate(string dateAsString, string format, out DateTime date);
        DateTime getDate(string dateAsString, string format);
        string getDateAsString(DateTime date, string format);

    }
}
