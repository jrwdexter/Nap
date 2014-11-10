using System;
using System.Net;

namespace Napper.Proxies
{
    /// <summary>
    /// Creates a PCL-compliant instance of the <see cref="IWebProxy" /> interface.
    /// </summary>
    public class NapWebProxy : IWebProxy
    {
        private readonly Uri _requestUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="NapWebProxy"/> class.
        /// </summary>
        public NapWebProxy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapWebProxy"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        public NapWebProxy(Uri requestUri)
        {
            _requestUri = requestUri;
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns>Uri.</returns>
        public Uri GetProxy(Uri destination)
        {
            return _requestUri;
        }

        /// <summary>
        /// Indicates that the proxy should not be used for the specified host.
        /// </summary>
        /// <param name="host">The <see cref="T:System.Uri" /> of the host to check for proxy use.</param>
        /// <returns>true if the proxy server should not be used for <paramref name="host" />; otherwise, false.</returns>
        public bool IsBypassed(Uri host)
        {
            return false;
        }

        /// <summary>
        /// The credentials to submit to the proxy server for authentication.
        /// </summary>
        /// <value>The credentials.</value>
        /// <returns>An <see cref="T:System.Net.ICredentials" /> instance that contains the credentials that are needed to authenticate a request to the proxy server.</returns>
        public ICredentials Credentials { get; set; }
    }
}