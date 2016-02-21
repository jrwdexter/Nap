using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// An implementation of <see cref="IProxyNapConfig"/> that enables use of *.config files.
    /// </summary>
    public class ProxyNapConfig : ConfigurationElement, IProxyNapConfig
    {
        /// <summary>
        /// Gets or sets the address of the proxy to use for Nap requests.
        /// </summary>
        [ConfigurationProperty("address", DefaultValue = null, IsRequired = false)]
        public Uri Address
        {
            get { return (Uri)this["address"]; }
            set { this["address"] = value; }
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

        /// <summary>
        /// Creates a copy of the <see cref="ProxyNapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        internal ProxyNapConfig Clone()
        {
            var proxyNapConfig = new ProxyNapConfig { Address = new Uri(Address.OriginalString) };
            return proxyNapConfig;
        }
    }
}
