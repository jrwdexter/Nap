using System;
using System.Net;

namespace Napper.Proxies
{
    public class NapWebProxy : IWebProxy
    {
        private readonly Uri _requestUri;

        public NapWebProxy()
        {
        }

        public NapWebProxy(Uri requestUri)
        {
            _requestUri = requestUri;
        }

        public Uri GetProxy(Uri destination)
        {
            return _requestUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }

        public ICredentials Credentials { get; set; }
    }
}