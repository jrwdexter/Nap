using System.Collections.Generic;
using Nap.Configuration.Sections.Base;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Represents a collection of headers that can be populated for use with each request.
    /// The class can be populated by *.config files.
    /// </summary>
    /// <example>
    /// Configuration can be implemented as:
    /// <c>
    /// &lt;Nap&gt;
    ///   &lt;Headers&gt;
    ///     &lt;add key="Content-Type" value="application/json" /&gt;
    ///   &lt;/Headers&gt;
    /// &lt;Nap&gt;
    /// </c>
    /// or by configuration on <see cref="NapClient.Config"/>.
    /// </example>
    public class Headers : NapConfigurationCollectionBase<Header, IHeader, string>, IHeaders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        public Headers() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        /// <param name="headers">The headers used to initialize the collection.</param>
        public Headers(IEnumerable<IHeader> headers) : base(headers) { }
    }
}