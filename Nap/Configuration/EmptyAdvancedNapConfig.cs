using System;
using System.Net.Http;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation (does not use *.config files) of the <see cref="IAdvancedNapConfig"/> interface.
    /// </summary>
    public class EmptyAdvancedNapConfig : IAdvancedNapConfig
    {
        /// <summary>
        /// Gets or sets the overridable version of the construction of the <see cref="HttpClient"/> that handles Nap requests.
        /// </summary>
        public Func<INapRequest, HttpClient> ClientCreator { get; set; }

        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        public IProxyNapConfig Proxy { get; private set; } = new EmptyProxyNapConfig();

        /// <summary>
        /// Gets or sets the authentication configuration properties.
        /// </summary>
        public IAuthenticationNapConfig Authentication { get; private set; } = new EmptyAuthenticationNapConfig();

        /// <summary>
        /// Creates a clone of the current advanced configuration.
        /// </summary>
        /// <returns>A cloned version of the current advanced configuration</returns>
        public EmptyAdvancedNapConfig Clone()
        {
            return new EmptyAdvancedNapConfig
            {
                ClientCreator = ClientCreator,
                Authentication = ((EmptyAuthenticationNapConfig)Authentication).Clone(),
                Proxy = ((EmptyProxyNapConfig)Proxy).Clone()
            };
        }
    }
}
