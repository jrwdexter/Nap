using System;
using System.Configuration;
using System.Net.Http;

namespace Nap.Configuration.Sections
{
    public class AdvancedNapConfig : ConfigurationElement, IAdvancedNapConfig
    {
        /// <summary>
        /// Gets or sets the overridable version of the construction of the <see cref="System.Net.Http.HttpClient"/> that handles Nap requests.
        /// </summary>
        /// <remarks>Cannot be set via a configuration setting; must be set programatically.</remarks>
        public Func<INapRequest, HttpClient> ClientCreator { get; set; }

        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        [ConfigurationProperty("proxy", DefaultValue = null, IsRequired = false)]
        public ProxyNapConfig Proxy
        {
            get
            {
                var proxyConfig = this["proxy"] as ProxyNapConfig;
                if (proxyConfig == null)
                {
                    proxyConfig = new ProxyNapConfig();
                    this["proxy"] = proxyConfig;
                }

                return proxyConfig;
            }
            set { this["proxy"] = value; }
        }

        /// <summary>
        /// Gets or sets the optional proxy for requests.
        /// </summary>
        IProxyNapConfig IAdvancedNapConfig.Proxy => Proxy;

        /// <summary>
        /// Gets or sets the authentication configuration properties.
        /// </summary>
        [ConfigurationProperty("authentication", DefaultValue = null, IsRequired = false)]
        public AuthenticationNapConfig Authentication
        {
            get
            {
                var authenticationConfig = this["authentication"] as AuthenticationNapConfig;
                if (authenticationConfig == null)
                {
                    authenticationConfig = new AuthenticationNapConfig();
                    this["authentication"] = authenticationConfig;
                }

                return authenticationConfig;
            }
            set { this["authentication"] = value; }
        }

        /// <summary>
        /// Gets or sets the authentication configuration properties.
        /// </summary>
        IAuthenticationNapConfig IAdvancedNapConfig.Authentication => Authentication;

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
        /// Creates a copy of the <see cref="AdvancedNapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        internal AdvancedNapConfig Clone()
        {
            var advancedNapConfig = new AdvancedNapConfig
            {
                Authentication = Authentication.Clone(),
                ClientCreator = ClientCreator,
                Proxy = Proxy.Clone()
            };

            return advancedNapConfig;
        }
    }
}
