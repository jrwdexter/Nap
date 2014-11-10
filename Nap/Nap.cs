using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Napper.Configuration;
using Napper.Formatters;

namespace Napper
{
    /// <summary>
    /// Nap is the top level class for performing easy REST requests.
    /// Requests can be made using <code>new Nap();</code> or <code>Nap.Lets</code>.
    /// </summary>
    public class Nap
    {
        private static Nap _instance;
        private readonly static object _padlock = new object();
        private readonly INapConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap"/> class.
        /// </summary>
        public Nap()
        {
            _config = new EmptyNapConfig();
            //_config = ((INapConfig)System.Configuration.ConfigurationManager.GetSection("nap") ?? new NapConfig()).Clone();
            Config.Serializers.Add(RequestFormat.Json, new NapJsonSerializer());
            Config.Serializers.Add(RequestFormat.Xml, new NapXmlSerializer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL to use.</param>
        public Nap(string baseUrl)
            : this()
        {
            Config.BaseUrl = baseUrl;
        }

        /// <summary>
        /// Gets a single instance of the <see cref="Nap"/>, which can be used to perform requests swiftly.
        /// </summary>
        public static Nap Lets
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padlock)
                    {
                        if (_instance == null)
                            _instance = new Nap();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets or sets the configuration that is used as the base for all requests.
        /// </summary>
        public INapConfig Config
        {
            get { return _config; }
        }

        /// <summary>
        /// Performs a GET request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the GET request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Get(string url)
        {
            return new NapRequest(_config.Clone(), url, HttpMethod.Get);
        }

        /// <summary>
        /// Performs a POST request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the POST request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Post(string url)
        {
            return new NapRequest(_config.Clone(), url, HttpMethod.Post);
        }

        /// <summary>
        /// Performs a DELETE request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the DELETE request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Delete(string url)
        {
            return new NapRequest(_config.Clone(), url, HttpMethod.Delete);
        }

        /// <summary>
        /// Performs a PUT request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the PUT request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Put(string url)
        {
            return new NapRequest(_config.Clone(), url, HttpMethod.Put);
        }
    }
}
