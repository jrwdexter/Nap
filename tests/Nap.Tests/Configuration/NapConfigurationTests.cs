using System;
using System.Collections.Generic;
using System.Linq;


using Nap.Configuration;
using Nap.Configuration.Sections;
using Nap.Exceptions;
using Nap.Html;
using Xunit;

namespace Nap.Configuration.Tests
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Configuration")]
    public class NapConfigurationTests
    {
        private readonly NapSetup _setup;
        // Tests to ensure that *.config files get parsed correctly.

        public NapConfigurationTests()
        {
            _setup = new NapSetup();
            _setup.InstallPlugin<NapConfigurationPlugin>();
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_DoesNotThrowException()
        {
            var config = new NapClient(_setup);
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_BaseUrl_Matches()
        {
            var nap = new NapClient(_setup);
            Assert.Equal("http://example.com", nap.Config.BaseUrl);
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_Headers_Match()
        {
            var nap = new NapClient(_setup);
            var headers = nap.Config.Headers.AsDictionary();

            // Assert
            Assert.NotNull(headers);
            Assert.Equal(1, headers.Count);
            Assert.Equal("testHeader", headers.First().Key);
            Assert.Equal("testHeaderValue", headers.First().Value);
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_QueryParameters_Match()
        {
            var nap = new NapClient(_setup);
            var queryParameters = nap.Config.QueryParameters.AsDictionary();

            // Assert
            Assert.NotNull(queryParameters);
            Assert.Equal(1, queryParameters.Count);
            Assert.Equal("testQueryParameter", queryParameters.First().Key);
            Assert.Equal("testQueryParameterValue", queryParameters.First().Value);
        }

        [Fact]
        public void AddInvalidHeader_ThrowsConfigurationException()
        {
            Assert.Throws<NapConfigurationException>(() =>
            {
                var nap = new NapClient(_setup);
                var config = nap.Config;
                ((Headers)config.Headers).Add(new EmptyHeader());
            });
        }

        [Fact]
        public void AddInvalidQueryParameter_ThrowsConfigurationException()
        {
            Assert.Throws<NapConfigurationException>(() =>
            {
                var nap = new NapClient(_setup);
                var config = nap.Config;
                ((QueryParameters)config.QueryParameters).Add(new EmptyQueryParameter());
            });
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_Serializers_Match()
        {
            var nap = new NapClient(_setup);
            var serializers = nap.Config.Serializers.AsDictionary();

            // Assert
            Assert.NotNull(serializers);
            Assert.Equal(4, serializers.Count);
            Assert.Equal("text/html", serializers.Last().Key);
            Assert.IsType<NapHtmlSerializer>(serializers.Last().Value);
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_Profile_HasProfile()
        {
            var nap = new NapClient(_setup);
            var config = (NapConfig)nap.Config;

            // Assert
            Assert.True(config.Profiles.Any(), "App.config should have at least one profile");
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_Profile_IsForFoobar()
        {
            var nap = new NapClient(_setup);
            var config = (NapConfig)nap.Config;

            // Assert
            Assert.True(config.Profiles.AsDictionary().ContainsKey("foobar"), "App.config should have a profile for foobar.com");
        }

        [Fact]
        public void GetConfiguration_FromConfigFile_Profile_Foobar_HasValidValues()
        {
            var nap = new NapClient(_setup);
            var config = (NapConfig)nap.Config;
            var profile = config.Profiles.AsDictionary()["foobar"];

            // Assert
            Assert.Equal("http://www.foobar.com", profile.BaseUrl);
            Assert.False(profile.FillMetadata, "Foobar profile should have fillMetadata=false");
            Assert.Equal("Json", profile.Serialization);
            Assert.True(profile.Headers.Any(), "Foobar profile should have at least one header");
            Assert.True(profile.Headers.Any(h => h.Key == "TOKEN"), "Foobar profile should have at least one header with key 'TOKEN'");
            Assert.Equal("LONG_GUID", profile.Headers.First(h => h.Key == "TOKEN").Value);
        }
    }
}
