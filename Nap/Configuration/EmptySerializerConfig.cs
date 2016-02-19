using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Exceptions;
using Nap.Serializers.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Describes an implementation of the <see cref="ISerializerConfig"/> interface for non-*.config implementations.
	/// </summary>
	public class EmptySerializerConfig : ISerializerConfig
	{
		private readonly object _padlock = new object();
		private INapSerializer _serializerInstance;

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptySerializerConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap serializer configuration applies to.</param>
		/// <param name="serializerType">The serializer type used to initialize the class.</param>
		public EmptySerializerConfig(string contentType, string serializerType)
		{
			ContentType = contentType;
			SerializerType = serializerType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptySerializerConfig"/> class.
		/// </summary>
		/// <param name="serializerInstance">The serializer instance used to initialize the class.</param>
		public EmptySerializerConfig(INapSerializer serializerInstance)
		{
			if (serializerInstance == null)
				throw new ArgumentNullException(nameof(serializerInstance));

			ContentType = serializerInstance.ContentType;
			_serializerInstance = serializerInstance;
			SerializerType = serializerInstance.GetType().FullName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptySerializerConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap serializer configuration applies to.</param>
		/// <param name="serializerInstance">The serializer instance used to initialize the class.</param>
		public EmptySerializerConfig(string contentType, INapSerializer serializerInstance)
		{
			if (contentType == null)
				throw new ArgumentNullException(nameof(contentType));
			if (serializerInstance == null)
				throw new ArgumentNullException(nameof(serializerInstance));

			ContentType = contentType;
			_serializerInstance = serializerInstance;
			SerializerType = serializerInstance.GetType().FullName;
		}

		/// <summary>
		/// Gets or sets the request format that this serializer configuration is for.  See <see cref="RequestFormat"/>.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of the serializer.
		/// </summary>
		/// <example><c>Nap.Serializers.NapJsonSerializer</c></example>
		public string SerializerType { get; set; }

		/// <summary>
		/// Gets the instance of the serializer corresponding to <see cref="ISerializerConfig.SerializerType"/>.
		/// </summary>
		/// <returns>An instance of the serializer corresponding to <see cref="ISerializerConfig.SerializerType"/>.</returns>
		public INapSerializer GetSerializer()
		{
			if (SerializerType == null && _serializerInstance == null)
				throw new NapConfigurationException("SerializerType is null, and SerializerConfiguration was not instantiated with an INapSerializer.");

			if (_serializerInstance == null)
			{
				lock (_padlock)
				{
					if (_serializerInstance == null)
						_serializerInstance = (INapSerializer)Activator.CreateInstance(Type.GetType(SerializerType));
				}
			}

			return _serializerInstance;
		}
	}
}
