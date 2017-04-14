using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Nap.Html.Serialization.Binders.Base;

namespace Nap.Html.Factories.Base
{
	/// <summary>
	/// An interface for a factory pattern to supply binders of specific type models.
	/// </summary>
	public interface IBinderFactory
	{
		/// <summary>
		/// Gets the binder for a specific type of object (eg string, int, etc).
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve a binder for.</typeparam>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		[Pure]
		IBinder GetBinder<T>();

		/// <summary>
		/// Gets the binder for a specific type of object (eg string, int, etc).
		/// </summary>
		/// <param name="type">The type of object to retrieve a binder for.</param>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		[Pure]
		IBinder GetBinder(Type type);

	    /// <summary>
	    /// Gets the binder for enumerables.
	    /// </summary>
	    /// <returns>The binder implementaiton corresponding to the specified type.</returns>
	    [Pure]
	    IEnumerableBinder GetEnumerableBinder();


		/// <summary>
		/// Gets the binder for a specific object (eg string, int, etc).
		/// </summary>
		/// <param name="model">The object to retrieve a binder for.</param>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if model is null.</exception>
		[Pure]
		IBinder GetBinderForModel(object model);
	}
}