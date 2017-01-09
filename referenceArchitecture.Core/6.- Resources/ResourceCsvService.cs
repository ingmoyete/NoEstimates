using CsvHelper;
using referenceArchitecture.Core.Cache;
using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Resources
{
    public class ResourceCsvService : IResource
    {
        // Dictionary
        Dictionary<string, string> resources;

        // File extension
        private string fileExtension = ".csv";
        
        // Cache, hp and logger service parameter
        private ICache cacheService;
        private ILogger loggerService;
        private Ihp hp;

        /// <summary>
        /// Construtor used to inject a cache service.
        /// </summary>
        /// <param name="_cacheService">Cache service to be injected.</param>
        public ResourceCsvService(ICache _cacheService, ILogger _loggerService, Ihp _hp)
        {
            this.cacheService   = _cacheService;
            this.loggerService  = _loggerService;
            this.hp             = _hp;
        }


        /// <summary>
        /// Get an specific resource inside a page.
        /// Page is the name of the csv file containing the resources.
        /// </summary>
        /// <param name="key">Key corresponding to a resource.</param>
        /// <returns>Resource corresponding to the "key".</returns>
        public string this[string key]
        {
            get
            {
                string resource;
                try
                {
                    bool keyExist = resources.Keys.Where(x => x == key).FirstOrDefault() != null;
                    
                    // Throw exception if resource for that key does not exist
                    if (!keyExist) throw new KeyOfResourceDoesNotExist(key);
                    else resource = resources[key];
                }
                catch (KeyOfResourceDoesNotExist ex)
                {
                    // If it does not exist, a "defaultstring-Key" is returned
                    resource = hp.getStringFromAppConfig("ThereIsNoResourceKey", new InvalidDeafultMessageForNotFoundResourceKey()) + "-" + key;
                    
                    // Log the exception
                    loggerService.writeLog(ex);
                }

                // Return resource if exist. Otherwise a default string from app.config is returned.
                return resource;

            }
        }

        /// <summary>
        /// Resources of this current instance.
        /// </summary>
        public Dictionary<string, string> Resources { get { return resources; } }

        /// <summary>
        /// File name of the global resource.
        /// </summary>
        public string GlobalResourceFileName { get { return "GlobalResources"; } }

        /// <summary>
        /// Get resources from cache or from a csv file if there is nothing in cache.
        /// </summary>
        /// <param name="page">Normally corresponds to an html page.</param>
        /// <returns>A dictionary containing all the resources in the "page".</returns>
        /// 
        public Dictionary<string, string> getResources(string page)
        {
            string upperPage = page.ToUpper();

            // try to pull resources from cache
            var localResources = (Dictionary<string, string>)cacheService.get(upperPage);

            // If there is nothing in cache
            if (localResources == null)
            {
                // Get resources from csv
                localResources = getResourcesFromCsv(upperPage);

                // Set this resources in cache
                cacheService.setWithSlideExpiration
                (
                    upperPage, 
                    localResources, 
                    getCsvFilePath(page), 
                    hp.getIntegerFromAppConfig("resourcesCache")
                );
            }

            // Fill dictionary parameters with resources
            this.resources = localResources;

            // Return resources
            return localResources;
        }

        /// <summary>
        /// Get a resource page as a dictionary from a csv file.
        /// </summary>
        /// <param name="page">Normally corresponds to an html page.</param>
        /// <returns>A dictionary containing all the resources in the "page".</returns>
        private Dictionary<string, string> getResourcesFromCsv(string page)
        {

            // Generate file path: base directory + resources folder + file name
            var filePath = getCsvFilePath(page);
            
            // Throw exception if resource file does not exist
            if (!File.Exists(filePath)) throw new ResourceNotExist(page);

            // Get CSV file as dictionary
            return getDictionaryFromCsv(filePath);
        }

        /// <summary>
        /// Built dictionary from a csv file.
        /// </summary>
        /// <param name="filePath">Path of the csv file.</param>
        /// <returns>A dictionary with the info of the csv file.</returns>
        private Dictionary<string, string> getDictionaryFromCsv(string filePath)
        {
            // Create dictionary to hold the resource
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // Create streamReader to read the csv
            using (var stream = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                // Read csv
                var csv = new CsvReader(stream);
                while (csv.Read())
                {
                    bool elementAlreadyExist = dictionary.Where(x => x.Key == csv.GetField(0) && x.Value == csv.GetField(1)).Count() > 0;

                    if (!elementAlreadyExist)
                    {
                        dictionary.Add(csv.GetField(0), csv.GetField(1));
                    }
                }
                
            }

            // Return dictionary
            return dictionary;
        }
        
        /// <summary>
        /// Get the file path of the a csv file.
        /// </summary>
        /// <param name="page">Name of the csv file: it will be the name of the controller + action result.</param>
        /// <returns>A string with the file path of the csv file.</returns>
        private string getCsvFilePath(string page)
        {
            var path = hp.getPathFromSeparatedCommaValue("resourcesFolderName");
            var filePath = Path.Combine
            (
                path,
                page + fileExtension
            );

            return filePath;
        }
    }
}
