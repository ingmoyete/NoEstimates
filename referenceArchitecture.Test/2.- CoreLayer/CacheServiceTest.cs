using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using referenceArchitecture.Test.Core.Factory;

namespace referenceArchitecture.Test._2.__CoreLayer
{
    [TestClass]
    public class CacheServiceTest
    {
        [TestMethod]
        public void setWithAbsoluteExpiration_contains_element()
        {
            string keyValue = "test";

            // Arrange
            var cache = Container.createICache();

            // Act
            cache.setWithAbsoluteExpiration(keyValue, keyValue, null, null);

            // Assert
            Assert.IsTrue((string)cache.get(keyValue) == keyValue, "The element is not saved in cache.");
        }

        [TestMethod]
        public void setWithSlidingExpiration_contains_element()
        {
            string keyValue = "test";

            // Arrange
            var cache = Container.createICache();

            // Act
            cache.setWithSlideExpiration(keyValue, keyValue, null, null);

            // Assert
            Assert.IsTrue((string)cache.get(keyValue) == keyValue, "The element is not saved in cache.");
        }

        [TestMethod]
        public void remove_element_justInserted()
        {
            string keyValue = "test";

            // Arrange
            var cache = Container.createICache();

            // Act
            cache.setWithSlideExpiration(keyValue, keyValue, null, null);
            cache.remove(keyValue);

            // Assert
            Assert.IsTrue((string)cache.get(keyValue) == null, "The element is not removed.");
        }

    }
}
