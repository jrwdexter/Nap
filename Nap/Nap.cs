using System.Net.Http;
using Nap.Configuration;

namespace Nap
{
    /// <summary>
    /// Nap is the top level class for performing easy REST requests.
    /// Requests can be made using <code>new Nap();</code> or <code>Nap.Lets</code>.
    /// </summary>
    public class Nap
    {
        private static Nap _instance;
        private readonly static object _padlock = new object();
        private INapConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap"/> class.
        /// </summary>
        public Nap()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nap" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL to use.</param>
        public Nap(string baseUrl)
            : this()
        {
            Config.BaseUrl = baseUrl;
        }

        /// <summary>
        /// Gets a new instance of the <see cref="Nap"/>, which can be used to perform requests swiftly.
        /// </summary>
        public static Nap Lets
        {
            get
            {
                return new Nap();
                //if (_instance == null)
                //{
                //    lock (_padlock)
                //    {
                //        if (_instance == null)
                //            _instance = new Nap();
                //    }
                //}

                //return _instance;
            }
        }

        /// <summary>
        /// Gets or sets the configuration that is used as the base for all requests.
        /// </summary>
        public INapConfig Config
        {
            get
            {
                if (_config == null)
                {
                    lock (_padlock)
                    {
                        if (_config == null)
                            _config = NapSetup.Default;
                    }
                }

                return _config;
            }
        }

        /// <summary>
        /// Performs a GET request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the GET request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Get(string url)
        {
            Authenticate();
            // TODO: Do authentication (if not done yet) here
            return new NapRequest(Config.Clone(), url, HttpMethod.Get);
        }

        /// <summary>
        /// Performs a POST request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the POST request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Post(string url)
        {
            // TODO: Do authentication (if not done yet) here
            return new NapRequest(Config.Clone(), url, HttpMethod.Post);
        }

        /// <summary>
        /// Performs a DELETE request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the DELETE request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Delete(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Delete);
        }

        /// <summary>
        /// Performs a PUT request against the <see cref="url"/>.
        /// </summary>
        /// <param name="url">The URL to perform the PUT request against.</param>
        /// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
        public INapRequest Put(string url)
        {
            return new NapRequest(Config.Clone(), url, HttpMethod.Put);
        }

        private void Authenticate()
        {
            var authentication = Config.Advanced.Authentication;
            if(authentication.AuthenticationType > AuthenticationTypeEnum.Basic) // Then we're doing some sort of federated authentication/authorization
                throw new System.NotImplementedException();
        }
    }
}
