namespace Nap
{
    /// <summary>
    /// Represents the strictness policy for same-site access.
    /// </summary>
    public enum SameSitePolicy
    {
        /// <summary>
        /// The site protection policy is unset.
        /// </summary>
        Unset,

        /// <summary>
        /// Disallows using the cookie in cross-site requests.
        /// </summary>
        Strict,

        /// <summary>
        /// Allow clients to send the cookie in cross-site requests.
        /// </summary>
        Lax
    }
}