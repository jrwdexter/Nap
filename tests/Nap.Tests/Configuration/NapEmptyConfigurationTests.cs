using System.Linq;

using Xunit;

namespace Nap.Tests.Configuration
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Configuration")]
    public class NapEmptyConfigurationTests
    {
        private const string ExampleUrl = "http://example.com";

        [Fact]
        public void GetConfiguration_DoesNotThrowException()
        {
            var config = NapClient.Lets.Config;
        }

        [Fact]
        public void GetConfiguration_BaseUrl_Matches()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Act
            nap.Config.BaseUrl = ExampleUrl;

            // Assert
            Assert.Equal(ExampleUrl, nap.Config.BaseUrl);
        }

        [Fact]
        public void GetConfiguration_Headers_Match()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Act
            nap.Config.Headers.Add("foo", "bar");

            // Assert
            Assert.Equal(1, nap.Config.Headers.Count);
            Assert.Equal("foo", nap.Config.Headers.AsDictionary().Keys.First());
            Assert.Equal("bar", nap.Config.Headers.AsDictionary()["foo"]);
        }

        [Fact]
        public void GetConfiguration_Headers_RemoveHeader_LeavesNoHeader()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Act
            nap.Config.Headers.Add("foo", "bar");
            nap.Config.Headers.Remove("foo");

            // Assert
            Assert.Equal(0, nap.Config.Headers.Count);
        }

        [Fact]
        public void GetConfiguration_QueryParameters_Match()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Act
            nap.Config.QueryParameters.Add("foo", "bar");

            // Assert
            Assert.Equal(1, nap.Config.QueryParameters.Count);
            Assert.Equal("foo", nap.Config.QueryParameters.AsDictionary().Keys.First());
            Assert.Equal("bar", nap.Config.QueryParameters.AsDictionary()["foo"]);
        }

        [Fact]
        public void GetConfiguration_QueryParameters_RemoveQueryParameter_LeavesNoQueryParameter()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Act
            nap.Config.QueryParameters.Add("foo", "bar");
            nap.Config.QueryParameters.Remove("foo");

            // Assert
            Assert.Equal(0, nap.Config.QueryParameters.Count);
        }
    }
}
