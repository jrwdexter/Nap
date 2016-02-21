namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation (does not use *.config files) of the <see cref="IAuthenticationNapConfig"/> interface.
    /// </summary>
    public class EmptyAuthenticationNapConfig : IAuthenticationNapConfig
    {
        /// <summary>
        /// Gets or sets the type of the authentication to be used (eg. Basic, SAML, OAUth).
        /// </summary>
        public AuthenticationTypeEnum AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration, which is required if <see cref="IAuthenticationNapConfig.AuthenticationType"/> is set to anything except <see cref="AuthenticationTypeEnum.Basic"/>.
        /// </summary>
        public IFederationConfiguration AuthenticationConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the username to use for authenticaiton.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use for authentication.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Creates a clone of the current authentication configuration.
        /// </summary>
        /// <returns>A cloned version of the current authentication configuration</returns>
        public EmptyAuthenticationNapConfig Clone()
        {
            return new EmptyAuthenticationNapConfig
            {
                AuthenticationType = AuthenticationType,
                AuthenticationConfiguration = AuthenticationConfiguration,
                Username = Username,
                Password = Password
            };
        }
    }
}