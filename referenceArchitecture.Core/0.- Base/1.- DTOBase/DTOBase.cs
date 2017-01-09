using referenceArchitecture.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using referenceArchitecture.Core.Helpers;
using System.Reflection;
using referenceArchitecture.Core.Exceptions;
using System.Web.Script.Serialization;

namespace referenceArchitecture.Core.Base.DTOBase
{
    public abstract class DTOBase : IDTOBase
    {
        /// <summary>
        /// Get a collection of DTO properties by names.
        /// </summary>
        /// <param name="validationContext">The validation context of the attribute.</param>
        /// <param name="propertyNames">The property name whose values will be retrived.</param>
        /// <returns>A collection with property values (same order as the one provided with the propertyNames).</returns>
        public List<object> getDTOPropertiesByNames(ValidationContext validationContext, params string[] propertyNames)
        {
            // Get DTO
            var dto = validationContext.ObjectInstance;
            var dtoType = dto.GetType();

            // get DTOBase
            var dtoBase = validationContext.ObjectInstance as DTOBase;

            // Get all properties through a loop
            List<object> properties = new List<object>();
            foreach (var propertyName in propertyNames)
            {
                // Get value of a single property
                PropertyInfo propertyInfo = dtoType.GetProperty(propertyName);
                object value = propertyInfo.GetValue(dtoBase);


                // insert value in collection
                properties.Add(value);
            }

            return properties;
        }

        /// <summary>
        /// Get the X and Y data (for jquery flot) from a collection after a conversion is applied.
        /// </summary>
        /// <param name="collectionToConvert">Collection which is converted to an X-Y jquery flot data.</param>
        /// <param name="conversion">Converstion to applied.</param>
        /// <param name="isForVelocity">True if the graph is the velocity graph. Otherwise false.</param>
        /// <returns>A collection of double arrays which X and Y data for jquery flot.</returns>
        protected List<double[]> getXYData(List<double[]> collectionToConvert, double conversion, bool isForVelocity = false)
        {
            // Declare
            var xyData = new List<double[]>();

            // Loop through all the items in the collection to build the serie X,Y if if the collection is null or empty
            if (!isForVelocity && collectionToConvert != null && collectionToConvert.Count > 0)
            {
                foreach (var item in collectionToConvert)
                    xyData.Add(new double[] { roundValues(item[0] / conversion), roundValues(item[1]) });
            }
            else if (isForVelocity && collectionToConvert != null && collectionToConvert.Count > 0)
            {
                foreach (var item in collectionToConvert)
                    xyData.Add(new double[] { roundValues(item[0] / conversion), roundValues(item[1] * conversion) });
            }
            return xyData;
        }

        /// <summary>
        /// Get a json string from a collection.
        /// </summary>
        /// <param name="objectToConvert">Collection to convert into json string.</param>
        /// <returns>A json string.</returns>
        protected string getJsonString<T>(T objectToConvert) where T : class
        {
            // Declare
            var javascriptSerializer = new JavaScriptSerializer();

            // Convert list of double array into json string and return it
            return javascriptSerializer.Serialize(objectToConvert);
        }

        /// <summary>
        /// Round a number with one decimal.
        /// </summary>
        /// <param name="num">Number to round.</param>
        /// <returns>A double.</returns>
        protected double roundAvg(double num)
        {
            return Math.Round(num, 2);
        }

        /// <summary>
        /// Round a number with few decimals.
        /// </summary>
        /// <param name="num">Number to round.</param>
        /// <returns>A double.</returns>
        protected double roundValues(double num)
        {
            return Math.Round(num, 6);
        }

        /// <summary>
        /// Conversion of hours for graph.
        /// </summary>
        public double HoursConversion { get { return hp.getIntegerFromAppConfig("secondsPerHour"); } }

        /// <summary>
        /// Conversion of days for graphs.
        /// </summary>
        public double DaysConversion
        {
            get
            {
                return hp.getIntegerFromAppConfig("secondsPerHour")
                    * hp.getIntegerFromAppConfig("hoursPerDay");
            }
        }

        /// <summary>
        /// Conversion of weeks for graph.
        /// </summary>
        public double WeeksConversion
        {
            get
            {
                return hp.getIntegerFromAppConfig("secondsPerHour")
                * hp.getIntegerFromAppConfig("hoursPerDay")
                * hp.getIntegerFromAppConfig("daysPerWeek");
            }
        }

        /// <summary>
        /// Conversion of months for graph.
        /// </summary>
        public double MonthsConversion
        {
            get
            {
                return hp.getIntegerFromAppConfig("secondsPerHour")
                * hp.getIntegerFromAppConfig("hoursPerDay")
                * hp.getIntegerFromAppConfig("daysPerWeek")
                * hp.getIntegerFromAppConfig("weeksPermonth");
            }
        }

        #region Properties
        ///<summary>
        /// Helper to get variables from app.config.
        /// </summary>
        private Ihp _hp = DependencyResolver.Current.GetService<Ihp>();
        public Ihp hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Resources object to get strings from csv.
        /// </summary>
        private IResource globalResources = DependencyResolver.Current.GetService<IResource>();
        public IResource GlobalResources
        {
            get
            {
                globalResources.getResources(globalResources.GlobalResourceFileName);
                return globalResources;
            }
            set { globalResources = value; }
        }


        /// <summary>
        /// Collection of validation results.
        /// </summary>
        private List<ValidationResult> validationResultCollection = new List<ValidationResult>();
        public List<ValidationResult> ValidationResultCollection
        {
            get { return validationResultCollection; }
            set { validationResultCollection = value; }
        }
        #endregion

    }
}
