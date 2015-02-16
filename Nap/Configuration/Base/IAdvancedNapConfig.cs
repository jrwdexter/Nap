using System;

namespace Nap.Configuration
{
    /// <summary>
    /// Advanced configuration propreties for Nap REST requests.
    /// </summary>
    public interface IAdvancedNapConfig
    {
        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        IProxyNapConfig Proxy { get; }

        /// <summary>
        /// Gets or sets the authentication configuration properties.
        /// </summary>
        IAuthenticationNapConfig Authentication { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to use SSL during requests.
        /// </summary>
        bool UseSsl { get; set; }
    }
}