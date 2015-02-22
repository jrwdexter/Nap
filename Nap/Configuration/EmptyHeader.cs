using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Describes the empty version of the header, which does not pull data from *.config files.
    /// </summary>
    public class EmptyHeader : IHeader
    {
        /// <summary>
        /// Gets or sets the key of the header.
        /// Example: "Content-Type" in "Content-Type: application/json"
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the header.
        /// Example: "application/json" in "Content-Type: application/json"
        /// </summary>
        public string Value { get; set; }
    }
}