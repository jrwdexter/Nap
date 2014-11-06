using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Napper.Formatters.Base;

namespace Napper.Configuration
{
    /// <summary>
    /// The configuration class for clients and requests.
    /// </summary>
    public class NapConfig : ConfigurationSection
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
        [ConfigurationProperty("BaseUrl", DefaultValue = null, IsRequired = false)]
        public string BaseUrl
        {
            get { return (string)this["BaseUrl"]; }
            set { this["BaseUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the accept-type format.
        /// </summary>
        [ConfigurationProperty("AcceptFormat", DefaultValue = RequestFormat.Json, IsRequired = false)]
        public RequestFormat AcceptFormat 
        {
            get { return (RequestFormat)this["AcceptFormat"]; }
            set { this["AcceptFormat"] = value; }
        }

        /// <summary>
        /// Gets or sets the content-type format.
        /// </summary>
        [ConfigurationProperty("ContentFormat", DefaultValue = RequestFormat.Json, IsRequired = false)]
        public RequestFormat ContentFormat 
        {
            get { return (RequestFormat)this["ContentFormat"]; }
            set { this["ContentFormat"] = value; }
        }

        /// <summary>
        /// Gets or sets the advanced portion of the configuration.
        /// </summary>
        [ConfigurationProperty("Advanced")]
        public AdvancedNapConfig Advanced
        {
            get { return (AdvancedNapConfig)this["Advanced"]; }
            set { this["Advanced"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
        /// </summary>
        [ConfigurationProperty("FillMetadata", DefaultValue = false, IsRequired = false)]
        public bool FillMetadata 
        {
            get { return (bool)this["FillMetadata"]; }
            set { this["FillMetadata"] = value; }
        }

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
                Advanced = Advanced.Clone()
            };

            return easyConfig;
        }
    }
}
