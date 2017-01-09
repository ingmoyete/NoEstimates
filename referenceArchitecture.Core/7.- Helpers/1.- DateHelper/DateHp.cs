using referenceArchitecture.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.Helpers.DateHelper
{
    public class DateHp : IDateHp
    {
        /// <summary>
        /// Helper to get strings from web config.
        /// </summary>
        Ihp hp;

        /// <summary>
        /// Constructor used to inject hp.S
        /// </summary>
        /// <param name="_hp"></param>
        public DateHp(Ihp _hp)
        {
            this.hp = _hp;
        }

        /// <summary>
        /// Check if a whole date, only date, or only time is valid according to the format set in web.config.
        /// </summary>
        /// <param name="dateAsString">Date as string to check.</param>
        /// <param name="date">Date variable to fill with the parsed value if it can be parsed.</param>
        /// <returns>True if it is a valid date. Otherwise false.</returns>
        public bool isValidDate(string dateAsString, string format, out DateTime date)
        {
            bool ret = false;
            date = WholeMinDate;

            // Check for the formats
            if (format == WholeDateFormat)
            {
                ret = DateTime.TryParseExact(dateAsString, hp.getStringFromAppConfig("wholeDateFormat"), CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }
            else if (format == OnlyDateFormat)
            {
                ret = DateTime.TryParseExact(dateAsString, hp.getStringFromAppConfig("onlyDateFormat"), CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }
            else if (format == OnlyTimeFormat)
            {
                ret = DateTime.TryParseExact(dateAsString, hp.getStringFromAppConfig("onlyTimeFormat"), CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            }

            // True if it can be parsed with the format
            return ret;
        }

        /// <summary>
        /// Get a minimun date with a format. Both the date and the format are set in web.config.
        /// </summary>
        public DateTime WholeMinDate { get { return DateTime.ParseExact(hp.getStringFromAppConfig("dateMinValue"), WholeDateFormat, CultureInfo.InvariantCulture); } }

        /// <summary>
        /// Format of the whole date.
        /// </summary>
        public string WholeDateFormat { get { return hp.getStringFromAppConfig("wholeDateFormat"); } }

        /// <summary>
        /// Format of the only date.
        /// </summary>
        public string OnlyDateFormat { get { return hp.getStringFromAppConfig("onlyDateFormat"); } }

        /// <summary>
        /// Format of the only time.
        /// </summary>
        public string OnlyTimeFormat { get { return hp.getStringFromAppConfig("onlyTimeFormat"); } }

        /// <summary>
        /// Get a date (Date and Time) from a string applying the format set in web.config.
        /// </summary>
        /// <param name="dateAsString">Date as string to be parsed.</param>
        /// <returns>A date as string if it can be parsed. Otherwise it returned a MinDate..</returns>
        public DateTime getDate(string dateAsString, string format)
        {
            // Get date if it can be parsed
            DateTime date = isValidDate(dateAsString, format, out date)
                            ? date
                            : WholeMinDate;

            // Return date if it can be parsed, Otherwise it returned the MinDate
            return date;
        }

        /// <summary>
        /// Get a date (Date and Time) as string applying the format set in web.config.
        /// </summary>
        /// <param name="date">Date to convert into string.</param>
        /// <returns>A date as string if it can be parsed. Otherwise it returned a MinDate.</returns>
        public string getDateAsString(DateTime date, string format)
        {
            // Convert date into string
            string dateAsString = date.ToString(format);

            // Check if the date can be parsed
            dateAsString = isValidDate(dateAsString, format, out date)
                            ? dateAsString
                            : WholeMinDate.ToString(format);

            // Return date as string if it can be parsed, Otherwise it returned the MinDate
            return dateAsString;
        }

        /// <summary>
        /// Check if a date is whether or not a min date.
        /// </summary>
        /// <param name="date">Date to be checked.</param>
        /// <returns>True if the date is a mindate. Otherwise false.</returns>
        public bool isWholeMinDate(DateTime date)
        {
            bool ret = (date == WholeMinDate);
            return ret;
        }
    }
}
