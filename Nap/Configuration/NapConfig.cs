using System;
using System.Collections.Generic;
using System.Linq;

using Napper.Formatters.Base;

namespace Napper.Configuration
{
    /// <summary>
    /// The configuration class for clients and requests.
    /// </summary>
    public class NapConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfig"/> class.
        /// </summary>
        public NapConfig()
        {
            Serializers = new Dictionary<RequestFormat, INapSerializer>();
            AcceptFormat = RequestFormat.Json;
            ContentFormat = RequestFormat.Json;
            FillMetadata = false;
        }

        /// <summary>
        /// Gets or sets the serializers that can be used to both serialize and deserialize content.
        /// </summary>
        public Dictionary<RequestFormat, INapSerializer> Serializers { get; set; }

        /// <summary>
        /// Gets or sets the optional base URL for easy requests.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the accept-type format.
        /// </summary>
        public RequestFormat AcceptFormat { get; set; }

        /// <summary>
        /// Gets or sets the content-type format.
        /// </summary>
        public RequestFormat ContentFormat { get; set; }

        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        public Uri Proxy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
        /// </summary>
        public bool FillMetadata { get; set; }

        /// <summary>
        /// Creates a copy of the <see cref="NapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        internal NapConfig Clone()
        {
            var easyConfig = new NapConfig
            {
                Serializers = Serializers.ToArray().ToDictionary(s => s.Key, s => s.Value),
                BaseUrl = BaseUrl,
                AcceptFormat = AcceptFormat,
                ContentFormat = ContentFormat,
                Proxy = Proxy
            };

            return easyConfig;
        }
    }
}
