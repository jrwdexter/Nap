using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// An implementation of <see cref="IAdvancedNapConfig"/> that allows for use of *.config files.
    /// </summary>
    public class AuthenticationNapConfig : ConfigurationElement, IAuthenticationNapConfig
    {
        /// <summary>
        /// Gets or sets the type of the authentication to be used (eg. Basic, SAML, OAUth).
        /// </summary>
        [ConfigurationProperty("type", DefaultValue = AuthenticationTypeEnum.None, IsRequired = false)]
        public AuthenticationTypeEnum AuthenticationType
        {
            get { return (AuthenticationTypeEnum)this["type"]; }
            set { this["type"] = value; }
        }

        public IFederationConfiguration AuthenticationConfiguration { get; set; } // TODO: Figure out how to do authentication in configuration

        /// <summary>
        /// Gets or sets the username to use for authenticaiton.
        /// </summary>
        [ConfigurationProperty("username", DefaultValue = null, IsRequired = false)]
        public string Username
        {
            get { return (string)this["username"]; }
            set { this["username"] = value; }
        }

        /// <summary>
        /// Gets or sets the password to use for authentication.
        /// </summary>
        [ConfigurationProperty("password", DefaultValue = null, IsRequired = false)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
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
        /// Creates a copy of the <see cref="AuthenticationNapConfig"/> configuration.
        /// </summary>
        /// <returns>A copy of the current instance.</returns>
        public AuthenticationNapConfig Clone()
        {
            return new AuthenticationNapConfig { AuthenticationType = AuthenticationType, Username = Username, Password = Password };
        }
    }
}