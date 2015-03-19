using System.Linq;
using System.Net.Http;
using Nap.Configuration;
using Nap.Plugins.Base;
using System;
using Nap.Exceptions;

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
							_config = NapSetup.Plugins.Aggregate<IPlugin, INapConfig>(null, (current, plugin) => current ?? plugin.GetConfiguration())
                                      ?? NapSetup.Default;
					}
				}

				return _config;
			}
		}

		/// <summary>
		/// Performs a GET request against the <paramref name="url"/>.
		/// </summary>
		/// <param name="url">The URL to perform the GET request against.</param>
		/// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
		public INapRequest Get(string url)
		{
			Authenticate();
			ApplyBeforeRequestPlugins();
			// TODO: Do authentication (if not done yet) here
			var request = CreateNapRequest(Config.Clone(), url, HttpMethod.Get);
			ApplyAfterRequestPlugins(request);
			return request;
		}


		/// <summary>
		/// Performs a POST request against the <paramref name="url"/>.
		/// </summary>
		/// <param name="url">The URL to perform the POST request against.</param>
		/// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
		public INapRequest Post(string url)
		{
			// TODO: Do authentication (if not done yet) here
			ApplyBeforeRequestPlugins();
			var request = CreateNapRequest(Config.Clone(), url, HttpMethod.Post);
			ApplyAfterRequestPlugins(request);
			return request;
		}

		/// <summary>
		/// Performs a DELETE request against the <paramref name="url"/>.
		/// </summary>
		/// <param name="url">The URL to perform the DELETE request against.</param>
		/// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
		public INapRequest Delete(string url)
		{
			ApplyBeforeRequestPlugins();
			var request = CreateNapRequest(Config.Clone(), url, HttpMethod.Delete);
			ApplyAfterRequestPlugins(request);
			return request;
		}

		/// <summary>
		/// Performs a PUT request against the <paramref name="url"/>.
		/// </summary>
		/// <param name="url">The URL to perform the PUT request against.</param>
		/// <returns>The configurable request object.  Run <see cref="INapRequest.ExecuteAsync{T}"/> or equivalent method.</returns>
		public INapRequest Put(string url)
		{
			ApplyBeforeRequestPlugins();
			var request = CreateNapRequest(Config.Clone(), url, HttpMethod.Put);
			ApplyAfterRequestPlugins(request);
			return request;
		}

		private void Authenticate()
		{
			var authentication = Config.Advanced.Authentication;
			if (authentication.AuthenticationType > AuthenticationTypeEnum.Basic) // Then we're doing some sort of federated authentication/authorization
				throw new System.NotImplementedException();
		}

		/// <summary>
		/// Helper method to run all plugin methods that need to be executed before <see cref="INapRequest"/> creation.
		/// </summary>
		private static void ApplyBeforeRequestPlugins()
		{
			try
			{
				if (!NapSetup.Plugins.Aggregate(true, (current, plugin) => current && plugin.BeforeNapRequestCreation()))
					throw new Exception("Nap pre-request creation aborted."); // TODO: Specific exception
			}
			catch (Exception e)
			{
				throw new Exception("Nap pre-request creation aborted.  See inner exception for details.", e); // TODO: Specific exception
			}
		}

		/// <summary>
		/// Helper method to run all plugin methods that need to be executed on <see cref="INapRequest"/> creation.
		/// </summary>
		private static INapRequest CreateNapRequest(INapConfig config, string url, HttpMethod method)
		{
			try
			{
				var request = NapSetup.Plugins.Aggregate<IPlugin, INapRequest>(null, (current, plugin) => current ?? plugin.GenerateNapRequest(config, url, method));
				request = request ?? new NapRequest(config, url, method);
				return request;
			}
			catch (Exception e)
			{
				throw new Exception("Nap request creation aborted.  See inner exception for details.", e); // TODO: Specific exception
			}
		}

		/// <summary>
		/// Helper method to run all plugin methods that need to be executed after <see cref="INapRequest"/> creation.
		/// </summary>
		private static void ApplyAfterRequestPlugins(INapRequest request)
		{
			try
			{
				if (!NapSetup.Plugins.Aggregate(true, (current, plugin) => current && plugin.AfterNapRequestCreation(request)))
					throw new NapPluginException("Nap post-request creation aborted.");
			}
			catch (Exception e)
			{
				throw new NapPluginException("Nap post-request creation aborted.  See inner exception for details.", e);
			}
		}
	}
}
