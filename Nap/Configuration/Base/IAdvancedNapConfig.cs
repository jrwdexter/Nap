using System;
using System.Net.Http;

namespace Nap.Configuration
{
    /// <summary>
    /// Advanced configuration propreties for Nap REST requests.
    /// </summary>
    public interface IAdvancedNapConfig
    {
        /// <summary>
        /// Gets or sets the overridable version of the construction of the <see cref="HttpClient"/> that handles Nap requests.
        /// </summary>
        Func<INapRequest, HttpClient> ClientCreator { get; set; }

        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        IProxyNapConfig Proxy { get; }

        /// <summary>
        /// Gets or sets the authentication configuration properties.
        /// </summary>
        IAuthenticationNapConfig Authentication { get; }
    }
}