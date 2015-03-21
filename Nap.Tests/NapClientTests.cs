using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using System.Net.Http.Headers;

using Nap.Configuration;

namespace Nap.Tests
{
    [TestClass]
    public class NapClientTests
    {
        private NapClient _nap;
        private TestHandler _handler;
        private HttpClient _httpClient;
        private string _url;

        [TestInitialize]
        public void Setup()
        {
            _url = "http://example.com/test";

            _nap = new NapClient();
            _nap.Config.FillMetadata = true;
            _handler = new TestHandler();
            _httpClient = new HttpClient(_handler);
            _nap.Config.Advanced.ClientCreator = request => _httpClient;
        }

        [TestMethod]
        [TestCategory("Nap")]
        public void Nap_CreatesNewNap()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Assert
            Assert.AreNotSame(NapClient.Lets, nap);
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_Get_PerformsHttpClientGet()
        {
            // Act
            var result = _nap.Get(_url).Execute<Result>();

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(HttpMethod.Get, _handler.Request.Method);
            Assert.AreEqual(null, _handler.Request.Content);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_Post_PerformsHttpClientPost()
        {
            // Act
            var result = _nap.Post(_url).Execute<Result>();

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(HttpMethod.Post, _handler.Request.Method);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_Put_PerformsHttpClientPut()
        {
            // Act
            var result = _nap.Put(_url).Execute<Result>();

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(HttpMethod.Put, _handler.Request.Method);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_Delete_PerformsHttpClientDelete()
        {
            // Act
            var result = _nap.Delete(_url).Execute<Result>();

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(HttpMethod.Delete, _handler.Request.Method);
            Assert.AreEqual(new Uri(_url), _handler.Request.RequestUri);
        }

        [TestMethod]
        [TestCategory("NapClient")]
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
                RequestContent = request.Content?.ReadAsStringAsync().Result;
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = content };
            }
        }

        public class Result
        {
            public int StatusCode { get; set; }
        }

        public class BadResult
        {
            BadResult(string constructorArg)
            {
            }

            public int StatusCode { get; set; }
        }
    }
}
