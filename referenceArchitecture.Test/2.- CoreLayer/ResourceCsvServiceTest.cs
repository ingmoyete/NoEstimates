using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.Core.Cache;
using referenceArchitecture.Test.Core.Factory;
using System.Collections.Generic;
using System.Linq;


namespace referenceArchitecture.Test.Core
{
    [TestClass]
    public class ResourceCsvServiceTest
    {
        [TestMethod]
        public void getResources_return_dictionary()
        {
            // Arrange
            var resourceCsv = Container.createIResource();

            // Act
            var dictionary = resourceCsv.getResources("homeusers");

            // Assert
            Assert.IsTrue(dictionary.Count > 0);
        }

        [TestMethod]
        public void getResources_return_dictionary_from_cache()
        {
            string keyResource = "homeusers";

            // Arrange
            var cache = Container.createICache();
            var logger = Container.createILogger();
            var hp = Container.createHp();

            var resourceCsv = new ResourceCsvService(cache, logger, hp);
            
            // Act
            // First call (should be from csv) 
            var dictionary = resourceCsv.getResources(keyResource);
            var dictionaryFromCache = (Dictionary< string, string>)cache.get(keyResource.ToUpper());
            
            // Assert
            Assert.IsTrue(dictionary.FirstOrDefault().Value == dictionaryFromCache.FirstOrDefault().Value, "Dictionary is not saved in cache.");
        }

        [TestMethod]
        public void CacheService_notUsed_AndRemoved_when_expiration_is_0()
        {
            // Arrange
            var cacheService = Container.createICache();

            // Act
            cacheService.setWithSlideExpiration("test", "test", null, 0);
            var stringTestFromCache = cacheService.get("test");

            // Assert
            Assert.IsTrue(cacheService.get("test") == null, "The cache is being used.");
        }
    }
}
