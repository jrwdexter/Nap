using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Nap.Html.Enum;

namespace Nap.Html.Binders.Base
{
	/// <summary>
	/// Describes an interface for handling binding behavior; that is, taking in an element and mapping it to a POCO.
	/// </summary>
	public interface IBinder
	{
		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		object Handle(string input, HtmlNode context, Type outputType);
	}


	/// <summary>
	/// Describes an interface for handling binding behavior; that is, taking in an element and mapping it to a POCO of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The type of object that can be handled by this binder.</typeparam>
	public interface IBinder<out T>
	{
		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <typeparam name="T">The type of output object to generate, whether a POCO, primitive, or other.</typeparam>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		T Handle(string input, HtmlNode context);
	}
}
