namespace Nap.Plugins.Base
{
    /// <summary>
    /// An abstract class for overriding the basic behavior of Nap.
    /// </summary>
    public abstract class NapPluginBase : IPlugin
    {
        /// <summary>
        /// Prepare a request for sending.
        /// </summary>
        /// <param name="request">The request to prepare.</param>
        /// <returns>A new, optionally modified request object.</returns>
        public NapRequest Prepare(NapRequest request) => request;

        /// <summary>
        /// Executes the <see cref="INapRequest"/>, and obtains content from HTTP.
        /// </summary>
        /// <param name="request">The request being executed.</param>
        /// <returns>An instance of an <see cref="object"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        public virtual object Execute(INapRequest request) => null;

        /// <summary>
        /// Process the response after recieving it.
        /// </summary>
        /// <param name="response">The response to process.</param>
        /// <returns>A modified response if necessary.</returns>
        public NapResponse Process(NapResponse response) => response;
    }
}
