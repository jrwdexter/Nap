using System.Collections.Generic;
using System.Linq;

using Nap.Formatters.Base;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation of <see cref="INapConfig"/> with default values populated.
    /// </summary>
    public class EmptyNapConfig : INapConfig
    {
        /// <summary>
        /// Gets or sets the serializers that can be used to both serialize and deserialize content.
        /// </summary>
        /// <value>The serializers.</value>
        public Dictionary<RequestFormat, INapSerializer> Serializers { get; set; } = new Dictionary<RequestFormat, INapSerializer>();

        /// <summary>
        /// Gets or sets the optional base URL for easy requests.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the accept-type format.
        /// </summary>
        /// <value>The serialization.</value>
        public RequestFormat Serialization { get; set; }

        /// <summary>
        /// Gets or sets the advanced portion of the configuration.
        /// </summary>
        /// <value>The advanced.</value>
        public IAdvancedNapConfig Advanced { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
        /// </summary>
        /// <value><c>true</c> if [fill metadata]; otherwise, <c>false</c>.</value>
        public bool FillMetadata { get; set; }

        /// <summary>
        /// Returns a new instance of the current Nap configuration, with identical properties filled out.
        /// </summary>
        /// <returns>INapConfig.</returns>
        public INapConfig Clone()
        {
            var clone = new EmptyNapConfig
            {
                Serializers = Serializers.ToArray().ToDictionary(s => s.Key, s => s.Value),
                BaseUrl = BaseUrl,
                FillMetadata = FillMetadata,
                Serialization = Serialization,
                //Advanced = ((IAdvancedNapConfig)Advanced).Clone()
                // TODO: Clone advanced.
            };

            return clone;
        }
    }
}