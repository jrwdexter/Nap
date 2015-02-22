using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Nap.Configuration.Utilities;
using Nap.Exceptions;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// Represents a collection of headers that can be populated for use with each request.
    /// The class can be populated by *.config files.
    /// </summary>
    /// <example>
    /// Configuration can be implemented as:
    /// <c>
    /// &lt;Nap&gt;
    ///   &lt;Headers&gt;
    ///     &lt;add key="Content-Type" value="application/json" /&gt;
    ///   &lt;/Headers&gt;
    /// &lt;Nap&gt;
    /// </c>
    /// or by configuration on <see cref="Nap.Config"/>.
    /// </example>
    public class Headers : ConfigurationElementCollection, IHeaders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        public Headers()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Headers"/> class.
        /// </summary>
        /// <param name="headers">The headers used to initialize the collection.</param>
        public Headers(IEnumerable<IHeader> headers)
        {
            foreach (var header in headers)
                Add(new Header { Key = header.Key, Value = header.Value });
        }

        /// <summary>
        /// Creates a new <see cref="Header"/> and returns it.
        /// </summary>
        /// <returns>A new <see cref="Header"/>.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new Header();
        }

        /// <summary>
        /// Gets the <see cref="Header.Key"/> value for a given header.
        /// </summary>
        /// <returns>
        /// A string that acts as the key for the <see cref="Header"/> object.
        /// </returns>
        /// <param name="element">The <see cref="Header"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            CheckType(element);
            return ((Header)element).Key;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// Wraps the base enumerator.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public new IEnumerator<IHeader> GetEnumerator()
        {
            return new ConfigurationEnumeratorWrapper<IHeader>(base.GetEnumerator());
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// Adds the element to the base <see cref="ConfigurationElementCollection"/> class.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(IHeader item)
        {
            CheckType(item);
            BaseAdd((ConfigurationElement)item);
        }

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
        /// Cheks the base <see cref="ConfigurationElementCollection"/> class to see if the element is present.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(IHeader item)
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
        public void CopyTo(IHeader[] array, int arrayIndex)
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
        public bool Remove(IHeader item)
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
        bool ICollection<IHeader>.IsReadOnly => false;

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
        /// Adds the specified header by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the header to add.</param>
        /// <param name="value">The value of the header to add.</param>
        public void Add(string key, string value)
        {
            Add(new Header { Key = key, Value = value });
        }

        /// <summary>
        /// Remvoes the specified header by key.
        /// </summary>
        /// <param name="key">The key of the header to remove.</param>
        public void Remove(string key)
        {
            var itemToRemove = this.FirstOrDefault(h => h.Key == key);
            if (itemToRemove != null)
                Remove(itemToRemove);
        }

        /// <summary>
        /// Converts the <see cref="IHeaders"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="System.Collections.Generic.ICollection{T}.Clear()"/>
        /// </summary>
        /// <returns>The <see cref="IHeaders"/> interface as a dictionary.</returns>
        public IDictionary<string, string> AsDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
                dictionary.Add(enumerator.Current.Key, enumerator.Current.Value);
            return dictionary;
        }

        /// <summary>
        /// Checks the type of the <paramref name="element"/> to ensure it can be operated on by this class.
        /// </summary>
        /// <param name="element">The element to check to ensure that it implements <see cref="IHeader"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        private void CheckType(ConfigurationElement element)
        {
            if (element.GetType().GetInterfaces().All(i => i != typeof(IHeader)))
                throw new NapConfigurationException("Configuration element being acted on by IHeaders must implement IHeader.");
        }

        /// <summary>
        /// Checks the type of the <paramref name="header"/> to ensure it can be operated on by this class (it has to be a <see cref="ConfigurationElement"/>).
        /// </summary>
        /// <param name="header">The element to check to ensure that it is a <see cref="ConfigurationElement"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        private void CheckType(IHeader header)
        {
            if (!(header is ConfigurationElement))
                throw new NapConfigurationException("Header element being acted on by IHeaders must inherit ConfigurationElement.");
        }
    }
}