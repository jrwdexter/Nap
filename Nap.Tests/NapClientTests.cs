using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Napper.Configuration;

namespace Napper.Tests
{
    [TestClass]
    public class NapClientTests
    {
        [TestMethod]
        public void Nap_ConfigurationFromAppConfig_MatchesExpectation()
        {
            var nap = new Nap();
            Assert.AreEqual("http://example.com", nap.Config.BaseUrl);
            Assert.AreEqual(true, nap.Config.FillMetadata);
            Assert.AreEqual(RequestFormat.Xml, nap.Config.Serialization);
        }

        [TestMethod]
        public async Task Something()
        {
            var response = await Nap.Lets.Get("http://example.com/")
                                         .IncludeQueryParameter("q", "v")
                                         .IncludeHeader("aHeader", "aValue")
                                         .IncludeBody(new TestClass { FirstName = "John", LastName = "Doe" })
                                         .ExecuteAsync();
            Assert.IsNotNull(response);
        }
    }
}
