using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Configuration.Sections;
using Nap.Exceptions;
using Nap.Html;

namespace Nap.Tests.Configuration
{
    [TestClass]
    public class NapConfigurationTests
    {
        // Tests to ensure that *.config files get parsed correctly.

        [TestInitialize]
        public void Setup()
        {
            NapSetup.AddConfig(NapConfig.GetCurrent);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_DoesNotThrowException()
        {
            var config = NapClient.Lets.Config;
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_BaseUrl_Matches()
        {
            Assert.AreEqual("http://example.com", NapClient.Lets.Config.BaseUrl);
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_Headers_Match()
        {
            var headers = NapClient.Lets.Config.Headers.AsDictionary();

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
            var queryParameters = NapClient.Lets.Config.QueryParameters.AsDictionary();

            // Assert
            Assert.IsNotNull(queryParameters);
            Assert.AreEqual(1, queryParameters.Count, "App.Config should create one, and only one, query parameter.");
            Assert.AreEqual("testQueryParameter", queryParameters.First().Key);
            Assert.AreEqual("testQueryParameterValue", queryParameters.First().Value);
        }

        [TestMethod]
        [TestCategory("Configuration")]
		[TestCategory("Nap.Html")]
		public void GetConfiguration_FromConfigFile_Formatters_Match()
		{
			var formatters = NapClient.Lets.Config.Formatters.AsDictionary();

			// Assert
			Assert.IsNotNull(formatters);
			Assert.AreEqual(4, formatters.Count, "App.Config should populate one formatter, and 3 should be added by default.");
			Assert.AreEqual("text/html", formatters.Last().Key);
			Assert.IsInstanceOfType(formatters.Last().Value, typeof(NapHtmlFormatter));
		}

        [TestMethod]
        [TestCategory("Configuration")]
        [ExpectedException(typeof(NapConfigurationException))]
        public void AddInvalidHeader_ThrowsConfigurationException()
        {
            var config = NapClient.Lets.Config;
            ((Headers)config.Headers).Add(new EmptyHeader());
        }

        [TestMethod]
        [TestCategory("Configuration")]
        [ExpectedException(typeof(NapConfigurationException))]
        public void AddInvalidQueryParameter_ThrowsConfigurationException()
        {
            var config = NapClient.Lets.Config;
            ((QueryParameters)config.QueryParameters).Add(new EmptyQueryParameter());
        }
    }
}
