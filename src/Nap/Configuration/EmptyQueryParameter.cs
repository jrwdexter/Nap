using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Describes the empty version of the query parameter, which does not pull data from *.config files.
    /// </summary>
    public class EmptyQueryParameter : IQueryParameter
    {
        /// <summary>
        /// Gets or sets the key for the query parameter.
        /// Example: "name" in ?name=John
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the query parameter.
        /// Example: "John" in ?name=John
        /// </summary>
        public string Value { get; set; }
    }
}