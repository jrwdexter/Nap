using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Finders.Base;

namespace Nap.Html.Factories.Base
{
	/// <summary>
	/// An interface for a factory pattern to supply finders of specific return types (eg. enumerable or not).
	/// </summary>
	public interface IFinderFactory
	{
		/// <summary>
		/// Gets the finder for a specific type of return object (eg Enumerable or non-enumerable).
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve a finder for.</typeparam>
		/// <returns>The finder implementaiton corresponding to the specified type.</returns>
		[Pure]
		IFinder<T> GetFinder<T>();

		/// <summary>
		/// Gets the finder for a specific type of return object (eg Enumerable or non-enumerable).
		/// </summary>
		/// <param name="type">The type of object to retrieve a finder for.</param>
		/// <returns>The finder implementaiton corresponding to the specified type.</returns>
		[Pure]
		IFinder GetFinder(Type type);
	}
}
