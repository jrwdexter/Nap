using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
	/// <summary>
	/// An empty implementation of <see cref="INapConfig"/> with default values populated.
	/// </summary>
	public class EmptyNapConfig : INapConfig
	{
		/// <summary>
		/// Gets or sets the serializers that can be used to both serialize and deserialize content.
		/// </summary>
		/// <value>The serializers.</value>
		public IFormattersConfig Formatters { get; set; } = new EmptyFormattersConfig();

		/// <summary>
		/// Gets or sets the optional base URL for easy requests.
		/// </summary>
		/// <value>The base URL.</value>
		public string BaseUrl { get; set; }

		/// <summary>
		/// Gets or sets the headers to use as a starting point to configure each request.
		/// </summary>
		public IHeaders Headers { get; private set; } = new EmptyHeaders();

		/// <summary>
		/// Gets or sets the query parameters to use as a starting point to configure each request.
		/// </summary>
		public IQueryParameters QueryParameters { get; private set; } = new EmptyQueryParameters();

		/// <summary>
		/// Gets or sets the Content-Type format, and which serializer to use on serialization.
		/// Available options are at <see cref="RequestFormat"/>.
		/// </summary>
		public string Serialization { get; set; } = RequestFormat.Json;

		/// <summary>
		/// Gets or sets the advanced portion of the configuration.
		/// </summary>
		/// <value>The advanced.</value>
		public IAdvancedNapConfig Advanced { get; private set; } = new EmptyAdvancedNapConfig();

		/// <summary>
		/// Gets or sets a value indicating whether or not to fill "Special Values" such as StatusCode on the deserialized object.
		/// </summary>
		/// <value><c>true</c> if [fill metadata]; otherwise, <c>false</c>.</value>
		public bool FillMetadata { get; set; } = true;

		/// <summary>
		/// Returns a new instance of the current Nap configuration, with identical properties filled out.
		/// </summary>
		/// <returns>INapConfig.</returns>
		public INapConfig Clone()
		{
			var clone = new EmptyNapConfig
			{
				Formatters = new EmptyFormattersConfig(Formatters),
				BaseUrl = BaseUrl,
				FillMetadata = FillMetadata,
				Serialization = Serialization,
				QueryParameters = new EmptyQueryParameters(QueryParameters),
				Headers = new EmptyHeaders(Headers),
				Advanced = ((EmptyAdvancedNapConfig)Advanced).Clone()
			};

			return clone;
		}
	}
}