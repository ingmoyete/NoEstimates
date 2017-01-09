using referenceArchitecture.Core.Exceptions;
using referenceArchitecture.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace referenceArchitecture.Core.Cache
{
    public class CacheService : ICache
    {
        // Helper
        private Ihp hp;

        /// <summary>
        /// Constructor used to inject helper.
        /// </summary>
        /// <param name="_hp"></param>
        public CacheService(Ihp _hp)
        {
            this.hp = _hp;
        }

        /// <summary>
        /// Get an object from cache corresponding to a key.
        /// </summary>
        /// <param name="key">Key that corresponds to an element in cache.</param>
        /// <returns>Object from cache</returns>
        public object get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// Remove an object from cache corresponding to a key.
        /// </summary>
        /// <param name="key">Key that corresponds to an element in cache.</param>
        public void remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// Set object in cache with a key. It uses absolute expiration.
        /// </summary>
        /// <param name="key">Key corresponding to an object.</param>
        /// <param name="data">Data to be set in cache.</param>
        /// <param name="filePathDependency">File path where a dependency is set.</param>
        /// <param name="minutesExpiration">Expiration of the saved object in minutes.</param>
        public void setWithAbsoluteExpiration(string key, object data, string filePathDependency = null, int? minutesExpiration = null)
        {
            // Remove cache and exits the method if minutesExpiration is 0
            if(minutesExpiration == 0 || AbsoluteExpiration == 0) { remove(key); return;}

            // Calculate expiration
            DateTime expiration =
                minutesExpiration != null
                ? DateTime.UtcNow.AddMinutes(minutesExpiration.Value)
                : DateTime.UtcNow.AddMinutes(AbsoluteExpiration);

            // Calculate Dependency
            var dependency =
                filePathDependency != null
                ? new System.Web.Caching.CacheDependency(filePathDependency)
                : null;

            // Set object in cache
            HttpRuntime.Cache.Insert
            (
                key, 
                data,
                dependency,
                expiration,
                System.Web.Caching.Cache.NoSlidingExpiration
            );
        }

        /// <summary>
        /// Set object in cache with a key. It uses slide expiration.
        /// </summary>
        /// <param name="key">Key corresponding to an object.</param>
        /// <param name="data">Data to be set in cache.</param>
        /// <param name="filePathDependency">File path where a dependency is set.</param>
        /// <param name="minutesExpiration">Expiration of the saved object in minutes.</param>
        public void setWithSlideExpiration(string key, object data, string filePathDependency = null, int? minutesExpiration = null)
        {
            // Remove cache and exits the method if minutesExpiration is 0
            if (minutesExpiration == 0 || SlidingExpiration == 0) { remove(key); return; }

            // Calculate expiration
            TimeSpan expiration =
                minutesExpiration != null
                ? TimeSpan.FromMinutes(minutesExpiration.Value)
                : TimeSpan.FromMinutes(SlidingExpiration);
            
            // Calculate Dependency
            var dependency =
                filePathDependency != null
                ? new System.Web.Caching.CacheDependency(filePathDependency)
                : null;

            // Set object in cache
            HttpRuntime.Cache.Insert
            (
                key, 
                data,
                dependency,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                expiration
            );
        }

        /// <summary>
        /// Sliding expiration in minutes from app.config.
        /// </summary>
        public int SlidingExpiration { get { return hp.getIntegerFromAppConfig("globalSlidingCache"); } }

        /// <summary>
        /// Absolute expiration in minutes from app.config.
        /// </summary>
        public int AbsoluteExpiration { get { return hp.getIntegerFromAppConfig("globalAbsoluteCache"); } }
    }
}
