using System.Collections.Generic;

namespace Nap.Configuration
{
    /// <summary>
    /// A collection of query parameters that can be populated for use with each request.
    /// </summary>
    public interface IQueryParameters : ICollection<IQueryParameter>
    {
        /// <summary>
        /// Adds the specified query parameter by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the query parameter to add.</param>
        /// <param name="value">The value of the query parameter to add.</param>
        void Add(string key, string value);

        /// <summary>
        /// Remvoes the specified query parameter by key.
        /// </summary>
        /// <param name="key">The key of the query parameter to remove.</param>
        void Remove(string key);

        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        IDictionary<string, string> AsDictionary();
    }
}