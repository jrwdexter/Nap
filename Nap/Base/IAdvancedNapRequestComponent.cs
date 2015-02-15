using System;

namespace Nap
{
    /// <summary>
    /// The partial component of the nap request that exposes advanced properties for further configuration.
    /// </summary>
    /// <remarks>Advanced properties are placed within this module to prevent pollution of the <see cref="INapRequest"/> object space.</remarks>
    public interface IAdvancedNapRequestComponent
    {
        /// <summary>
        /// Gets a <see cref="IAuthenticatedNapRequestComponent"/> object, which exposes authentication properties for futher configuration of the nap request.
        /// </summary>
        /// <value>
        /// The <see cref="IAuthenticatedNapRequestComponent"/> object, which exposes authentication properties for futher configuration of the nap request.
        /// Note that this is simply the <see cref="NapRequest"/> object itself.
        /// </value>
        IAuthenticatedNapRequestComponent Authentication { get; }

        /// <summary>
        /// Add a proxy for use in the <see cref="NapRequest"/>.
        /// Traffic will be proxied through the <paramref name="uri"/> provided.
        /// </summary>
        /// <param name="uri">The URI to proxy traffic through.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> object on success, which can be further configured.</returns>
        IAdvancedNapRequestComponent UseProxy(Uri uri);
    }
}
