using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nap
{
    /// <summary>
    /// A single cookie returned from the server.
    /// </summary>
    public struct NapCookie
    {
        /// <summary>
        /// Gets the name of the cookie.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the cookie.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets all metadata associated with this cookie.
        /// </summary>
        public NapCookieMetadata Metadata { get; }

        /// <summary>
        /// Initializes a new instance of a <see cref="NapCookie"/> class.
        /// </summary>
        /// <param name="requestUri">The request URI that this cookie belongs to.</param>
        /// <param name="cookieString">The string that will generate a cookie (value of a Set-Cookie header).</param>
        public NapCookie(Uri requestUri, string cookieString)
        {
            var segments = cookieString.Split(';').Select(c =>
            {
                var split = c.Split('=').Select(s => s?.Trim()).ToList();
                if (c.Length > 1)
                    return new KeyValuePair<string, string>(split[0], split[1]);
                else
                    return new KeyValuePair<string, string>(split[0], null);
            }).ToDictionary(kv => kv.Key, kv => kv.Value);
            if (segments.Count > 0)
            {
                Name = segments.First().Key;
                Value = segments.First().Value;
                Metadata = new NapCookieMetadata(requestUri, segments);
            }
            else
            {
                Name = null;
                Value = null;
                Metadata = new NapCookieMetadata();
            }
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="NapCookie"/> class from a .NET cookie.
        /// </summary>
        public NapCookie(Cookie cookie)
        {
            Name = cookie.Name;
            Value = cookie.Value;
            Metadata = new NapCookieMetadata(cookie);
        }
    }
}
