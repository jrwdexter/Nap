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
    }
}