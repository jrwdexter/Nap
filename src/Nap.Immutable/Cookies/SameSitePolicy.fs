namespace Nap
/// <summary>
/// Represents the strictness policy for same-site access.
/// </summary>
type SameSitePolicy =
    /// <summary>
    /// The site protection policy is unset.
    /// </summary>
    | Unset = 0x00

    /// <summary>
    /// Disallows using the cookie in cross-site requests.
    /// </summary>
    | Strict = 0x01

    /// <summary>
    /// Allow clients to send the cookie in cross-site requests.
    /// </summary>
    | Lax = 0x02
