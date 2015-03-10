using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Formatters.Base;

namespace Nap.Tests.Configuration
{
    [TestClass]
    public class NapEmptyConfigurationTests
    {
        private const string _exampleUrl = "http://example.com";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_DoesNotThrowException()
        {
            var config = Nap.Lets.Config;
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_BaseUrl_Matches()
        {
            // Arrange
            var nap = Nap.Lets;

            // Act
            nap.Config.BaseUrl = _exampleUrl;

            // Assert
            Assert.AreEqual(_exampleUrl, nap.Config.BaseUrl);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_Headers_Match()
        {
            // Arrange
            var nap = Nap.Lets;

            // Act
            nap.Config.Headers.Add("foo", "bar");

            // Assert
            Assert.AreEqual(1, nap.Config.Headers.Count, "One and only one header was added.");
            Assert.AreEqual("foo", nap.Config.Headers.AsDictionary().Keys.First());
            Assert.AreEqual("bar", nap.Config.Headers.AsDictionary()["foo"]);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_Headers_RemoveHeader_LeavesNoHeader()
        {
            // Arrange
            var nap = Nap.Lets;
            
            // Act
            nap.Config.Headers.Add("foo", "bar");
            nap.Config.Headers.Remove("foo");

            // Assert
            Assert.AreEqual(0, nap.Config.Headers.Count, "No query parameters should remain");
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_QueryParameters_Match()
        {
            // Arrange
            var nap = Nap.Lets;

            // Act
            nap.Config.QueryParameters.Add("foo", "bar");

            // Assert
            Assert.AreEqual(1, nap.Config.QueryParameters.Count, "One and only one header was added.");
            Assert.AreEqual("foo", nap.Config.QueryParameters.AsDictionary().Keys.First());
            Assert.AreEqual("bar", nap.Config.QueryParameters.AsDictionary()["foo"]);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_QueryParameters_RemoveQueryParameter_LeavesNoQueryParameter()
        {
            // Arrange
            var nap = Nap.Lets;

            // Act
            nap.Config.QueryParameters.Add("foo", "bar");
            nap.Config.QueryParameters.Remove("foo");

            // Assert
            Assert.AreEqual(0, nap.Config.QueryParameters.Count, "No query parameters should remain");
        }
    }
}
