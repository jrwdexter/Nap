using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Nap.Configuration.Sections;

using Nap.Formatters;
using Nap.Formatters.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// The configuration class for clients and requests.
	/// </summary>
	public class NapConfig : ConfigurationSection, INapConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NapConfig"/> class.
		/// </summary>
		public NapConfig()
		{
			Formatters = new FormattersConfig();
			Headers = new Headers();
			QueryParameters = new QueryParameters();
		}

		/// <summary>
		/// Gets or sets the formatters that can be used to both serialize and deserialize content.
		/// </summary>
		[ConfigurationProperty("formatters", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(FormattersConfig), AddItemName = "add", RemoveItemName = "remove", ClearItemsName = "clear")]
		public FormattersConfig Formatters
		{
			get { return (FormattersConfig)this["formatters"]; }
			private set { this["formatters"] = value; }
		}

		/// <summary>
		/// Gets or sets the formatters that can be used to both serialize and deserialize content.
		/// </summary>
		IFormattersConfig INapConfig.Formatters => Formatters;

		/// <summary>
		/// Gets or sets the optional base URL for easy requests.
		/// </summary>
		[ConfigurationProperty("baseUrl", DefaultValue = null, IsRequired = false)]
		public string BaseUrl
		{
			get { return (string)this["baseUrl"]; }
			set { this["baseUrl"] = value; }
		}

		/// <summary>
		/// Gets or sets the headers to use as a starting point to configure each request.
		/// </summary>
		[ConfigurationProperty("headers", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(Headers), AddItemName = "add", RemoveItemName = "remove", ClearItemsName = "clear")]
		public Headers Headers
		{
			get { return (Headers)this["headers"]; }
			private set { this["headers"] = value; }
		}

		/// <summary>
		/// Gets or sets the headers to use as a starting point to configure each request.
		/// </summary>
		IHeaders INapConfig.Headers => Headers;

		/// <summary>
		/// Gets or sets the query parameters to use as a starting point to configure each request.
		/// </summary>
		[ConfigurationProperty("queryParameters", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(QueryParameter), AddItemName = "add", RemoveItemName = "remove", ClearItemsName = "clear")]
		public QueryParameters QueryParameters
		{
			get { return (QueryParameters)this["queryParameters"]; }
			private set { this["queryParameters"] = value; }
		}

		/// <summary>
		/// Gets or sets the query parameters to use as a starting point to configure each request.
		/// </summary>
		IQueryParameters INapConfig.QueryParameters => QueryParameters;

		/// <summary>
		/// Gets or sets the content-type format.
		/// </summary>
		[ConfigurationProperty("serialization", DefaultValue = RequestFormat.Json, IsRequired = false)]
		public string Serialization
		{
			get { return this["serialization"] as string; }
			set { this["serialization"] = value; }
		}

		/// <summary>
		/// Gets or sets the advanced portion of the configuration.
		/// </summary>
		[ConfigurationProperty("advanced", DefaultValue = null, IsRequired = false)]
		public AdvancedNapConfig Advanced
		{
			get
			{
				var advancedConfig = this["advanced"] as AdvancedNapConfig;
				if (advancedConfig == null)
				{
					advancedConfig = new AdvancedNapConfig();
					this["advanced"] = advancedConfig;
				}

				return advancedConfig;
			}
			set { this["advanced"] = value; }
		}

		/// <summary>
		/// Gets or sets the advanced portion of the configuration.
		/// </summary>
		IAdvancedNapConfig INapConfig.Advanced => Advanced;

		/// <summary>
		/// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
		/// </summary>
		[ConfigurationProperty("fillMetadata", DefaultValue = true, IsRequired = false)]
		public bool FillMetadata
		{
			get { return (bool)this["fillMetadata"]; }
			set { this["fillMetadata"] = value; }
		}

		/// <summary>
		/// Creates a copy of the <see cref="NapConfig"/> configuration.
		/// </summary>
		/// <returns>A copy of the current instance.</returns>
		public INapConfig Clone()
		{
			var clone = new NapConfig
			{
				Formatters = new FormattersConfig(Formatters),
				BaseUrl = BaseUrl,
				FillMetadata = FillMetadata,
				Serialization = Serialization,
				Advanced = Advanced.Clone(),
				Headers = new Headers(Headers),
				QueryParameters = new QueryParameters(QueryParameters)
			};

			return clone;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Configuration.ConfigurationElement"/> object is read-only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}

		/// <summary>
		/// Gets the current nap configuration from the *.config file.
		/// </summary>
		/// <returns>A new NapConfig object populated with data from a *.config file, if possible.</returns>
		public static NapConfig GetCurrent()
		{
			return ConfigurationManager.GetSection("nap") as NapConfig ?? new NapConfig();
		}
	}
}
