using MathNet.Numerics;
using NoEstimates.Core.DTO;
using referenceArchitecture.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Helpers
{
    public class hp : Ihp
    {
        /// <summary>
        /// Get the regression parameter (slope, interception and r).
        /// </summary>
        /// <param name="xData">X data.</param>
        /// <param name="yData">Y data.</param>
        /// <returns>A DTOLineFitParameters object containing the regression parameters.</returns>
        public DTOLineFitParameters getRegressionParameters(double[] xData, double[] yData)
        {
            if (xData.Count() < 2 || yData.Count() < 2) return new DTOLineFitParameters();
            // Fit the data to a line
            Tuple<double, double> p = Fit.Line(xData, yData);

            // Get parameters
            var slope = p.Item2;
            var interception = p.Item1;
            var ret = new DTOLineFitParameters
            {
                Slope = slope,
                Interception = interception,
                R = GoodnessOfFit.RSquared(xData.Select(x => slope + interception * x), yData)
            };

            return ret;
        }

        /// <summary>
        /// Get the key for the resources dictionary.
        /// </summary>
        /// <returns>A string containing the key of the resources dictionary.</returns>
        public string getLocalResourceKey()
        {
            return "ResourcesKey";
        }

        /// <summary>
        /// Get the key for the global resources dictionary.
        /// </summary>
        /// <returns>A string containing the key of the global resources dictionary.</returns>
        public string getGlobalResourceKey()
        {
            return "GlobalResourcesKey";
        }

        /// <summary>
        /// Get value as integer from App.config. Throws an exception if the value
        /// in App.config is not valid (i.e is not a number).
        /// </summary>
        /// <param name="key">Key of the value in app.config to retrive.</param>
        /// <param name="exception">Exception to be thrown if the value is not valid.</param>
        /// <returns>An integer if value in App.config is valid. Otherwise throw exception </returns>
        public int getIntegerFromAppConfig(string key, Exception exception)
        {
            // Get minutes from app config
            int integerNumber;

            // Throw exception if value is not valid
            if (!int.TryParse(ConfigurationManager.AppSettings[key], out integerNumber)) throw exception;

            // Return integer if valid
            return integerNumber;
        }

        /// <summary>
        /// Get value as integer from App.config. Throws an InvalidValueInAppConfig exception if the value
        /// in App.config is not valid (i.e is not a number).
        /// </summary>
        /// <param name="key">Key of the value in app.config to retrive.</param>
        /// <returns>An integer if value in App.config is valid. Otherwise throw exception </returns>
        public int getIntegerFromAppConfig(string key)
        {
            // Get minutes from app config
            int integerNumber;

            // Throw exception if value is not valid
            if (!int.TryParse(ConfigurationManager.AppSettings[key], out integerNumber)) throw new InvalidValueInAppConfig(key);

            // Return integer if valid
            return integerNumber;
        }

        /// <summary>
        /// Get value as string from App.config. Throws an exception if the value
        /// in App.config is null, empty or white space.
        /// </summary>
        /// <param name="key">Key of the value in app.config to retrive.</param>
        /// <param name="exception">Exception to be thrown if the value is not valid.</param>
        /// <returns>A string if value is valid. Otherwise throws exception.</returns>
        public string getStringFromAppConfig(string key, Exception exception)
        {
            // Get value from configuration
            string valueFromAppConfig = ConfigurationManager.AppSettings[key];

            // Throw exception if value is not valid
            if (string.IsNullOrWhiteSpace(valueFromAppConfig)) throw exception;

            return valueFromAppConfig;
        }

        /// <summary>
        /// Get value as string from App.config. Throws an  InvalidValueInAppConfig exception if the value
        /// in App.config is null, empty or white space.
        /// </summary>
        /// <param name="key">Key of the value in app.config to retrive.</param>
        /// <returns>A string if value is valid. Otherwise throws exception.</returns>
        public string getStringFromAppConfig(string key)
        {
            // Get value from configuration
            string valueFromAppConfig = ConfigurationManager.AppSettings[key];

            // Throw exception if value is not valid
            if (string.IsNullOrWhiteSpace(valueFromAppConfig)) throw new InvalidValueInAppConfig(key);

            return valueFromAppConfig;
        }

        /// <summary>
        /// Get a route from a comma separated value in app.config.
        /// </summary>
        /// <param name="keyAppConfig">The key of the comma separated value in app.config.</param>
        /// <returns>A path with the base directory.</returns>
        public string getPathFromSeparatedCommaValue(string keyAppConfig)
        {
            // Get comma separated values from app.config
            string[] commaSeparatedValues = getStringFromAppConfig(keyAppConfig).Split(',');

            string routeBuilder = "";

            // Get first value if there is only one element in the array
            if (commaSeparatedValues.Length == 1)
            {
                routeBuilder = commaSeparatedValues[0];
            }
            // Get aroute from all the elements in the array
            else
            {
                foreach (var item in commaSeparatedValues)
                {
                    routeBuilder = Path.Combine(routeBuilder, item);
                }

            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, routeBuilder);
        }

    }
}
