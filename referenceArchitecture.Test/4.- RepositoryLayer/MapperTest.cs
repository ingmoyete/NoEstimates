using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoEstimates.repository.Core.Mapper;
using referenceArchitecture.Test.Core.Factory;

namespace NoEstimates.Test.RepositoryLayer
{
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void MapperT()
        {
            var mapper = Container.createIMapper();
            Assert.IsTrue(true);
        }
    }
}
