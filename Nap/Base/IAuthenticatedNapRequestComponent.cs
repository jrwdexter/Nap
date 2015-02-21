namespace Nap
{
    /// <summary>
    /// The partial component of the nap request that exposes authentication properties for further configuration.
    /// </summary>
    public interface IAuthenticatedNapRequestComponent
    {
        /// <summary>
        /// Configures the <see cref="NapRequest"/> for basic authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for basic authentication.</param>
        /// <param name="password">The plaintext password to use for basic authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        IAdvancedNapRequestComponent Basic(string username, string password);

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for SAML authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for SAML authentication.</param>
        /// <param name="password">The plaintext password to use for SAML authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        IAdvancedNapRequestComponent SAML(string username, string password);

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for OAuth1 authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for OAuth1 authentication.</param>
        /// <param name="password">The plaintext password to use for OAuth1 authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        IAdvancedNapRequestComponent OAuth1(string username, string password);

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for OAuth2 authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for OAuth2 authentication.</param>
        /// <param name="password">The plaintext password to use for OAuth2 authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        IAdvancedNapRequestComponent OAuth2(string username, string password);
    }
}