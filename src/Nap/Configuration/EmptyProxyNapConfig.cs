using System;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation (does not use *.config files) of the <see cref="IProxyNapConfig"/> interface.
    /// </summary>
    public class EmptyProxyNapConfig : IProxyNapConfig
    {
        /// <summary>
        /// Gets or sets the address of the proxy to use for Nap requests.
        /// </summary>
        public Uri Address { get; set; }

        /// <summary>
        /// Creates a clone of the current proxy configuration.
        /// </summary>
        /// <returns>A cloned version of the current proxy configuration</returns>
        public EmptyProxyNapConfig Clone()
        {
            return new EmptyProxyNapConfig
            {
                Address = Address != null ? new Uri(Address.OriginalString) : null
            };
        }
    }
}