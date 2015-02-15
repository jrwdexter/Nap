using System.Threading.Tasks;

namespace Nap
{
    /// <summary>
    /// Represents a request which can be configured fluently.
    /// </summary>
    public interface INapRequest
    {
        /// <summary>
        /// Gets a set of methods to perform some removal of data from the request.
        /// </summary>
        INapRemovableRequestComponent DoNot { get; }

        /// <summary>
        /// Gets the advanced collection of methods for <see cref="INapRequest"/> configuration.
        /// </summary>
        IAdvancedNapRequestComponent Advanced { get; }

        /// <summary>
        /// Includes the query parameter in the value for the URL.
        /// </summary>
        /// <param name="key">The key for the query parameter to include.</param>
        /// <param name="value">The value of the query parameter to include.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest IncludeQueryParameter(string key, string value);

        /// <summary>
        /// Includes some content in the body, serialized according to <see cref="NapConfig.ContentFormat"/>.
        /// </summary>
        /// <param name="body">The object to serialize into the body.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest IncludeBody(object body);

        /// <summary>
        /// Includes a header in the request.
        /// </summary>
        /// <param name="headerName">Name of the header to include.</param>
        /// <param name="value">The value of the header to send.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest IncludeHeader(string headerName, string value);

        /// <summary>
        /// Includes a cookie in the request.
        /// </summary>
        /// <param name="uri">The URL the cookie corresponds to.</param>
        /// <param name="cookieName">The name of the cookie to include.</param>
        /// <param name="value">The value of the cookie to include.</param>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest IncludeCookie(string uri, string cookieName, string value);

        /// <summary>
        /// Fills the response object with metadata using special keys, such as "StatusCode".
        /// </summary>
        /// <returns>The <see cref="INapRequest"/> object.</returns>
        INapRequest FillMetadata();

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <returns>A task, that when run returns the body content.</returns>
        Task<string> ExecuteAsync();

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// A task, that when run returns the body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="NapConfig.AcceptFormat"/>.
        /// </returns>
        Task<T> ExecuteAsync<T>();

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <returns>The response body content.</returns>
        string Execute();

        /// <summary>
        /// Runs the request.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <returns>
        /// The body content deserialized to the object <typeparamref name="T"/>,
        /// using the serializer matching <see cref="NapConfig.AcceptFormat"/>.
        /// </returns>
        T Execute<T>();
    }
}