using System;
using System.Collections.Generic;
using System.Net;

namespace Nap
{
    /// <summary>
    /// Metadata and flags associated with a cookie response.
    /// </summary>
    public struct NapCookieMetadata
    {
        /// <summary>
        /// Gets the moment at which this cookie was generated.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Gets the GMT moment at which the cookie expires.
        /// </summary>
        public DateTime? Expires { get; }

        /// <summary>
        /// Gets the max age that the cookie will be usable for, in seconds.
        /// </summary>
        public int? MaxAge { get; }

        /// <summary>
        /// Gets the domain that the cookie is valid for.
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// Gets the absolute path that the cookie is valid for.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets a flag that if true indicates that the cookie should only be used in HTTPS/SSL requests.
        /// </summary>
        public bool IsSecure { get; }

        /// <summary>
        /// Gets the cookie is set to not be accessible through JavaScript.
        /// </summary>
        public bool HttpOnly { get; }

        /// <summary>
        /// Gets the level of protection the cookie offers for cross-site access.
        /// </summary>
        /// <remarks>Currently experimental API component.</remarks>
        public SameSitePolicy SameSite { get; }

        /// <summary>
        /// Gets the URL that this cookie applies to.
        /// </summary>
        public Uri Url
        {
            get
            {
                var secureString = IsSecure ? "s" : string.Empty;
                return new Uri($"http{secureString}://{Domain.TrimEnd('/')}/{Path.TrimStart('/')}");
            }
        }

        /// <summary>
        /// A value indicating whether or not the cookie is still valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (MaxAge.HasValue)
                {
                    return DateTime.UtcNow < (CreationDate.AddSeconds(MaxAge.Value));
                }
                else if (Expires.HasValue)
                {
                    return DateTime.UtcNow < Expires.Value;
                }

                return true;
            }
        }

        internal NapCookieMetadata(Uri hostUri, IDictionary<string, string> directives)
        {
            CreationDate = DateTime.UtcNow;
            // Expires
            if (directives.ContainsKey("expires") && directives["expires"] != null)
                Expires = DateTime.Parse(directives["expires"]);
            else
                Expires = null;
            // Max-Age
            if (directives.ContainsKey("max-age"))
            {
                int maxAge;
                if (int.TryParse(directives["max-age"], out maxAge))
                    MaxAge = maxAge;
                else
                    MaxAge = null;
            }
            else
                MaxAge = null;
            // Domain
            if (directives.ContainsKey("domain"))
                Domain = directives["domain"] ?? hostUri.Host;
            else
                Domain = hostUri.Host;
            // Path
            if (directives.ContainsKey("path"))
                Path = directives["path"] ?? "/";
            else
                Path = "/";
            // Path
            IsSecure = directives.ContainsKey("secure");

            // HttpOnly
            HttpOnly = directives.ContainsKey("httponly");

            // SameSite
            if (directives.ContainsKey("samesite") && directives["samesite"] != null)
            {
                SameSitePolicy policy;
                if (Enum.TryParse(directives["samesite"], out policy))
                    SameSite = policy;
                else
                    SameSite = SameSitePolicy.Unset;
            }
            else
                SameSite = SameSitePolicy.Unset;
        }

        internal NapCookieMetadata(Cookie cookie)
        {
            CreationDate = cookie.TimeStamp;
            Expires = cookie.Expires;
            MaxAge = null;
            Domain = cookie.Domain;
            Path = cookie.Path;
            IsSecure = cookie.Secure;
            HttpOnly = cookie.HttpOnly;
            SameSite = SameSitePolicy.Unset;
        }
    }
}
