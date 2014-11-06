using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Napper.Base;
using Napper.Configuration;
using Napper.Formatters;

namespace Napper
{
    /// <summary>
    /// The Easy REST client allows for simple requests to be made and configured.
    /// </summary>
    public class Nap
    {
        private static Nap _instance;
        private readonly static object _padlock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap"/> class.
        /// </summary>
        public Nap()
        {
            Config = (NapConfig)System.Configuration.ConfigurationManager.GetSection("Nap") ?? new NapConfig();
            Config.Serializers.Add(RequestFormat.Json, new NapJsonSerializer());
            Config.Serializers.Add(RequestFormat.Xml, new NapXmlSerializer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL to use.</param>
        public Nap(string baseUrl) : this()
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
        public NapConfig Config { get; set; }

        /// <summary>
        /// Performs a GET request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the GET request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Get(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Get);
        }

        /// <summary>
        /// Performs a POST request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the POST request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Post(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Post);
        }

        /// <summary>
        /// Performs a DELETE request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the DELETE request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Delete(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Delete);
        }

        /// <summary>
        /// Performs a PUT request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the PUT request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Put(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Put);
        }
    }
}
