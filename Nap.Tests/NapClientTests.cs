using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Nap.Configuration;
using Nap.Tests.TestClasses;

namespace Nap.Tests
{
    [TestClass]
    public class NapClientTests
    {
        private Nap _nap;
        private TestHandler _handler;
        private HttpClient _httpClient;
        private string _url;

        [TestInitialize]
        public void Setup()
        {
            _url = "http://example.com/test";

            _nap = new Nap();
            _nap.Config.FillMetadata = true;
            _handler = new TestHandler();
            _httpClient = new HttpClient(_handler);
            _nap.Config.Advanced.ClientCreator = request => _httpClient;
        }

        [TestMethod]
        public void Nap_Get_PerformsHttpClientGet()
        {
            // Act
            _nap.Get(_url);

            // Assert
            Assert.AreEqual(HttpMethod.Get, _handler.Request.Method);
            Assert.AreEqual(null, _handler.Request.Content);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
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

        public class TestHandler : HttpMessageHandler
        {
            public HttpRequestMessage Request { get; set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Request = request;
                return new Task<HttpResponseMessage>(() => new HttpResponseMessage()); // Return empty response
            }
        }
    }
}
