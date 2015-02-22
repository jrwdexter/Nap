namespace Nap.Configuration
{
    /// <summary>
    /// Credential configuration propreties for Nap REST requests.
    /// </summary>
    public interface IAuthenticationNapConfig
    {
        /// <summary>
        /// Gets or sets the type of the authentication to be used (eg. Basic, SAML, OAUth).
        /// </summary>
        AuthenticationTypeEnum AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the authentication configuration, which is required if <see cref="AuthenticationType"/> is set to anything except <see cref="AuthenticationTypeEnum.Basic"/>.
        /// </summary>
        IFederationConfiguration AuthenticationConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the username to use for authenticaiton.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use for authentication.
        /// </summary>
        string Password { get; set; }
    }
}