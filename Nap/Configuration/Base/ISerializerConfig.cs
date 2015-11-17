using System;
using System.Collections.Generic;
using System.Linq;
using Nap.Serializers.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Represents a single serializer's configuration.
	/// </summary>
	public interface ISerializerConfig
	{
		/// <summary>
		/// Gets or sets the content type (MIME)that this serializer configuration is for.
		/// </summary>
		string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of the serializer.
		/// </summary>
		/// <example><c>Nap.Serializers.NapJsonSerializer</c></example>
		string SerializerType { get; set; }

		/// <summary>
		/// Gets the instance of the serializer corresponding to <see cref="SerializerType"/>.
		/// </summary>
		/// <returns>An instance of the serializer corresponding to <see cref="SerializerType"/>.</returns>
		INapSerializer GetSerializer();
	}
}