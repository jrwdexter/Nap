using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using System.Net.Http.Headers;
using FakeItEasy;
using Nap.Configuration;
using Nap.Plugins.Base;
using Nap.Tests.AutoFixtureConfiguration;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Xunit2;
using Xunit;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nap.Tests
{
#if IMMUTABLE
    [Trait("Library", "Nap.Immutable")]
#else
    [Trait("Library", "Nap")]
#endif
    [Trait("Type", "Base Functionality")]
    [Trait("Class", "NapClient")]
    public class NapClientTests
    {
        private NapClient _nap;
        private readonly TestHandler _handler;
        private readonly string _url;
        private readonly string _otherUrl;
        private readonly IFixture _fixture;
        private readonly IPlugin _plugin;
        private readonly NapConfig _config;
        private readonly INapRequest _napRequest;

        public NapClientTests()
        {
            _url = "http://example.com/test";
            _otherUrl = "http://foobar.com/test";

            _handler = new TestHandler();
            _fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());
#if IMMUTABLE
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
            _fixture.Inject(config);
            _nap = new NapClient(config);
#else

            _nap = new NapClient { Config = { FillMetadata = true } };
            _nap.Config.Advanced.ClientCreator = request =>
            {
                _handler.CookieContainer = new CookieContainer();
                foreach (var cookie in request.Cookies)
                    _handler.CookieContainer.Add(cookie.Item1, cookie.Item2);
                return new HttpClient(_handler);
            };
            _plugin = _fixture.Freeze<IPlugin>();
            A.CallTo(() => _plugin.Configure(A<INapConfig>._)).Returns(_fixture.Freeze<NapConfig>());
            A.CallTo(() => _plugin.Prepare(A<NapRequest>._)).Returns(_napRequest as NapRequest);
            A.CallTo(() => _plugin.Execute<Result>(A<INapRequest>._)).Returns(null);
            A.CallTo(() => _plugin.Process(A<NapResponse>._)).Returns(null);
#endif

            _config = _fixture.Freeze<NapConfig>();
            _napRequest = _fixture.Freeze<INapRequest>();
        }

        [Fact]
        public void Nap_CreatesNewNap()
        {
            // Arrange
            var nap = NapClient.Lets;

            // Assert
            Assert.NotSame(NapClient.Lets, nap);
        }

        [Theory, AutoData]
        public void Nap_WithUrl_CreatesNewNap_WithUrl(Uri uri)
        {
            var nap = new NapClient(uri.ToString());

            Assert.Equal(uri.ToString(), nap.Config.BaseUrl);
        }

        [Fact]
        public void Nap_WithConfig_CreatesNewNap_WithConfig()
        {
            var nap = new NapClient(_config);

            Assert.Same(_config, nap.Config);
        }

#if IMMUTABLE
#else
        [Fact]
        public void Nap_WithSetup_CreatesNewNap_WithSetupApplied_And_PluginMethodsRun()
        {
            // Arrange
            var setup = new NapSetup();
            setup.InstallPlugin(_plugin);

            // Act
            var nap = new NapClient(setup);
            nap.Get("test");

            // Assert
            A.CallTo(() => _plugin.Configure(A<INapConfig>._)).MustHaveHappened();
            A.CallTo(() => _plugin.Prepare(A<NapRequest>._)).MustNotHaveHappened();
            A.CallTo(() => _plugin.Execute<Result>(A<INapRequest>._)).MustNotHaveHappened();
            A.CallTo(() => _plugin.Process(A<NapResponse>._)).MustNotHaveHappened();
        }

        [Fact]
        public void Nap_WithSetup_AndConfiguraiton_CreatesNewNap_WithSetupApplied_And_PluginMethodsRun()
        {
            // Arrange
            var setup = new NapSetup();
            setup.InstallPlugin(_plugin);

            // Act
            var nap = new NapClient(_config, setup);
            var request = nap.Get("test");

            // Assert
            Assert.Same(_config, nap.Config);
            A.CallTo(() => _plugin.Configure(A<INapConfig>._)).MustHaveHappened();
            A.CallTo(() => _plugin.Prepare(A<NapRequest>._)).MustNotHaveHappened();
            A.CallTo(() => _plugin.Execute<Result>(A<INapRequest>._)).MustNotHaveHappened();
            A.CallTo(() => _plugin.Process(A<NapResponse>._)).MustNotHaveHappened();
        }
#endif

        [Fact]
        public void Nap_Get_PerformsHttpClientGet()
        {
            // Act
            var result = _nap.Get(_url).Execute<Result>();

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(HttpMethod.Get, _handler.Request.Method);
#if IMMUTABLE
            Assert.Equal(string.Empty, _handler.RequestContent);
#else
            Assert.Equal(null, _handler.Request.Content);
#endif
            Assert.Equal(new Uri(_url), _handler.Request.RequestUri);
        }

        [Fact]
        public void Nap_Post_PerformsHttpClientPost()
        {
            // Act
            var result = _nap.Post(_url).Execute<Result>();

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(HttpMethod.Post, _handler.Request.Method);
            Assert.Equal(new Uri(_url), _handler.Request.RequestUri);
        }

        [Fact]
        public void Nap_Put_PerformsHttpClientPut()
        {
            // Act
            var result = _nap.Put(_url).Execute<Result>();

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(HttpMethod.Put, _handler.Request.Method);
            Assert.Equal(new Uri(_url), _handler.Request.RequestUri);
        }

        [Fact]
        public void Nap_Delete_PerformsHttpClientDelete()
        {
            // Act
            var result = _nap.Delete(_url).Execute<Result>();

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(HttpMethod.Delete, _handler.Request.Method);
            Assert.Equal(new Uri(_url), _handler.Request.RequestUri);
        }

        [Fact]
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
            Assert.Equal(HttpMethod.Post, _handler.Request.Method);
            Assert.Equal("{\"Foo\":\"Bar\"}", _handler.RequestContent);
            Assert.Equal("application/json", _handler.Request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void Nap_IncludeHeader_SendsHeader()
        {
            // Act
            const string key = "foo";
            const string value = "bar";
            _nap.Get(_url).IncludeHeader(key, value).Execute<Result>();

            // Assert
            Assert.Equal(1, _handler.Request.Headers.Count());
            Assert.Equal(1, _handler.Request.Headers.Count(h => h.Key == key));
            Assert.Equal(1, _handler.Request.Headers.First(h => h.Key == key).Value.Count());
            Assert.Equal(value, _handler.Request.Headers.First(h => h.Key == key).Value.First());
        }

        [Fact]
        public void Nap_IncludeCookie_SendsCookie()
        {
            // Act
            const string key = "foo";
            const string value = "bar";
            _nap.Get(_url).IncludeCookie(_url, key, value).IncludeCookie(_otherUrl, value, key).Execute<Result>();

            // Assert
            Assert.Equal(1, _handler.Request.Headers.Count());
            Assert.Equal(1, _handler.Request.Headers.Count(h => h.Key.ToLower() == "cookie"));
            Assert.Equal($"{key}={value}", _handler.Request.Headers.First(h => h.Key == "cookie").Value.First());
        }

        [Fact]
        public void Nap_Result_Includes_CookiesAndHeaders()
        {
            // Act
            const string key = "foo";
            const string value = "bar";
            var result = _nap.Get(_url).Execute<Result>();

            // Assert
            Assert.True(result.Headers.Count >= 2); // 2 specified headers
            Assert.Equal(1, result.Cookies.Count());
            Assert.Equal("12345", result.SessionId);
            Assert.Equal("application/json", result.ContentType);
        }

        [Fact]
        public void Nap_Response_Chain_ToRequest_Includes_Cookie()
        {
            // Arrange
            var request = _nap.Get(_url) as NapRequest;

            // Assert
            Assert.True(request.Cookies.Count() == 0);

            // Act
            request = ((INapRequest)request).ExecuteRaw().ChainRequest(_url, HttpMethod.Get) as NapRequest;

            // Assert
            Assert.True(request.Cookies.Count() > 0);
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
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content };
                response.Headers.Add("Set-Cookie", "Session-Id = 12345");
                return response;
            }
        }

        public class Result
        {
            public int StatusCode { get; set; }
            public IEnumerable<NapCookie> Cookies { get; set; }
            public IDictionary<string, string> Headers { get; set; }
            public string ContentType { get; set; }
            public string SessionId { get; set; }
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
