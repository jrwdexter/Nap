using System;
using System.Reflection;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Nap.Configuration;
using Nap.Exceptions;
using Nap.Plugins.Base;
using Nap.Proxies;
using Nap.Serializers.Base;
using System.Net.Http.Headers;

namespace Nap
{
    /// <summary>
    /// An easily configurable request.
    /// </summary>
    public partial class NapRequest : INapRequest
    {
        private readonly IReadOnlyCollection<IPlugin> _plugins;
        private readonly INapConfig _config;
        private readonly string _url;
        private readonly HttpMethod _method;
        private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly List<Tuple<Uri, Cookie>> _cookies = new List<Tuple<Uri, Cookie>>();
        private readonly Dictionary<EventCode, List<Event>> _events;
        private string _content;
        private bool _doNot;

        /// <summary>
        /// Initializes a new instance of the <see cref="NapRequest" /> class.
        /// </summary>
        /// <param name="plugins">The set of plugins to apply during execution of this request.</param>
        /// <param name="initialConfiguration">The initial configuration for the request.</param>
        /// <param name="url">The URL to perform the request against.  Can be a full URL or a partial.</param>
        /// <param name="method">The method to use in the request.</param>
        internal NapRequest(IReadOnlyCollection<IPlugin> plugins, INapConfig initialConfiguration, string url, HttpMethod method)
        {
            // Set up initial configuration parameters.
            _plugins = plugins;
            _config = initialConfiguration;
            _events = new Dictionary<EventCode, List<Event>>();
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
        /// Gets the URL of the request.
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
            try
            {
                _content = _config.Serializers.AsDictionary()[_config.Serialization].Serialize(body);
            }
            catch (Exception ex)
            {
                throw new NapSerializationException($"Serialization failed for data:\n {body}", ex);
            }

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
            var napPluginResult = _plugins.Aggregate<IPlugin, string>(null, (current, plugin) => current ?? plugin.Execute<string>(this) as string);
            if (napPluginResult != null)
                return napPluginResult;
            return (await RunRequestAsync()).Body;
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
            var napPluginResult = _plugins.Aggregate<IPlugin, T>(null, (current, plugin) => current ?? plugin.Execute<T>(this) as T);
            if (napPluginResult != null)
                return napPluginResult;
            var response = await RunRequestAsync();
            var toReturn = GetSerializer(response.ContentHeaders.ContentType.MediaType).Deserialize<T>(response.Body) ?? new T();

            if (_config.FillMetadata)
            {
                var property = typeof(T).GetRuntimeProperty("StatusCode");
                property?.SetValue(toReturn, Convert.ChangeType(response.StatusCode, property.PropertyType));
                HydrateCookieProperties(response, toReturn);
                HydrateHeaderProperties(response, toReturn);

                // TODO: Populate items with defaults (headers)
            }

            return toReturn;
        }

        /// <summary>
        /// Execute the request and retrieve a pre-built response type.
        /// The pre-built <see cref="NapResponse"/> contains many commonly utilized properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> that when awaited produces a <see cref="NapResponse"/> that equates to the server's response.</returns>
        public async Task<NapResponse> ExecuteRawAsync()
        {
            var napPluginResult = _plugins.Aggregate<IPlugin, NapResponse>(null, (current, plugin) => current ?? plugin.Execute<NapResponse>(this) as NapResponse);
            if (napPluginResult != null)
                return napPluginResult;
            return await RunRequestAsync();
        }

        /// <summary>
        /// Execute the request and retrieve a pre-built response type.
        /// The pre-built <see cref="NapResponse"/> contains many commonly utilized properties.
        /// </summary>
        /// <returns>A <see cref="NapResponse"/> that equates to the server's response.</returns>
        public NapResponse ExecuteRaw()
        {
            return Task.Run(ExecuteRawAsync).Result;
        }

        /// <summary>
        /// Runs the request using a threadpool thread, and blocks the current thread until completion.
        /// </summary>
        /// <returns>The response body content.</returns>
        public string Execute()
        {
            return Task.Run(ExecuteAsync).Result;
        }

        /// <summary>
        /// Runs the request using a threadpool thread, and blocks the current thread until completion.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// The body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="INapConfig.Serializers"/>.
        /// </returns>
        public T Execute<T>() where T : class, new()
        {
            return Task.Run(ExecuteAsync<T>).Result;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The content and response.</returns>
        private async Task<NapResponse> RunRequestAsync(bool skipPreparePlugins = false)
        {
            // If plugins have not been run, run those and then run the request.
            if (!skipPreparePlugins)
                return await _plugins.Aggregate(this, (request, plugin) => plugin.Prepare(request)).RunRequestAsync(true);

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
                    return null;

                using (var ms = new MemoryStream())
                using (var sr = new StreamReader(ms))
                {
                    await response.Content.CopyToAsync(ms);
                    ms.Position = 0;
                    var responseContent = await sr.ReadToEndAsync();
                    return _plugins.Aggregate(new NapResponse(this, response, responseContent), (r, plugin) => plugin.Process(r));
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

        private static T HydrateHeaderProperties<T>(NapResponse response, T toReturn) where T : class, new()
        {
            PropertyInfo property = typeof(T).GetRuntimeProperty("Headers");

            // Types of header properties set:
            // Array-type string
            // Dictionary-type <string, string>
            // Specific named string prop
            // Specific named Convert.ChangeType prop
            if ((property?.PropertyType?.IsConstructedGenericType ?? false) || (property?.PropertyType?.IsArray ?? false))
            {
                Type headerType;
                // If the property 'Headers' exists and is generic type, let's find the generic parameter and set it to an array
                if (property.PropertyType.IsConstructedGenericType)
                {
                    var isDictionaryType = property.PropertyType.GenericTypeArguments.Count() > 1;
                    headerType = property.PropertyType.GenericTypeArguments.First();
                    if ((headerType.IsConstructedGenericType && headerType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)) || isDictionaryType)
                    {
                        property.SetValue(toReturn, response.Headers.ToDictionary(c => c.Key, c => c.Value));
                    }
                    else if (headerType == typeof(string))
                        property.SetValue(toReturn, response.Headers.Select(c => $"{c.Key}: {c.Value}").ToArray());
                }
                else
                {
                    headerType = property.PropertyType.GetElementType();
                    if (headerType == typeof(string))
                        property.SetValue(toReturn, response.Cookies.Select(c => $"{c.Name}: {c.Value}").ToArray());
                }
            }
            else if (property != null)
            {
                // TODO: Log
            }

            // Set individual properties
            foreach (var h in response.Headers)
            {
                var p = typeof(T).GetRuntimeProperty(h.Key.Replace("-", string.Empty));
                if (p != null)
                {
                    if (p.PropertyType == typeof(string))
                        p.SetValue(toReturn, h.Value);
                    else
                    {
                        try
                        {
                            p.SetValue(toReturn, Convert.ChangeType(h.Value, p.PropertyType));
                        }
                        catch (InvalidCastException e)
                        {
                            // TODO: Log
                        }
                    }
                }
            }

            return toReturn;
        }

        private static T HydrateCookieProperties<T>(NapResponse response, T toReturn) where T : class, new()
        {
            PropertyInfo property = typeof(T).GetRuntimeProperty("Cookies");

            // Types of cookie properties set:
            // Array-type NapCookie
            // Array-type Cookie
            // Dictionary-type <string, string>
            // Array-type string
            // Specific named NapCookie prop
            // Specific named Cookie prop
            // Specific named string prop
            // Specific named Convert.ChangeType prop
            if ((property?.PropertyType?.IsConstructedGenericType ?? false) || (property?.PropertyType?.IsArray ?? false))
            {
                Type cookieType;
                // If the property 'Cookies' exists and is generic type, let's find the generic parameter and set it to an array
                if (property.PropertyType.IsConstructedGenericType)
                {
                    var isDictionaryType = property.PropertyType.GenericTypeArguments.Count() > 1;
                    cookieType = property.PropertyType.GenericTypeArguments.First();
                    if ((cookieType.IsConstructedGenericType && cookieType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)) || isDictionaryType)
                    {
                        property.SetValue(toReturn, response.Cookies.ToDictionary(c => c.Name, c => c.Value));
                    }
                    else if (cookieType == typeof(NapCookie))
                        property.SetValue(toReturn, response.Cookies.ToArray());
                    else if (cookieType == typeof(Cookie))
                        property.SetValue(toReturn, response.Cookies.Select(c => new Cookie(c.Name, c.Value, c.Metadata.Path, c.Metadata.Domain)).ToArray());
                    else if (cookieType == typeof(string))
                        property.SetValue(toReturn, response.Cookies.Select(c => $"{c.Name}: {c.Value}").ToArray());
                }
                else
                {
                    cookieType = property.PropertyType.GetElementType();
                    if (cookieType == typeof(NapCookie))
                        property.SetValue(toReturn, response.Cookies.ToArray());
                    else if (cookieType == typeof(Cookie))
                        property.SetValue(toReturn, response.Cookies.Select(c => new Cookie(c.Name, c.Value, c.Metadata.Path, c.Metadata.Domain)).ToArray());
                    else if (cookieType == typeof(string))
                        property.SetValue(toReturn, response.Cookies.Select(c => $"{c.Name}: {c.Value}").ToArray());
                }
            }
            else if (property != null)
            {
                // TODO: Log
            }

            // Set individual properties
            foreach (var c in response.Cookies)
            {
                var p = typeof(T).GetRuntimeProperty(c.Name.Replace("-", string.Empty));
                if (p != null)
                {
                    if (p.PropertyType == typeof(NapCookie))
                        p.SetValue(toReturn, c);
                    else if (p.PropertyType == typeof(Cookie))
                        p.SetValue(toReturn, new Cookie(c.Name, c.Value, c.Metadata.Path, c.Metadata.Domain));
                    else if (p.PropertyType == typeof(string))
                        p.SetValue(toReturn, c.Value);
                    else
                    {
                        try
                        {
                            p.SetValue(toReturn, Convert.ChangeType(c.Value, p.PropertyType));
                        }
                        catch (InvalidCastException e)
                        {
                            // TODO: Log
                        }
                    }
                }
            }

            return toReturn;
        }


        private void RunBooleanPluginMethod(Func<IPlugin, bool> pluginMethod, string errorMessage)
        {
            try
            {
                if (!_plugins.Aggregate(true, (current, plugin) => current && pluginMethod.Invoke(plugin)))
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

    /// <summary>
    /// A class that describes a specific type of object that can be marked 'complete'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Finalizable<T>
    {
        /// <summary>
        /// Gets the state of the finalizable item - true if the item should not continue down the pipeline, false.
        /// </summary>
        public bool IsFinal { get; }

        /// <summary>
        /// The item which is being processed by some pipeline.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// The default constructor for a Finalizable type 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isFinal"></param>
        public Finalizable(T item, bool isFinal)
        {
            Item = item;
            IsFinal = isFinal;
        }

        /// <summary>
        /// Create a new <see cref="Finalizable{T}"/> type with a status of final marked.
        /// </summary>
        /// <param name="item">The item to wrap in a finalizable state.</param>
        /// <returns>The new item that was created.</returns>
        public static Finalizable<T> Final(T item) => new Finalizable<T>(item, true);

        /// <summary>
        /// Create a new <see cref="Finalizable{T}"/> type with a status of continuing marked.
        /// </summary>
        /// <param name="item">The item to wrap in a finalizable state.</param>
        /// <returns>The new item that was created.</returns>
        public static Finalizable<T> Continuing(T item) => new Finalizable<T>(item, false);
    }

    /// <summary>
    /// An event that occurs during NapRequest pipline.
    /// </summary>
    /// <param name="napRequest">The request that is causing the event.</param>
    /// <returns>A new modified <see cref="NapRequest"/> to continue.</returns>
    public delegate INapRequest Event(INapRequest napRequest);

    /// <summary>
    /// Event codes for events that can happen to a response.
    /// </summary>
    public enum EventCode
    {
        /// <summary>
        /// An event that is executed before a the configuration is applied to the request.
        /// </summary>
        BeforeConfigurationApplied = 0x01,

        /// <summary>
        /// An event that is executed immediately following configuration application to a request.
        /// </summary>
        AfterConfigurationApplied = 0x02,

        /// <summary>
        /// An event that is executed immediately before a request is sent off.
        /// </summary>
        BeforeRequestExecution = 0x04,

        /// <summary>
        /// An event that is executed immediately after a request is sent off (after it returns, successfully or not).
        /// </summary>
        AfterRequestExecution = 0x08,

        /// <summary>
        /// An event that is executed immediately before respone deserialization is slated to happen. Will not occur if deserialization is not set to occur.
        /// </summary>
        BeforeResponseDeserialization = 0x10,

        /// <summary>
        /// An event that is executed immediately after response deserialization happens. Will not occur if no deserialization occurred.
        /// </summary>
        AfterResponseDeserialization = 0x20,

        /// <summary>
        /// An event that is executed immediately prior to a request being serialized by any serializer.
        /// </summary>
        BeforeRequestSerialization = 0x40,

        /// <summary>
        /// An event that is executed immediately prior to a request being serialized by any serializer.
        /// </summary>
        AfterRequestSerialization = 0x80,

        /// <summary>
        /// An event that is executed immediately before creating a nap request.
        /// </summary>
        CreatingNapRequest = 0x100,

        /// <summary>
        /// An event that is executed before authentication is set.
        /// </summary>
        SetAuthentication = 0x200,

        /// <summary>
        /// A meta-event describing the collection of all events.
        /// </summary>
        All = 0x3FF
    }

}