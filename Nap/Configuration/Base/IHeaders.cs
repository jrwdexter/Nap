using System.Collections.Generic;

namespace Nap.Configuration
{
    /// <summary>
    /// Represents a collection of headers that can be populated for use with each request.
    /// </summary>
    public interface IHeaders : ICollection<IHeader>
    {
        /// <summary>
        /// Adds the specified header by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the header to add.</param>
        /// <param name="value">The value of the header to add.</param>
        void Add(string key, string value);

        /// <summary>
        /// Remvoes the specified header by key.
        /// </summary>
        /// <param name="key">The key of the header to remove.</param>
        void Remove(string key);

        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Clear()"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        IDictionary<string, string> AsDictionary();
    }
}