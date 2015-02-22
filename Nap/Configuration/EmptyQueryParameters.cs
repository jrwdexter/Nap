using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation of the query parameters collection, which does not pull from *.config files.
    /// </summary>
    public class EmptyQueryParameters : List<IQueryParameter>, IQueryParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueryParameters"/> class.
        /// </summary>
        public EmptyQueryParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueryParameters"/> class.
        /// </summary>
        /// <param name="queryParameters">The headers to initialize this collection with.</param>
        public EmptyQueryParameters(IEnumerable<IQueryParameter> queryParameters)
        {
            AddRange(queryParameters);
        }

        /// <summary>
        /// Adds the specified query parameter by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the query parameter to add.</param>
        /// <param name="value">The value of the query parameter to add.</param>
        public void Add(string key, string value)
        {
            Add(new EmptyQueryParameter { Key = key, Value = value });
        }

        /// <summary>
        /// Remvoes the specified query parameter by key.
        /// </summary>
        /// <param name="key">The key of the query parameter to remove.</param>
        public void Remove(string key)
        {
            var itemToRemove = this.FirstOrDefault(h => h.Key == key);
            if (itemToRemove != null)
                Remove(itemToRemove);
        }

        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Clear()"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        public IDictionary<string, string> AsDictionary()
        {
            return this.ToDictionary(q => q.Key, q => q.Value);
        }
    }
}