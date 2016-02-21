using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Configuration;
using Nap.Configuration.Sections;
using Nap.Exceptions;
using Nap.Html;

namespace Nap.Configuration.Tests
{
    [TestClass]
    public class NapConfigurationTests
    {
        // Tests to ensure that *.config files get parsed correctly.

        [TestInitialize]
        public void Setup()
        {
            NapSetup.RegisterPlugin<NapConfigurationPlugin>();
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

        [TestMethod]
        [TestCategory("Configuration")]
        [TestCategory("Nap.Html")]
        public void GetConfiguration_FromConfigFile_Serializers_Match()
        {
            var serializers = NapClient.Lets.Config.Serializers.AsDictionary();

            // Assert
            Assert.IsNotNull(serializers);
            Assert.AreEqual(4, serializers.Count, "App.Config should populate one serializer, and 3 should be added by default.");
            Assert.AreEqual("text/html", serializers.Last().Key);
            Assert.IsInstanceOfType(serializers.Last().Value, typeof(NapHtmlSerializer));
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_Profile_HasProfile()
        {
            var config = (NapConfig)NapClient.Lets.Config;

            // Assert
            Assert.IsTrue(config.Profiles.Any(), "App.config should have at least one profile");
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_Profile_IsForFoobar()
        {
            var config = (NapConfig)NapClient.Lets.Config;

            // Assert
            Assert.IsTrue(config.Profiles.AsDictionary().ContainsKey("foobar"), "App.config should have a profile for foobar.com");
        }

        [TestMethod]
        [TestCategory("Configuration")]
        public void GetConfiguration_FromConfigFile_Profile_Foobar_HasValidValues()
        {
            var config = (NapConfig)NapClient.Lets.Config;
            var profile = config.Profiles.AsDictionary()["foobar"];

            // Assert
            Assert.AreEqual("http://www.foobar.com", profile.BaseUrl);
            Assert.IsFalse(profile.FillMetadata, "Foobar profile should have fillMetadata=false");
            Assert.AreEqual("Json", profile.Serialization);
            Assert.IsTrue(profile.Headers.Any(), "Foobar profile should have at least one header");
            Assert.IsTrue(profile.Headers.Any(h => h.Key == "TOKEN"), "Foobar profile should have at least one header with key 'TOKEN'");
            Assert.AreEqual("LONG_GUID", profile.Headers.First(h => h.Key == "TOKEN").Value);
        }
    }
}
