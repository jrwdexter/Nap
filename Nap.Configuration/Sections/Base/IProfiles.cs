using System.Collections.Generic;

namespace Nap.Configuration.Sections.Base
{
    /// <summary>
    /// A configuration collection for defining "profiles" of nap configurations, allowing for easy application of settings.
    /// </summary>
    public interface IProfiles : ICollection<IProfile>
    {
        /// <summary>
        /// Adds the specified configuration by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the header to add.</param>
        /// <param name="value">The configuration to add, associated with the key</param>
        void Add(string key, ChildNapConfig value);

        /// <summary>
        /// Remvoes the specified configuration by key.
        /// </summary>
        /// <param name="key">The key of the configuration to remove.</param>
        void Remove(string key);

        /// <summary>
        /// Converts the <see cref="IProfiles"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>
        /// </summary>
        /// <returns>The <see cref="IProfiles"/> interface as a dictionary.</returns>
        IDictionary<string, ChildNapConfig> AsDictionary();
    }
}