using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using EasyHttp.Base;
using EasyHttp.Configuration;
using EasyHttp.Formatters;

namespace EasyHttp
{
    /// <summary>
    /// The Easy REST client allows for simple requests to be made and configured.
    /// </summary>
    public class EasyHttpClient
    {
        /// <summary>
        /// Gets or sets the configuration that is used as the base for all requests.
        /// </summary>
        public EasyConfig Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyHttpClient"/> class.
        /// </summary>
        public EasyHttpClient()
        {
            Config = new EasyConfig();
            Config.Serializers.Add(RequestFormat.Json, new EasyJsonSerializer());
            Config.Serializers.Add(RequestFormat.Xml, new EasyXmlSerializer());
        }

        /// <summary>
        /// Performs a GET request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the GET request against.</param>
        /// <returns>The configurable request object.  Run <see cref="IEasyHttpRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public IEasyHttpRequest Get(string url)
        {
            return new EasyHttpRequest(Config.Clone(), url, HttpMethod.Get);
        }

        /// <summary>
        /// Performs a POST request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the POST request against.</param>
        /// <returns>The configurable request object.  Run <see cref="IEasyHttpRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public IEasyHttpRequest Post(string url)
        {
            return new EasyHttpRequest(Config.Clone(), url, HttpMethod.Post);
        }

        /// <summary>
        /// Performs a DELETE request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the DELETE request against.</param>
        /// <returns>The configurable request object.  Run <see cref="IEasyHttpRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public IEasyHttpRequest Delete(string url)
        {
            return new EasyHttpRequest(Config.Clone(), url, HttpMethod.Delete);
        }

        /// <summary>
        /// Performs a PUT request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the PUT request against.</param>
        /// <returns>The configurable request object.  Run <see cref="IEasyHttpRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public IEasyHttpRequest Put(string url)
        {
            return new EasyHttpRequest(Config.Clone(), url, HttpMethod.Put);
        }
    }
}
