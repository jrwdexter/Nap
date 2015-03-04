using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nap.Configuration;

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
            _nap.Get(_url).Execute();

            // Assert
            Assert.AreEqual(HttpMethod.Get, _handler.Request.Method);
            Assert.AreEqual(null, _handler.Request.Content);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
        }

        [TestMethod]
        public void Nap_PostJson_ContentTypeIncluded()
        {
            // Arrange
            _nap.Config.Serialization = RequestFormat.Json;

            // Act
            _nap.Post(_url).IncludeBody(new { Foo = "Bar" }).Execute();

            //
            Assert.AreEqual(HttpMethod.Post, _handler.Request.Method);
            Assert.AreEqual("{\"Foo\":\"Bar\"}", _handler.RequestContent);
            Assert.AreEqual("application/json", _handler.Request.Content.Headers.ContentType.MediaType);
        }

        public class TestHandler : HttpMessageHandler
        {
            public HttpRequestMessage Request { get; set; }

            public string RequestContent { get; set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Request = request;
                RequestContent = request.Content.ReadAsStringAsync().Result;
                return new HttpResponseMessage { Content = new StringContent(string.Empty) };
            }
        }
    }
}
