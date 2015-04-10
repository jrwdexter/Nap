using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation of the headers collection, which does not pull from *.config files.
    /// </summary>
    public class EmptyHeaders : List<IHeader>, IHeaders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyHeaders"/> class.
        /// </summary>
        public EmptyHeaders()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyHeaders"/> class.
        /// </summary>
        /// <param name="headers">The headers to initialize this collection with.</param>
        public EmptyHeaders(IEnumerable<IHeader> headers)
        {
            AddRange(headers);
        }

        /// <summary>
        /// Adds the specified header by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the header to add.</param>
        /// <param name="value">The value of the header to add.</param>
        public void Add(string key, string value)
        {
            Add(new EmptyHeader { Key = key, Value = value });
        }

        /// <summary>
        /// Remvoes the specified header by key.
        /// </summary>
        /// <param name="key">The key of the header to remove.</param>
        public void Remove(string key)
        {
            var itemToRemove = this.FirstOrDefault(h => h.Key == key);
            if (itemToRemove != null)
                Remove(itemToRemove);
        }

        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        public IDictionary<string, string> AsDictionary()
        {
            return this.ToDictionary(h => h.Key, h => h.Value);
        }
    }
}