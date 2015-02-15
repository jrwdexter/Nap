using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nap.Configuration;
using Nap.Tests.TestClasses;

namespace Nap.Tests
{
    [TestClass]
    public class NapClientTests
    {
        [TestMethod]
        public void Nap_ConfigurationFromAppConfig_MatchesExpectation()
        {
            var nap = new Nap();
            NapSetup.AddConfig(NapConfig.GetCurrent());

            Assert.AreEqual("http://example.com", nap.Config.BaseUrl);
            Assert.AreEqual(true, nap.Config.FillMetadata);
            Assert.AreEqual(RequestFormat.Xml, nap.Config.Serialization);
        }

        [TestMethod]
        public async Task Something()
        {
            var response = await Nap.Lets.Post("http://example.com/")
                                         .IncludeQueryParameter("q", "v")
                                         .IncludeHeader("aHeader", "aValue")
                                         .IncludeBody(new TestClass { FirstName = "John", LastName = "Doe" })
                                         .ExecuteAsync();
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestSomething()
        {

            var person = new TestClass { FirstName = "John", LastName = "Doe" };
            var xmlSerializer = new XmlSerializer(person.GetType());
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, person);
                Console.Write(textWriter.ToString());
            }
        }
    }
}
