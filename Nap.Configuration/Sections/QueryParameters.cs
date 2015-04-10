using System.Collections.Generic;
using Nap.Configuration.Sections.Base;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Represents a collection of query parameters that can be populated for use with each request.
    /// The class can be populated by *.config files.
    /// </summary>
    /// <example>
    /// Configuration can be implemented as:
    /// <c>
    /// &lt;Nap&gt;
    ///   &lt;QueryParameters&gt;
    ///     &lt;add key="name" value="John" /&gt;
    ///   &lt;/QueryParameters&gt;
    /// &lt;Nap&gt;
    /// </c>
    /// or by configuration on <see cref="NapClient.Config"/>.
    /// </example>
    public class QueryParameters : NapConfigurationCollectionBase<QueryParameter, IQueryParameter, string>, IQueryParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters"/> class.
        /// </summary>
        public QueryParameters() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters"/> class.
        /// </summary>
        /// <param name="queryParameters">The query parameters used to initialize the collection.</param>
        public QueryParameters(IEnumerable<IQueryParameter> queryParameters) : base(queryParameters) { }
    }
}