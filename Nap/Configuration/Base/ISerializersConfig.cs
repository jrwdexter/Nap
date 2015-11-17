using System;
using System.Collections.Generic;
using System.Linq;
using Nap.Serializers.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Represents a collection of serializers that can have additional serializers added.
	/// </summary>
	public interface ISerializersConfig : ICollection<ISerializerConfig>
	{
		/// <summary>
		/// Adds the specified serializer by specifying key/value pair.
		/// </summary>
		/// <param name="contentType">The key of the serializer to add (see MIME types).</param>
		/// <param name="serializerType">The full type name of the serializer to add.</param>
		void Add(string contentType, string serializerType);

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="INapSerializer"/> to add.</typeparam>
        void Add<T>() where T : INapSerializer, new();

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        /// <typeparam name="T">The type of <see cref="INapSerializer"/> to add.</typeparam>
        void Add<T>(string contentType) where T : INapSerializer, new();

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        void Add(INapSerializer napSerializer);

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        void Add(INapSerializer napSerializer, string contentType);

        /// <summary>
        /// Remvoes the specified serializer by key, a string of appropriate MIME type.
        /// </summary>
        /// <param name="contentType">The MIME type key of the serializer to remove.</param>
        void Remove(string contentType);

		/// <summary>
		/// Converts the <see cref="ISerializersConfig"/> interface to a dicitonary.
		/// Note that operations on this object (such as <see cref="IDictionary{T1,T2}.Add(T1, T2)"/>) do not persist.
		/// </summary>
		/// <returns>The <see cref="ISerializersConfig"/> interface as a dictionary.</returns>
		IDictionary<string, INapSerializer> AsDictionary();
	}
}