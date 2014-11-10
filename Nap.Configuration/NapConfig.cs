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
    public class NapConfig : ConfigurationSection, INapConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfig"/> class.
        /// </summary>
        public NapConfig()
        {
            Serializers = new Dictionary<RequestFormat, INapSerializer>();
            Serialization = RequestFormat.Json;
            FillMetadata = false;
        }

        /// <summary>
        /// Gets or sets the serializers that can be used to both serialize and deserialize content.
        /// </summary>
        public Dictionary<RequestFormat, INapSerializer> Serializers { get; set; }

        /// <summary>
        /// Gets or sets the optional base URL for easy requests.
        /// </summary>
        [ConfigurationProperty("baseUrl", DefaultValue = null, IsRequired = false)]
        public string BaseUrl
        {
            get { return (string)this["baseUrl"]; }
            set { this["baseUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the content-type format.
        /// </summary>
        [ConfigurationProperty("serialization", DefaultValue = RequestFormat.Json, IsRequired = false)]
        public RequestFormat Serialization
        {
            get { return (RequestFormat)this["serialization"]; }
            set { this["serialization"] = value; }
        }

        /// <summary>
        /// Gets or sets the advanced portion of the configuration.
        /// </summary>
        [ConfigurationProperty("advanced")]
        public IAdvancedNapConfig Advanced
        {
            get
            {
                var advancedConfig = (AdvancedNapConfig)this["advanced"];
                if (advancedConfig == null)
                {
                    advancedConfig = new AdvancedNapConfig();
                    this["advanced"] = advancedConfig;
                }

                return advancedConfig;
            }
            set { this["advanced"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
        /// </summary>
        [ConfigurationProperty("fillMetadata", DefaultValue = false, IsRequired = false)]
        public bool FillMetadata
        {
            get { return (bool)this["fillMetadata"]; }
            set { this["fillMetadata"] = value; }
        }

        /// <summary>
        /// Creates a copy of the <see cref="NapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public INapConfig Clone()
        {
            var easyConfig = new NapConfig
            {
                Serializers = Serializers.ToArray().ToDictionary(s => s.Key, s => s.Value),
                BaseUrl = BaseUrl,
                FillMetadata = FillMetadata,
                Serialization = Serialization,
                Advanced = ((AdvancedNapConfig)Advanced).Clone()
            };

            return easyConfig;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
