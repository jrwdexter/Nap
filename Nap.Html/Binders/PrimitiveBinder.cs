using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using HtmlAgilityPack;

using Nap.Html.Binders.Base;
using Nap.Html.Enum;

namespace Nap.Html.Binders
{
	/// <summary>
	/// A binder for most primitive types, including <see cref="int"/>, <see cref="double"/>, etc.
	/// </summary>
	public sealed class PrimitiveBinder : IBinder
	{
		/// <summary>
		/// Binds the specified input string to an output object of a certain primitive type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		[Pure]
		public object Handle(string input, HtmlNode context, Type outputType)
		{
			return Convert.ChangeType(input, outputType);
		}
	}
}
