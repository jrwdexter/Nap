using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Napper.Base;
using Napper.Configuration;

namespace Napper
{
    /// <summary>
    /// An easily configurable request.
    /// </summary>
    internal partial class NapRequest : INapRequest, INapRemovableRequestComponent
    {
        private readonly NapConfig _config;
        private readonly string _url;
        private readonly HttpMethod _method;
        private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private string _content;
        private bool _doNot;

        /// <summary>
        /// Initializes a new instance of the <see cref="NapRequest" /> class.
        /// </summary>
        /// <param name="initialConfiguration">The initial configuration for the request.</param>
        /// <param name="url">The URL to perform the request against.  Can be a full URL or a partial.</param>
        /// <param name="method">The method to use in the request.</param>
        internal NapRequest(NapConfig initialConfiguration, string url, HttpMethod method)
        {
            _config = initialConfiguration;
            _url = url;
            _method = method;
        }

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
        /// Includes some content in the body, serialized according to <see cref="NapConfig.ContentFormat"/>.
        /// </summary>
        /// <param name="body">The object to serialize into the body.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeBody(object body)
        {
            _content = _config.Serializers[_config.ContentFormat].Serialize(body);
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

        /// <summary>
        /// Excludes the body from the request.
        /// </summary>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeBody()
        {
            if (_doNot)
            {
                _content = null;
                _doNot = false;
            }

            return this;
        }

        /// <summary>
        /// Excludes the header with key <see cref="headerName"/>.
        /// </summary>
        /// <param name="headerName">The header name to be removed.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeHeader(string headerName)
        {
            if (_doNot)
            {
                if (_headers.Keys.Any(k => k == headerName))
                {
                    _headers.Remove(headerName);
                }

                _doNot = false;
            }

            return this;
        }

        /// <summary>
        /// Excludes the query with key <see cref="key"/>.
        /// </summary>
        /// <param name="key">The key of the query parameter to remove.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeQueryParameter(string key)
        {
            if (_doNot)
            {
                if (_queryParameters.Keys.Any(k => k == key))
                {
                    _queryParameters.Remove(key);
                }

                _doNot = false;
            }

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
            return (await RunRequest()).Content;
        }

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// A task, that when run returns the body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="NapConfig.AcceptFormat"/>.
        /// </returns>
        public async Task<T> ExecuteAsync<T>()
        {
            var responseWithContent = await RunRequest();
            var toReturn = _config.Serializers[_config.AcceptFormat].Deserialize<T>(responseWithContent.Content);

            if (_config.FillMetadata)
            {
                var property = typeof(T).GetProperty("StatusCode", BindingFlags.Instance | BindingFlags.Public);
                if (property != null)
                    property.SetValue(toReturn, Convert.ChangeType(responseWithContent.Response.StatusCode, property.PropertyType));

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
            return ExecuteAsync().Result;
        }

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// The body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="NapConfig.AcceptFormat"/>.
        /// </returns>
        public T Execute<T>()
        {
            return ExecuteAsync<T>().Result;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The content and response.</returns>
        private async Task<InternalResponse> RunRequest()
        {
            using (var client = CreateClient())
            {
                var content = new StringContent(_content);
                var url = CreateUrl();
                foreach (var header in _headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

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
            if (_config.Advanced.Proxy != null)
            {
                handler.Proxy = new WebProxy(_config.Advanced.Proxy);
                handler.UseProxy = true;
            }

            var client = new HttpClient(handler);
            return client;
        }

        /// <summary>
        /// Creates the URL from an (optional) <see cref="NapConfig.BaseUrl"/>, URL and query parameters.
        /// </summary>
        /// <returns>The fully formed URL.</returns>
        private string CreateUrl()
        {
            var urlTemp = _url;
            if (!_url.StartsWith("http") && !string.IsNullOrEmpty(_config.BaseUrl))
                urlTemp = new Uri(new Uri(_config.BaseUrl), urlTemp).ToString();
            if (_queryParameters.Any())
            {
                urlTemp = _url.Contains("?") ?
                    (_url.EndsWith("?") ? _url : _url + "&") :
                    _url + "?";
                urlTemp += string.Join("&", _queryParameters.Select(x => string.Format("{0}={1}", x.Key, WebUtility.UrlEncode(x.Value))));
            }

            return urlTemp;
        }

        #endregion

        /// <summary>
        /// An internal class to house response and content information.
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