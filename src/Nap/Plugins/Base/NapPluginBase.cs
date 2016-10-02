using System.Net.Http;

using Nap.Configuration;

namespace Nap.Plugins.Base
{
    /// <summary>
    /// An abstract class for overriding the basic behavior of Nap.
    /// </summary>
    public abstract class NapPluginBase : IPlugin
    {
        /// <summary>
        /// Generate a <see cref="INapConfig"/> object for use with <see cref="INapRequest"/>s.
        /// If a non-null value is returned, execution of plugins and default behavior (<see cref="EmptyNapConfig"/>) is truncated, and the returned value is used.
        /// </summary>
        /// <returns>An instance of an <see cref="INapConfig"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        public virtual INapConfig GetConfiguration() => null;

        /// <summary>
        /// Method that is run before generation of a <see cref="INapRequest"/>.
        /// If <c>false</c> is returned, the <see cref="INapRequest"/> is truncated with an error.
        /// </summary>
        /// <returns><c>true</c> to continue <see cref="INapRequest"/> creation and subsequent execution; otherwise, <c>false</c>.</returns>
        public virtual bool BeforeNapRequestCreation() => true;

        /// <summary>
        /// Method that is run at <see cref="INapRequest"/> creation time (see <see cref="NapClient.Get(string)"/> for example) to create a new request object.
        /// If a non-null value is returned, execution of plugins and default behavior (<see cref="NapRequest"/>) is truncated, and the returned value is used.
        /// </summary>
        /// <param name="configuration">The initial configuration used to setup the <see cref="INapRequest"/>.</param>
        /// <param name="url">The URL that the <see cref="INapRequest"/> is being generated to target.</param>
        /// <param name="method">The HTTP method (GET/POST/PUT/etc) that the <see cref="INapRequest"/> is being targeted for.</param>
        /// <returns>An instance of an <see cref="INapRequest"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        public virtual INapRequest GenerateNapRequest(INapConfig configuration, string url, HttpMethod method) => null;

        /// <summary>
        /// Method to run a after <see cref="INapRequest"/> creation.  See <see cref="NapClient.Get(string)"/> for example.
        /// If <c>false</c> is returned, the <see cref="INapRequest"/> is truncated with an error.
        /// </summary>
        /// <returns><c>true</c> to continue <see cref="INapRequest"/> creation and subsequent execution; otherwise, <c>false</c>.</returns>
        public virtual bool AfterNapRequestCreation(INapRequest request) => true;

        /// <summary>
        /// Runs before a <see cref="INapRequest"/> serializes it's body content.
        /// If <c>false</c> is returned, the <see cref="INapRequest"/> is truncated with an error.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <returns><c>true</c> to continue <see cref="INapRequest"/> creation and subsequent execution; otherwise, <c>false</c>.</returns>
        public virtual bool BeforeRequestSerialization(INapRequest request) => true;

        /// <summary>
        /// Runs after a <see cref="INapRequest"/> serializes it's body content.
        /// If <c>false</c> is returned, the <see cref="INapRequest"/> is truncated with an error.
        /// </summary>
        /// <param name="request">The current request.</param>
        /// <returns><c>true</c> to continue <see cref="INapRequest"/> creation and subsequent execution; otherwise, <c>false</c>.</returns>
        public virtual bool AfterRequestSerialization(INapRequest request) => true;

        /// <summary>
        /// Executes the <see cref="INapRequest"/>, and obtains content from HTTP.
        /// </summary>
        /// <param name="request">The request being executed.</param>
        /// <returns>An instance of an <see cref="object"/> object if applicable; otherwise, null to use other plugin or default behavior.</returns>
        public virtual object Execute(INapRequest request) => null;
    }
}
