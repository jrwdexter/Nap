using System;
using System.Reflection;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Nap.Configuration;
using Nap.Exceptions;
using Nap.Plugins.Base;
using Nap.Proxies;
using Nap.Serializers.Base;

namespace Nap
{
    /// <summary>
    /// An easily configurable request.
    /// </summary>
    public partial class NapRequest : INapRequest
    {
        private readonly INapConfig _config;
        private readonly string _url;
        private readonly HttpMethod _method;
        private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly List<Tuple<Uri, Cookie>> _cookies = new List<Tuple<Uri, Cookie>>();
        private string _content;
        private bool _doNot;

        /// <summary>
        /// Initializes a new instance of the <see cref="NapRequest" /> class.
        /// </summary>
        /// <param name="initialConfiguration">The initial configuration for the request.</param>
        /// <param name="url">The URL to perform the request against.  Can be a full URL or a partial.</param>
        /// <param name="method">The method to use in the request.</param>
        internal NapRequest(INapConfig initialConfiguration, string url, HttpMethod method)
        {
            // Set up initial configuration parameters.
            _config = initialConfiguration;
            foreach (var header in _config.Headers)
                _headers.Add(header.Key, header.Value);
            foreach (var queryParameter in _config.QueryParameters)
                _queryParameters.Add(queryParameter.Key, queryParameter.Value);
            if (_config.Advanced.Proxy.Address != null)
                Advanced.UseProxy(_config.Advanced.Proxy.Address);
            if (!string.IsNullOrEmpty(_config.Advanced.Authentication.Username) && !string.IsNullOrEmpty(_config.Advanced.Authentication.Password))
                Advanced.Authentication.Basic(_config.Advanced.Authentication.Username, _config.Advanced.Authentication.Password);
            ClientCreator = _config.Advanced.ClientCreator;

            _url = url;
            _method = method;
        }

        #region Properties

        /// <summary>
        /// Gets the set of cookies that has been configured for this request.
        /// Required for configuration of the request with a custom <see cref="HttpClient"/>.
        /// </summary>
        public IReadOnlyList<Tuple<Uri, Cookie>> Cookies => _cookies;

        /// <summary>
        /// Gets the set of headers that have been configured for this request.
        /// </summary>
        public IReadOnlyDictionary<string, string> Headers => _headers; 

        /// <summary>
        /// Gets the set of query parameters that have been configured for this request.
        /// </summary>
        public IReadOnlyDictionary<string, string> QueryParamters => _queryParameters;

        /// <summary>
        /// Gets the method this request will be sent using.
        /// </summary>
        public HttpMethod Method => _method;

        /// <summary>
        /// Gets the content of this request, if any.
        /// </summary>
        public string Content => _content;

        /// <summary>
        /// Gets the
        /// </summary>
        public string Url => _url;
        #endregion

        #region Include/Fill

        /// <summary>
        /// Gets a set of methods to perform some removal of data from the request.
        /// </summary>
        public INapRemovableRequestComponent DoNot
        {
            get
            {
                _doNot = true;
                return this;
            }
        }

        /// <summary>
        /// Gets the advanced collection of methods for <see cref="INapRequest"/> configuration.
        /// </summary>
        public IAdvancedNapRequestComponent Advanced
        {
            get
            { return this; }
        }

        /// <summary>
        /// Includes the query parameter in the value for the URL.
        /// </summary>
        /// <param name="key">The key for the query parameter to include.</param>
        /// <param name="value">The value of the query parameter to include.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeQueryParameter(string key, string value)
        {
            _queryParameters.Add(key, value);
            return this;
        }

        /// <summary>
        /// Includes some content in the body, serialized according to <see cref="INapSerializer"/>s.
        /// </summary>
        /// <param name="body">The object to serialize into the body.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeBody(object body)
        {
            RunBooleanPluginMethod(p => p.BeforeRequestSerialization(this), "Nap plugin returned false.  Request serialization cancelled.");

            try
            {
                _content = _config.Serializers.AsDictionary()[_config.Serialization].Serialize(body);
            }
            catch (Exception ex)
            {
                throw new NapSerializationException($"Serialization failed for data:\n {body}", ex);
            }

            RunBooleanPluginMethod(p => p.AfterRequestSerialization(this), "Nap plugin returned false.  Request post-serialization cancelled.");
            return this;
        }

        /// <summary>
        /// Includes a header in the request.
        /// </summary>
        /// <param name="headerName">Name of the header to include.</param>
        /// <param name="value">The value of the header to send.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeHeader(string headerName, string value)
        {
            _headers.Add(headerName, value);
            return this;
        }

        /// <summary>
        /// Includes a cookie in the request.
        /// </summary>
        /// <param name="uri">The URL the cookie corresponds to.</param>
        /// <param name="cookieName">The name of the cookie to include.</param>
        /// <param name="value">The value of the cookie to include.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeCookie(string uri, string cookieName, string value)
        {
            var asUri = new Uri(uri);
            _cookies.Add(new Tuple<Uri, Cookie>(asUri, new Cookie(cookieName, value, asUri.AbsolutePath, asUri.Host)));
            return this;
        }

        /// <summary>
        /// Fills the response object with metadata using special keys, such as "StatusCode".
        /// If used after <see cref="DoNot"/>, instead removes the fill metadata flag.
        /// </summary>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest FillMetadata()
        {
            _config.FillMetadata = _doNot == false;
            _doNot = false;
            return this;
        }

        #endregion

        #region Execution

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <returns>A task, that when run returns the body content.</returns>
        public async Task<string> ExecuteAsync()
        {
            var napPluginResult = NapSetup.Plugins.Aggregate<IPlugin, string>(null, (current, plugin) => current ?? plugin.Execute(this) as string);
            if (napPluginResult != null)
                return napPluginResult;
            return (await RunRequestAsync()).Content;
        }

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// A task, that when run returns the body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="INapConfig.Serializers"/>.
        /// </returns>
        public async Task<T> ExecuteAsync<T>() where T : class, new()
        {
            var napPluginResult = NapSetup.Plugins.Aggregate<IPlugin, T>(null, (current, plugin) => current ?? plugin.Execute(this) as T);
            if (napPluginResult != null)
                return napPluginResult;
            var responseWithContent = await RunRequestAsync();
            var toReturn = GetSerializer(responseWithContent.Response.Content.Headers.ContentType.MediaType).Deserialize<T>(responseWithContent.Content) ?? new T();

            if (_config.FillMetadata)
            {
                var property = typeof(T).GetRuntimeProperty("StatusCode");
                property?.SetValue(toReturn, Convert.ChangeType(responseWithContent.Response.StatusCode, property.PropertyType));

                property = typeof(T).GetRuntimeProperty("Cookies");
                var cookies = responseWithContent.Response.Headers.Where(h => h.Key.StartsWith("set-cookie", StringComparison.OrdinalIgnoreCase));
                var simpleCookies = cookies.Select(c => new KeyValuePair<string, string>(c.Key, c.Value.FirstOrDefault()));
                property?.SetValue(toReturn, simpleCookies);

                // TODO: Populate items with defaults (statuscode, etc)
            }

            return toReturn;
        }

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The response body content.</returns>
        public string Execute()
        {
            var execution = ExecuteAsync();
            execution.ConfigureAwait(false);
            return execution.Result;
        }

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// The body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="INapConfig.Serializers"/>.
        /// </returns>
        public T Execute<T>() where T : class, new()
        {
            var execution = ExecuteAsync<T>();
            execution.ConfigureAwait(false);
            return execution.Result;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The content and response.</returns>
        private async Task<InternalResponse> RunRequestAsync()
        {
            var excludedHeaders = new[] { "content-type" };
            using (var client = ClientCreator != null ? ClientCreator(this) : CreateClient())
            {
                var content = new StringContent(_content ?? string.Empty);
                var url = CreateUrl();
                var allowedDefaultHeaders = _headers.Where(h => !excludedHeaders.Any(eh => eh.Equals(h.Key, StringComparison.OrdinalIgnoreCase)));
                foreach (var header in allowedDefaultHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var contentType = _headers.FirstOrDefault(kv => kv.Key.Equals("content-type", StringComparison.OrdinalIgnoreCase));
                if (contentType.Value != null)
                {
                    content.Headers.ContentType.MediaType = contentType.Value;
                }
                else
                    content.Headers.ContentType.MediaType = _config.Serializers.AsDictionary()[_config.Serialization].ContentType;

                HttpResponseMessage response = null;
                if (_method == HttpMethod.Get)
                    response = await client.GetAsync(new Uri(url));
                if (_method == HttpMethod.Post)
                    response = await client.PostAsync(new Uri(url), content);
                if (_method == HttpMethod.Put)
                    response = await client.PutAsync(new Uri(url), content);
                if (_method == HttpMethod.Delete)
                    response = await client.DeleteAsync(new Uri(url));
                if (response == null)
                    return await new Task<InternalResponse>(() => null);

                using (var ms = new MemoryStream())
                using (var sr = new StreamReader(ms))
                {
                    await response.Content.CopyToAsync(ms);
                    ms.Position = 0;
                    var responseContent = await sr.ReadToEndAsync();
                    return new InternalResponse { Response = response, Content = responseContent };
                }
            }
        }

        /// <summary>
        /// Creates the HttpClient for usage.
        /// </summary>
        /// <returns>The newly created and configured <see cref="HttpClient"/>.</returns>
        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();
            if (_config.Advanced.Proxy?.Address != null)
            {
                handler.Proxy = new NapWebProxy(_config.Advanced.Proxy.Address);
                handler.UseProxy = true;
            }

            foreach (var cookie in _cookies)
            {
                handler.CookieContainer.Add(cookie.Item1, cookie.Item2);
            }

            var client = new HttpClient(handler);
            return client;
        }

        /// <summary>
        /// Creates the URL from an (optional) <see cref="INapConfig.BaseUrl"/>, URL and query parameters.
        /// </summary>
        /// <returns>The fully formed URL.</returns>
        private string CreateUrl()
        {
            var urlTemp = _url;
            if (!_url.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(_config.BaseUrl))
                urlTemp = new Uri(new Uri(_config.BaseUrl), urlTemp).ToString();
            if (_queryParameters.Any())
            {
                urlTemp = _url.Contains("?") ?
                    (_url.EndsWith("?", StringComparison.OrdinalIgnoreCase) ? _url : _url + "&") :
                    _url + "?";
                urlTemp += string.Join("&", _queryParameters.Select(x => string.Format("{0}={1}", x.Key, WebUtility.UrlEncode(x.Value))));
            }

            return urlTemp;
        }

        /// <summary>
        /// Gets the serializer for a given content type.
        /// </summary>
        /// <param name="contentType">Type of the content (eg. application/json).</param>
        /// <returns>The serializer matching the content type.</returns>
        private INapSerializer GetSerializer(string contentType)
        {
            INapSerializer serializer;
            var serializers = _config.Serializers.AsDictionary();
            if (!serializers.TryGetValue(contentType, out serializer))
            {
                var getSubType = new Func<string, string>(type => type.Substring(type.IndexOf("/")));
                var subType = getSubType(contentType);
                serializer = serializers.FirstOrDefault(f => getSubType(f.Key) == subType).Value;

                if (serializer == null)
                    throw new NapSerializationException($"No serializer was found for content type {contentType}");
            }

            return serializer;
        }

        private void RunBooleanPluginMethod(Func<IPlugin, bool> pluginMethod, string errorMessage)
        {
            try
            {
                if (!NapSetup.Plugins.Aggregate(true, (current, plugin) => current && pluginMethod.Invoke(plugin)))
                    throw new NapPluginException(errorMessage);
            }
            catch (Exception e)
            {
                throw new NapPluginException($"{errorMessage} See inner exception for details.", e); // TODO: Make specific exception
            }
        }

        #endregion

        /// <summary>
        /// A child private class to house response and content information.
        /// </summary>
        private sealed class InternalResponse
        {
            /// <summary>
            /// Gets or sets the response.
            /// </summary>
            public HttpResponseMessage Response { get; set; }

            /// <summary>
            /// Gets or sets the content from the response body.
            /// </summary>
            public string Content { get; set; }
        }
    }
}