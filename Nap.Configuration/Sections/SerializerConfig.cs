using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Nap.Exceptions;
using Nap.Serializers.Base;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// An implementation of <see cref="ISerializerConfig"/> that allows for use of *.config files.
    /// </summary>
	public class SerializerConfig : ConfigurationElement, ISerializerConfig
	{
		private readonly object _padlock = new object();
		private INapSerializer _serializerInstance;

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerConfig"/> class.
		/// </summary>
		public SerializerConfig()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerConfig"/> class.
		/// </summary>
		/// <param name="serializerInstance">The serializer instance used to initialize the class.</param>
		public SerializerConfig(INapSerializer serializerInstance)
		{
			if (serializerInstance == null)
				throw new ArgumentNullException(nameof(serializerInstance));

			ContentType = serializerInstance.ContentType;
			_serializerInstance = serializerInstance;
			SerializerType = serializerInstance.GetType().FullName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap serializer configuration applies to.</param>
		/// <param name="serializerInstance">The serializer instance used to initialize the class.</param>
		public SerializerConfig(string contentType, INapSerializer serializerInstance)
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
		/// Gets or sets the content type (MIME)that this serializer configuration is for.
		/// </summary>
		[ConfigurationProperty("contentType", IsRequired = true)]
		public string ContentType
		{
			get { return (string)this["contentType"]; }
			set { this["contentType"] = value; }
		}

		/// <summary>
		/// Gets or sets the full name of the type of the serializer.
		/// </summary>
		/// <example><c>Nap.Serializers.NapJsonSerializer</c></example>
		[ConfigurationProperty("serializerType", IsRequired = true)]
		public string SerializerType
		{
			get { return (string)this["serializerType"]; }
			set { this["serializerType"] = value; }
		}

		/// <summary>
		/// Gets the instance of the serializer corresponding to <see cref="ISerializerConfig.SerializerType"/>.
		/// </summary>
		/// <returns>An instance of the serializer corresponding to <see cref="ISerializerConfig.SerializerType"/>.</returns>
		public INapSerializer GetSerializer()
		{
			if (_serializerInstance == null)
			{
				lock (_padlock)
				{
					if (_serializerInstance == null)
					{
						if (SerializerType == null)
							throw new NapConfigurationException("SerializerType is null, and SerializerConfiguration was not instantiated with an INapSerializer.");

						var serializerType = Type.GetType(SerializerType);
						if (serializerType == null)
							throw new NapConfigurationException($"No serializer found for type name {SerializerType}");

						_serializerInstance = (INapSerializer)Activator.CreateInstance(serializerType);
					}
				}
			}

			return _serializerInstance;
		}

		/// <summary>
		/// Creates a copy of the <see cref="SerializerConfig"/> configuration.
		/// </summary>
		/// <returns>A copy of the current instance.</returns>
		public SerializerConfig Clone()
		{
			return new SerializerConfig { ContentType = ContentType, SerializerType = SerializerType };
		}
	}
}
