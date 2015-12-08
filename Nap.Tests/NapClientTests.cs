using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nap.Exceptions;
using System.Net.Http.Headers;
using FakeItEasy.ExtensionSyntax.Full;
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
        private string _otherUrl;

        [TestInitialize]
#if IMMUTABLE
        public void Setup_Immutable()
        {
            _url = "http://example.com/test";
            _otherUrl = "http://foobar.com/test";
            _handler = new TestHandler();
            var config =
                NapConfig.Default.SetMetadataBehavior(true)
                    .ConfigureAdvanced(a => a.SetClientCreator(objRequest =>
                    {
                        _handler.CookieContainer = new CookieContainer();
                        var request = (NapRequest) objRequest;
                        foreach (var cookie in request.Cookies)
                            _handler.CookieContainer.Add(cookie.Item1, cookie.Item2);
                        return new HttpClient(_handler);
                    }));
            _nap = new NapClient(config);
        }
#else
        public void Setup_Mutable()
        {
            _url = "http://example.com/test";
            _otherUrl = "http://foobar.com/test";

            _nap = new NapClient();
            _nap.Config.FillMetadata = true;
            _handler = new TestHandler();
            _httpClient = new HttpClient(_handler);
            _nap.Config.Advanced.ClientCreator = request =>
            {
                _handler.CookieContainer = new CookieContainer();
                foreach (var cookie in request.Cookies)
                    _handler.CookieContainer.Add(cookie.Item1, cookie.Item2);
                return new HttpClient(_handler);
            };
        }
#endif

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
#if IMMUTABLE
            Assert.AreEqual(string.Empty, _handler.RequestContent);
#else
            Assert.AreEqual(null, _handler.Request.Content);
#endif
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
#if IMMUTABLE
            _nap = new NapClient(_nap.Config.SetDefaultSerialization(RequestFormat.Json));
#else
            _nap.Config.Serialization = RequestFormat.Json;
#endif

            // Act
            _nap.Post(_url).IncludeBody(new { Foo = "Bar" }).Execute();

            //
            Assert.AreEqual(HttpMethod.Post, _handler.Request.Method);
            Assert.AreEqual("{\"Foo\":\"Bar\"}", _handler.RequestContent);
            Assert.AreEqual("application/json", _handler.Request.Content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_IncludeHeader_SendsHeader()
        {
            // Act
            const string key = "foo";
            const string value = "bar";
            _nap.Get(_url).IncludeHeader(key, value).Execute<Result>();

            // Assert
            Assert.AreEqual(1, _handler.Request.Headers.Count());
            Assert.AreEqual(1, _handler.Request.Headers.Count(h => h.Key == key));
            Assert.AreEqual(1, _handler.Request.Headers.First(h => h.Key == key).Value.Count());
            Assert.AreEqual(value, _handler.Request.Headers.First(h => h.Key == key).Value.First());
        }

        [TestMethod]
        [TestCategory("NapClient")]
        public void Nap_IncludeCookie_SendsCookie()
        {
            // Act
            const string key = "foo";
            const string value = "bar";
            _nap.Get(_url).IncludeCookie(_url, key, value).IncludeCookie(_otherUrl, value, key).Execute<Result>();

            // Assert
            Assert.AreEqual(1, _handler.Request.Headers.Count());
            Assert.AreEqual(1, _handler.Request.Headers.Count(h => h.Key.ToLower() == "cookie"));
            Assert.AreEqual($"{key}={value}", _handler.Request.Headers.First(h => h.Key == "cookie").Value.First());
        }

        public class TestHandler : HttpClientHandler
        {
            public HttpRequestMessage Request { get; set; }

            public string RequestContent { get; set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var cookies = CookieContainer.GetCookies(request.RequestUri);
                if (cookies.Count > 0)
                    request.Headers.Add("cookie", string.Join(";", CookieContainer.GetCookies(request.RequestUri).OfType<Cookie>().Select(c => $"{c.Name}={c.Value}")));
                Request = request;
                RequestContent = request.Content == null ? string.Empty : await request.Content?.ReadAsStringAsync();
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
