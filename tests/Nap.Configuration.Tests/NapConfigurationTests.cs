using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Configuration.Sections;
using Nap.Exceptions;

namespace Nap.Configuration.Tests
{
    [TestClass]
    public class NapConfigurationTests
    {
        private NapClient _napClient;

        // Tests to ensure that *.config files get parsed correctly.

        [TestInitialize]
        public void Setup()
        {
            _napClient = new NapClient(NapConfig.GetCurrent());
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_DoesNotThrowException()
        {
            var config = _napClient.Config;
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_BaseUrl_Matches()
        {
            Assert.AreEqual("http://example.com", _napClient.Config.BaseUrl);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_Headers_Match()
        {
            var headers = _napClient.Config.Headers.AsDictionary();

            // Assert
            Assert.IsNotNull(headers);
            Assert.AreEqual(1, headers.Count, "App.Config should create one, and only one, header.");
            Assert.AreEqual("testHeader", headers.First().Key);
            Assert.AreEqual("testHeaderValue", headers.First().Value);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_QueryParameters_Match()
        {
            var queryParameters = _napClient.Config.QueryParameters.AsDictionary();

            // Assert
            Assert.IsNotNull(queryParameters);
            Assert.AreEqual(1, queryParameters.Count, "App.Config should create one, and only one, query parameter.");
            Assert.AreEqual("testQueryParameter", queryParameters.First().Key);
            Assert.AreEqual("testQueryParameterValue", queryParameters.First().Value);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        [ExpectedException(typeof(NapConfigurationException))]
        public void AddInvalidHeader_ThrowsConfigurationException()
        {
            var config = _napClient.Config;
            ((Headers)config.Headers).Add(new EmptyHeader());
        }

        [TestMethod]
        [TestCategory("Configuration")]
        [ExpectedException(typeof(NapConfigurationException))]
        public void AddInvalidQueryParameter_ThrowsConfigurationException()
        {
            var config = _napClient.Config;
            ((QueryParameters)config.QueryParameters).Add(new EmptyQueryParameter());
        }
    }
}
