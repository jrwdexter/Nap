using Nap.Configuration.Utilities;
using Nap.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Configuration.Sections.Base
{
    /// <summary>
    /// Describes a base class for Nap configuration collection elements (add/remove/clear).
    /// </summary>
    /// <typeparam name="T">The type of element that the collection is for.  Must inherit from <see cref="NapConfigurationElementBase{T, TV}"/>.</typeparam>
    /// <typeparam name="TI">
    /// The interface for the element type that the collection is for.
    /// <typeparamref name="T"/> must implement this interface.
    /// </typeparam>
    /// <typeparam name="TV">The value type that is contained within the element described by <typeparamref name="T"/> and <typeparamref name="TI"/>.</typeparam>
    public abstract class NapConfigurationCollectionBase<T, TI, TV> : ConfigurationElementCollection, ICollection<TI>
        where T : NapConfigurationElementBase<TI, TV>, TI, new()
        where TI : class
        where TV : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationCollectionBase{T, TI, TV}"/> class.
        /// </summary>
        public NapConfigurationCollectionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationCollectionBase{T, TI, TV}"/> class.
        /// </summary>
        /// <param name="initialCollection">The objects used to initialize the collection.</param>
        public NapConfigurationCollectionBase(IEnumerable<TI> initialCollection)
        {
            foreach (var element in initialCollection)
            {
                var toAdd = new T();
                toAdd.Hydrate(element);
                Add(toAdd);
            }
        }

        /// <summary>
        /// Creates a new <typeparamref name="T"/> and returns it.
        /// </summary>
        /// <returns>A new <typeparamref name="T"/>.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// Gets the <see cref="NapConfigurationElementBase{T, TV}.Key"/> value for a given element.
        /// </summary>
        /// <returns>
        /// A string that acts as the key for the <typeparamref name="T"/> object.
        /// </returns>
        /// <param name="element">The <typeparamref name="T"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            CheckType(element);
            return ((T)element).Key;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// Wraps the base enumerator.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public new IEnumerator<TI> GetEnumerator()
        {
            return new ConfigurationEnumeratorWrapper<TI>(base.GetEnumerator());
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// Adds the element to the base <see cref="ConfigurationElementCollection"/> class.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(TI item)
        {
            CheckType(item);
            BaseAdd((T)item);
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
        public bool Contains(TI item)
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
        public void CopyTo(TI[] array, int arrayIndex)
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
        public bool Remove(TI item)
        {
            CheckType(item);
            var index = BaseIndexOf((T)item);
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
        bool ICollection<TI>.IsReadOnly => false;

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
        /// Adds the specified <typeparamref name="T"/> by specifying key/value pair.
        /// </summary>
        /// <param name="key">The key of the <typeparamref name="T"/> to add.</param>
        /// <param name="value">The value of the <typeparamref name="T"/> to add.</param>
        public void Add(string key, TV value)
        {
            Add(new T { Key = key, Value = value });
        }

        /// <summary>
        /// Remvoes the specified <typeparamref name="TI"/> by key.
        /// </summary>
        /// <param name="key">The key of the <typeparamref name="TI"/> to remove.</param>
        public void Remove(string key)
        {
            var itemToRemove = this.OfType<T>().FirstOrDefault(h => h.Key == key);
            if (itemToRemove != null)
                Remove(itemToRemove);
        }

        /// <summary>
        /// Converts the <see cref="ICollection{T}"/> interface to a dicitonary.
        /// Note that operations on this object (such as <see cref="ICollection{T}.Clear()"/>
        /// </summary>
        /// <returns>The <see cref="ICollection{T}"/> interface as a dictionary.</returns>
        public IDictionary<string, TV> AsDictionary()
        {
            var dictionary = new Dictionary<string, TV>();
            var enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current as T;
                dictionary.Add(current.Key, current.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// Checks the type of the <paramref name="element"/> to ensure it can be operated on by this class.
        /// </summary>
        /// <param name="element">The element to check to ensure that it implements <typeparamref name="TI"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        private void CheckType(ConfigurationElement element)
        {
            if (element.GetType().GetInterfaces().All(i => i != typeof(TI)))
                throw new NapConfigurationException($"Configuration element being acted on by {GetType().FullName} must implement {typeof(TI).FullName}.");
        }

        /// <summary>
        /// Checks the type of the <paramref name="element"/> to ensure it can be operated on by this class (it has to be a <see cref="ConfigurationElement"/>).
        /// </summary>
        /// <param name="element">The element to check to ensure that it is a <see cref="ConfigurationElement"/>.</param>
        // ReSharper disable once UnusedParameter.Local
        private void CheckType(TI element)
        {
            if (!(element is ConfigurationElement))
                throw new NapConfigurationException($"Element being acted on by {GetType().FullName} must inherit {typeof(NapConfigurationElementBase<T, TV>).FullName}.");
        }
    }
}
