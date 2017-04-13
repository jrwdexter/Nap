using Nap.Configuration;

namespace Nap.Plugins.Base
{
    /// <summary>
    /// An abstract class for overriding the basic behavior of Nap.
    /// </summary>
    public abstract class NapPluginBase : IPlugin
    {
        /// <summary>
        /// Setup a <see cref="NapClient"/> for initial use with a configuration.
        /// </summary>
        /// <param name="configuration">The configuration to permute and setup.</param>
        /// <returns>A new or modified client for generation of requests.</returns>
        public virtual INapConfig Configure(INapConfig configuration) => configuration;

        /// <summary>
        /// Prepare a request for sending.
        /// </summary>
        /// <param name="request">The request to prepare.</param>
        /// <returns>A new, optionally modified request object.</returns>
        public virtual NapRequest Prepare(NapRequest request) => request;

        /// <summary>
        /// Executes the <see cref="INapRequest"/>, and obtains content from HTTP.
        /// </summary>
        /// <param name="request">The request being executed.</param>
        /// <returns>An instance of an <see cref="object"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        public virtual T Execute<T>(INapRequest request) where T : class => null;

        /// <summary>
        /// Process the response after recieving it.
        /// </summary>
        /// <param name="response">The response to process.</param>
        /// <returns>A modified response if necessary.</returns>
        public virtual NapResponse Process(NapResponse response) => response;
    }
}
