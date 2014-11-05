using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyHttp.Base
{
    /// <summary>
    /// An interface that exposes removal methods from the request.
    /// </summary>
    public interface IEasyRemovableRequestComponent
    {
        /// <summary>
        /// Does not fill the response object with metadata using special keys, such as "StatusCode".
        /// </summary>
        /// <returns>The <see cref="IEasyHttpRequest"/> object.</returns>
        IEasyHttpRequest FillMetadata();

        /// <summary>
        /// Excludes the body from the request.
        /// </summary>
        /// <returns>The <see cref="IEasyHttpRequest"/> object.</returns>
        IEasyHttpRequest IncludeBody();

        /// <summary>
        /// Excludes the header with key <see cref="headerName"/>.
        /// </summary>
        /// <param name="headerName">The header name to be removed.</param>
        /// <returns>The <see cref="IEasyHttpRequest"/> object.</returns>
        IEasyHttpRequest IncludeHeader(string headerName);

        /// <summary>
        /// Excludes the query with key <see cref="key"/>.
        /// </summary>
        /// <param name="key">The key of the query parameter to remove.</param>
        /// <returns>The <see cref="IEasyHttpRequest"/> object.</returns>
        IEasyHttpRequest IncludeQueryParameter(string key);
    }
}