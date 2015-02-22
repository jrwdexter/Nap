namespace Nap.Configuration
{
    /// <summary>
    /// The enumeration describing the type of authentication to use.
    /// </summary>
    public enum AuthenticationTypeEnum
    {
        /// <summary>
        /// If selected, no authentication is performed.
        /// </summary>
        None,

        /// <summary>
        /// If selected, basic authentication is performed, adding a {"Authentication", "Basic [Base64 Username:Password]"} header.
        /// </summary>
        Basic,

        /// <summary>
        /// If selected, SAML (2.0) authentication is performed, requesting additional information from an identity provider.
        /// </summary>
        SAML2,

        /// <summary>
        /// If selected, OAuth authorization is performed, requesting an access token from an authorization provider (not identity).
        /// </summary>
        OAuth1,

        /// <summary>
        /// If selected, OAuth authorization is performed, requesting an access token from an authorization provider (not identity).
        /// </summary>
        OAuth2
    }
}