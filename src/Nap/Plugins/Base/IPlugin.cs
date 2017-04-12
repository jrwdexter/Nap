namespace Nap.Plugins.Base
{
    /// <summary>
    /// An interface for overriding the basic behavior of Nap.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Prepare a request for sending.
        /// </summary>
        /// <param name="request">The request to prepare.</param>
        /// <returns>A new, optionally modified request object.</returns>
        NapRequest Prepare(NapRequest request);

        /// <summary>
        /// Executes the <see cref="INapRequest"/>, and obtains content from HTTP.
        /// </summary>
        /// <param name="request">The request being executed.</param>
        /// <returns>An instance of an <see cref="object"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        object Execute(INapRequest request);

        /// <summary>
        /// Process the response after recieving it.
        /// </summary>
        /// <param name="response">The response to process.</param>
        /// <returns>A modified response if necessary.</returns>
        NapResponse Process(NapResponse response);
    }
}
