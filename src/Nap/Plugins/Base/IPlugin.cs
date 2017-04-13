using Nap.Configuration;

namespace Nap.Plugins.Base
{
    /// <summary>
    /// An interface for overriding the basic behavior of Nap.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Setup a <see cref="NapClient"/> for initial use with a configuration.
        /// </summary>
        /// <param name="configuration">The configuration to permute and setup.</param>
        /// <returns>A new or modified client for generation of requests.</returns>
        INapConfig Configure(INapConfig configuration);

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
        T Execute<T>(INapRequest request) where T : class;

        /// <summary>
        /// Process the response after recieving it.
        /// </summary>
        /// <param name="response">The response to process.</param>
        /// <returns>A modified response if necessary.</returns>
        NapResponse Process(NapResponse response);
    }
}
