using System;
using System.Collections.Generic;
using System.Linq;
using Nap.Serializers;
using Nap.Serializers.Base;

namespace Nap.Configuration
{
    /// <summary>
    /// An empty implementation of the serializers collection, which does not pull from *.config files.
    /// </summary>
	public class EmptySerializersConfig : List<ISerializerConfig>, ISerializersConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySerializersConfig"/> class.
        /// </summary>
        /// <remarks>Initializes the class with 3 serializers already part of it: <see cref="NapFormsSerializer"/>, <see cref="NapJsonSerializer"/> and <see cref="NapXmlSerializer"/>.</remarks>
        public EmptySerializersConfig()
        {
            Add(new EmptySerializerConfig(new NapFormsSerializer()));
            Add(new EmptySerializerConfig(new NapJsonSerializer()));
            Add(new EmptySerializerConfig(new NapXmlSerializer()));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySerializersConfig"/> class.
        /// </summary>
        /// <param name="serializerConfigs">The serializer configs to use to initialize the class.</param>
        public EmptySerializersConfig(IEnumerable<ISerializerConfig> serializerConfigs)
        {
            AddRange(serializerConfigs);
        }

        /// <summary>
        /// Adds the specified serializer by specifying key/value pair.
        /// </summary>
        /// <param name="contentType">The key of the serializer to add (see MIME types).</param>
        /// <param name="serializerType">The full type name of the serializer to add.</param>
        public void Add(string contentType, string serializerType)
        {
            var serializerConfiguration = new EmptySerializerConfig(contentType, serializerType);
            Add(serializerConfiguration);
        }

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="INapSerializer" /> to add.</typeparam>
        public void Add<T>() where T : INapSerializer, new() => Add(new EmptySerializerConfig(new T()));

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="INapSerializer" /> to add.</typeparam>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        public void Add<T>(string contentType) where T : INapSerializer, new() => Add(new EmptySerializerConfig(contentType, new T()));

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        public void Add(INapSerializer napSerializer) => Add(new EmptySerializerConfig(napSerializer));

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        public void Add(INapSerializer napSerializer, string contentType) => Add(new EmptySerializerConfig(contentType, napSerializer));

        /// <summary>
        /// Remvoes the specified serializer by key, a string of appropriate MIME type.
        /// </summary>
        /// <param name="contentType">The MIME type key of the serializer to remove.</param>
        public void Remove(string contentType)
        {
            var toRemove = this.FirstOrDefault(serializers => serializers.ContentType == contentType);
            if (toRemove != null)
                Remove(toRemove);
        }

        /// <summary>
        /// Converts the <see cref="ISerializersConfig"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>) do not persist.
        /// </summary>
        /// <returns>The <see cref="ISerializersConfig"/> interface as a dictionary.</returns>
        public IDictionary<string, INapSerializer> AsDictionary()
        {
            return this.ToDictionary(serializer => serializer.ContentType, serializer => serializer.GetSerializer());
        }
    }
}