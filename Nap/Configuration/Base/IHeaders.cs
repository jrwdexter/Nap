using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Represents a collection of headers that can be populated for use with each request.
    /// </summary>
    public interface IHeaders : ICollection<IHeader>
    {
        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Clear()"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        IDictionary<string, string> AsDictionary();
    }
}