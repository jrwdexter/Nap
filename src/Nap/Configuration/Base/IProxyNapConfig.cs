using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Proxy configuration propreties for Nap REST requests.
    /// </summary>
    public interface IProxyNapConfig
    {
        /// <summary>
        /// Gets or sets the address of the proxy to use for Nap requests.
        /// </summary>
        Uri Address { get; set; }
    }
}