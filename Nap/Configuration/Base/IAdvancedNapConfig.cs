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
        Uri Proxy { get; set; }
    }
}