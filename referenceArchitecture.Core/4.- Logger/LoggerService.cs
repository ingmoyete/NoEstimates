using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace referenceArchitecture.Core.Logger
{
    /// <summary>
    /// Logger service
    /// </summary>
    public class LoggerService : ILogger
    {
        // Parameters used in the constructor
        string filename;

        // helper
        private Ihp hp;

        /// <summary>
        /// Construct the logger service with the path of the log.
        /// </summary>
        public LoggerService(Ihp _hp)
        {
            // Get helper
            this.hp = _hp;

            // Folder path: base directory + folder path
            string directories = hp.getPathFromSeparatedCommaValue("logFolderName");

            // throw exception if the directory folder does not exist
            if (!Directory.Exists(directories)) throw new LoggerDirectoryDoesNotExist(directories);

            // File path: Folder path + filename and extension Name
            filename = Path.Combine
                (
                    directories,
                    GetFilenameYYYMMDD_Hours("_LOG", ".log")
                );

        }

        /// <summary>
        /// Write an exception in a log file.
        /// </summary>
        /// <param name="ex">Exception to be writen in the log.</param>
        public void writeLog(Exception ex)
        {

            // just in case: we protect code with try.
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                XElement xmlEntry = new XElement("logEntry",
                    new XElement("Date", System.DateTime.Now.ToString()),
                    new XElement("Exception",
                        new XElement("Source", ex.Source),
                        new XElement("Type", ex.GetType().ToString()),
                        new XElement("Message", ex.Message),
                        new XElement("Stack", ex.StackTrace)
                        )//end exception
                );
                //has inner exception?
                if (ex.InnerException != null)
                {
                    xmlEntry.Element("Exception").Add(
                        new XElement("InnerException",
                            new XElement("Source", ex.InnerException.Source),
                            new XElement("Message", ex.InnerException.Message),
                            new XElement("Stack", ex.InnerException.StackTrace))
                        );
                }
                sw.WriteLine(xmlEntry);
                sw.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get the name of the log in the format of "yyyy_MM_dd__H_mm_ss".
        /// </summary>
        /// <param name="suffix">The name after the date.</param>
        /// <param name="extension">The extension of the log file.</param>
        /// <returns>A string with the name and extension of the log file.</returns>
        private string GetFilenameYYYMMDD_Hours(string suffix, string extension)
        {
            return System.DateTime.Now.ToString("yyyy_MM_dd__H_mm_ss") + suffix + extension;
        }

        /// <summary>
        /// Write a message in a log file.
        /// </summary>
        /// <param name="message">Message to be writen in the log file.</param>
        public void writeLog(string message)
        {
            // just in case: we protect code with try.
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                XElement xmlEntry = new XElement("logEntry",
                    new XElement("Date", System.DateTime.Now.ToString()),
                    new XElement("Message", message));
                sw.WriteLine(xmlEntry);
                sw.Close();
            }
            catch (Exception)
            {
            }
        }

    }
}
