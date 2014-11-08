using System;
using System.Collections.Generic;
using System.Linq;

namespace Napper
{
    /// <summary>
    /// An easily configurable request.
    /// </summary>
    internal partial class NapRequest : INapRemovableRequestComponent
    {
        /// <summary>
        /// Fills the response object with metadata using special keys, such as "StatusCode".
        /// If used after <see cref="DoNot"/>, instead removes the fill metadata flag.
        /// </summary>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest INapRemovableRequestComponent.FillMetadata()
        {
            return this.FillMetadata();
        }

        /// <summary>
        /// Excludes the body from the request.
        /// </summary>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeBody()
        {
            if (_doNot)
            {
                _content = null;
                _doNot = false;
            }

            return this;
        }

        /// <summary>
        /// Excludes the header with key <see cref="headerName"/>.
        /// </summary>
        /// <param name="headerName">The header name to be removed.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeHeader(string headerName)
        {
            if (_doNot)
            {
                if (_headers.Keys.Any(k => k == headerName))
                {
                    _headers.Remove(headerName);
                }

                _doNot = false;
            }

            return this;
        }

        /// <summary>
        /// Excludes the query with key <see cref="key"/>.
        /// </summary>
        /// <param name="key">The key of the query parameter to remove.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        public INapRequest IncludeQueryParameter(string key)
        {
            if (_doNot)
            {
                if (_queryParameters.Keys.Any(k => k == key))
                {
                    _queryParameters.Remove(key);
                }

                _doNot = false;
            }

            return this;
        }
    }
}