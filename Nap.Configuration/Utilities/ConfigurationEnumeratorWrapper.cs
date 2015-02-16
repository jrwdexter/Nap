using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration.Utilities
{
    /// <summary>
    /// A simple enumerator class that wraps a given enumerator to convert it to a hard-type.
    /// If a non-<typeparamref name="T"/> type is found during execution of <see cref="Current"/>, the code will throw an <see cref="InvalidCastException"/>.
    /// </summary>
    /// <typeparam name="T">The type of the enumerator to produce.</typeparam>
    internal class ConfigurationEnumeratorWrapper<T> : IEnumerator<T>
    {
        private readonly IEnumerator _innerEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEnumeratorWrapper{T}" /> class.
        /// </summary>
        /// <param name="innerEnumerator">The inner enumerator that backs this enumerator.</param>
        public ConfigurationEnumeratorWrapper(IEnumerator innerEnumerator)
        {
            _innerEnumerator = innerEnumerator;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            return _innerEnumerator.MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            _innerEnumerator.Reset();
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator, cast to <typeparamref name="T"/>.
        /// If a non-<typeparamref name="T"/> type is found during execution of <see cref="Current"/>, the code will throw an <see cref="InvalidCastException"/>.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        /// <exception cref="InvalidCastException">Thrown if a non-<typeparamref name="T"/> element is found.</exception>
        public T Current => (T)_innerEnumerator.Current;

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        object IEnumerator.Current => _innerEnumerator.Current;

        /// <summary>
        /// Does nothing - there are no resources to dispose in this case.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }
    }
}