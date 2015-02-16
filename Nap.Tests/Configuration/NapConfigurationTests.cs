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
    public class NapConfigurationTests
    {
        [TestInitialize]
        public void Setup()
        {
            NapSetup.AddConfig(NapConfig.GetCurrent);
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
            Assert.AreEqual("http://example.com", Nap.Lets.Config.BaseUrl);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_Headers_Match()
        {
            var headers = Nap.Lets.Config.Headers.AsDictionary();

            // Assert
            Assert.IsNotNull(headers);
            Assert.AreEqual(1, headers.Count, "App.Config should create one, and only one, header.");
            Assert.AreEqual("testHeader", headers.First().Key);
            Assert.AreEqual("testHeaderValue", headers.First().Value);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_QueryParameters_Match()
        {
            var queryParameters = Nap.Lets.Config.QueryParameters.AsDictionary();

            // Assert
            Assert.IsNotNull(queryParameters);
            Assert.AreEqual(1, queryParameters.Count, "App.Config should create one, and only one, query parameter.");
            Assert.AreEqual("testQueryParameter", queryParameters.First().Key);
            Assert.AreEqual("testQueryParameterValue", queryParameters.First().Value);
        }
    }
}
