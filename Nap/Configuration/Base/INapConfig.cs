using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Configuration properties for Nap REST requests.
    /// </summary>
    public interface INapConfig
    {
        /// <summary>
        /// Gets or sets the formatters that can be used to both serialize and deserialize content.
        /// </summary>
		IFormattersConfig Formatters { get; }

        /// <summary>
        /// Gets or sets the optional base URL for easy requests.
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the headers to use as a starting point to configure each request.
        /// </summary>
        IHeaders Headers { get; }

        /// <summary>
        /// Gets or sets the query parameters to use as a starting point to configure each request.
        /// </summary>
        IQueryParameters QueryParameters { get; }

        /// <summary>
        /// Gets or sets the Content-Type format, and which formatter to use on serialization.
        /// Available options are at <see cref="RequestFormat"/>.
        /// </summary>
        string Serialization { get; set; }

        /// <summary>
        /// Gets or sets the advanced portion of the configuration.
        /// </summary>
        IAdvancedNapConfig Advanced { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
        /// </summary>
        bool FillMetadata { get; set; }

        /// <summary>
        /// Returns a new instance of the current Nap configuration
        /// </summary>
        /// <returns></returns>
        INapConfig Clone();
    }
}