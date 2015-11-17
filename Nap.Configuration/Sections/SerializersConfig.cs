using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Nap.Configuration.Utilities;
using Nap.Exceptions;
using Nap.Serializers;
using Nap.Serializers.Base;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// An implementation of <see cref="ISerializersConfig"/> that allows for use of *.config files.
    /// </summary>
	public class SerializersConfig : ConfigurationElementCollection, ISerializersConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializersConfig"/> class.
		/// </summary>
		/// <remarks>Adds three serializers by default: <see cref="NapFormsSerializer"/>, <see cref="NapJsonSerializer"/> and <see cref="NapXmlSerializer"/></remarks>
		public SerializersConfig()
		{
			Add(new SerializerConfig(new NapFormsSerializer()));
			Add(new SerializerConfig(new NapJsonSerializer()));
			Add(new SerializerConfig(new NapXmlSerializer()));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializersConfig"/> class.
		/// </summary>
		/// <param name="serializers">The serializers used to initialize this class.</param>
		public SerializersConfig(IEnumerable<ISerializerConfig> serializers)
		{
			foreach (var serializer in serializers)
				Add(new SerializerConfig { ContentType = serializer.ContentType, SerializerType = serializer.SerializerType });
		}

        /// <summary>
        /// Creates a new <see cref="SerializerConfig"/> and returns it.
        /// </summary>
        /// <returns>A new <see cref="SerializerConfig"/>.</returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new SerializerConfig();
		}

		/// <summary>
		/// Gets the <see cref="SerializerConfig.ContentType"/> value for a given header.
		/// </summary>
		/// <returns>
		/// A string that acts as the key for the <see cref="SerializerConfig"/> object.
		/// </returns>
		/// <param name="element">The <see cref="SerializerConfig"/> to return the key for. </param>
		protected override object GetElementKey(ConfigurationElement element)
		{
			CheckType(element);
			return ((SerializerConfig)element).ContentType;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// Wraps the base enumerator.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public new IEnumerator<ISerializerConfig> GetEnumerator()
		{
			return new ConfigurationEnumeratorWrapper<ISerializerConfig>(base.GetEnumerator());
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Adds the element to the base <see cref="ConfigurationElementCollection"/> class.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(ISerializerConfig item)
		{
			CheckType(item);
			BaseAdd((ConfigurationElement)item);
		}

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="INapSerializer"/> to add.</typeparam>
        public void Add<T>() where T : INapSerializer, new() => Add(new SerializerConfig(new T()));

        /// <summary>
        /// Adds the specified serializer generically.
        /// </summary>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        /// <typeparam name="T">The type of <see cref="INapSerializer"/> to add.</typeparam>
        public void Add<T>(string contentType) where T : INapSerializer, new() => Add(new SerializerConfig(contentType, new T()));

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        public void Add(INapSerializer napSerializer) => Add(new SerializerConfig(napSerializer));

        /// <summary>
        /// Adds the specified serializer by specifying an instance of the serializer.
        /// </summary>
        /// <param name="napSerializer">The serializer instance to add to the collection of serializers.</param>
        /// <param name="contentType">The content type to apply the serializer to.</param>
        public void Add(INapSerializer napSerializer, string contentType) => Add(new SerializerConfig(contentType, napSerializer));

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Clears elements from the base <see cref="ConfigurationElementCollection"/> class.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
		public void Clear()
		{
			BaseClear();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// Checks the base <see cref="ConfigurationElementCollection"/> class to see if the element is present.
		/// </summary>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		public bool Contains(ISerializerConfig item)
		{
			CheckType(item);
			var itemFound = false;
			var enumerator = GetEnumerator();
			while (enumerator.MoveNext())
				itemFound = itemFound || enumerator.Current == item;

			return itemFound;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
		public void CopyTo(ISerializerConfig[] array, int arrayIndex)
		{
			var enumerator = GetEnumerator();
			while (enumerator.MoveNext())
				array[arrayIndex++] = enumerator.Current;
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// Removes elements from the base <see cref="ConfigurationElementCollection"/> class.
		/// </summary>
		/// <returns>
		/// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </returns>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public bool Remove(ISerializerConfig item)
		{
			CheckType(item);
			var index = BaseIndexOf((ConfigurationElement)item);
			if (index >= 0)
				BaseRemoveAt(index);

			return index >= 0;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// False in this case.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
		/// </returns>
		bool ICollection<ISerializerConfig>.IsReadOnly => false;

		/// <summary>
		/// Indicates whether the <see cref="T:System.Configuration.ConfigurationElementCollection"/> object is read only.
		/// False in this case.
		/// </summary>
		/// <returns>
		/// true if the <see cref="T:System.Configuration.ConfigurationElementCollection"/> object is read only; otherwise, false.
		/// </returns>
		public override bool IsReadOnly()
		{
			return false;
		}

		/// <summary>
		/// Adds the specified serializer by specifying key/value pair.
		/// </summary>
		/// <param name="contentType">The key of the serializer to add (see MIME types).</param>
		/// <param name="serializerType">The full type name of the serializer to add.</param>
		public void Add(string contentType, string serializerType)
		{
			Add(new SerializerConfig { ContentType = contentType, SerializerType = serializerType });
		}

		/// <summary>
		/// Remvoes the specified serializer by key, a string of appropriate MIME type.
		/// </summary>
		/// <param name="contentType">The MIME type key of the serializer to remove.</param>
		public void Remove(string contentType)
		{
			var itemToRemove = this.FirstOrDefault(h => h.ContentType == contentType);
			if (itemToRemove != null)
				Remove(itemToRemove);
		}

		/// <summary>
		/// Converts the <see cref="IHeaders"/> interface to a dicitonary.
		/// Note that operations on this object (such as <see cref="System.Collections.Generic.ICollection{T}.Clear()"/>
		/// </summary>
		/// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
		public IDictionary<string, INapSerializer> AsDictionary()
		{
			var dictionary = new Dictionary<string, INapSerializer>();
			var enumerator = GetEnumerator();
			while (enumerator.MoveNext())
				dictionary.Add(enumerator.Current.ContentType, enumerator.Current.GetSerializer());
			return dictionary;
		}

		/// <summary>
		/// Checks the type of the <paramref name="element"/> to ensure it can be operated on by this class.
		/// </summary>
		/// <param name="element">The element to check to ensure that it implements <see cref="ISerializerConfig"/>.</param>
		// ReSharper disable once UnusedParameter.Local
		private void CheckType(ConfigurationElement element)
		{
			if (element.GetType().GetInterfaces().All(i => i != typeof(ISerializerConfig)))
				throw new NapConfigurationException("Configuration element being acted on by ISerializersConfig must implement ISerializerConfig.");
		}

		/// <summary>
		/// Checks the type of the <paramref name="serializerConfig"/> to ensure it can be operated on by this class (it has to be a <see cref="ConfigurationElement"/>).
		/// </summary>
		/// <param name="serializerConfig">The element to check to ensure that it is a <see cref="ConfigurationElement"/>.</param>
		// ReSharper disable once UnusedParameter.Local
		private void CheckType(ISerializerConfig serializerConfig)
		{
			if (!(serializerConfig is ConfigurationElement))
				throw new NapConfigurationException("Header element being acted on by ISerializersConfig must inherit ConfigurationElement.");
		}
    }
}