using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// A single query parameter to use on a request.
    /// </summary>
    public interface IQueryParameter
    {
        /// <summary>
        /// Gets or sets the key for the query parameter.
        /// Example: "name" in ?name=John
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the query parameter.
        /// Example: "John" in ?name=John
        /// </summary>
        string Value { get; set; }
    }
}