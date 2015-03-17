using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Nap.Html.Enum;

namespace Nap.Html.Binders.Base
{
	/// <summary>
	/// A base class for binders, allowing simplified binding calls without type passed in.
	/// </summary>
	/// <typeparam name="T">The type of object that this binder can handle.</typeparam>
	public abstract class BaseBinder<T> : IBinder<T>, IBinder
	{
		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <typeparam name="T">The type of output object to generate, whether a POCO, primitive, or other.</typeparam>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		public virtual T Handle(string input, HtmlNode context)
		{
			return (T)Handle(input, context, typeof(T));
		}

		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		public abstract object Handle(string input, HtmlNode context, Type outputType);
	}
}
