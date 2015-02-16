using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// A single header to use on each request.
    /// </summary>
    public interface IHeader
    {
        /// <summary>
        /// Gets or sets the key of the header.
        /// Example: "Content-Type" in "Content-Type: application/json"
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the header.
        /// Example: "application/json" in "Content-Type: application/json"
        /// </summary>
        string Value { get; set; }
    }
}