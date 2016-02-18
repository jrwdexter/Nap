using System;
using System.Diagnostics.Contracts;
using CsQuery;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Serialization.Binders.Base;

namespace Nap.Html.Serialization.Binders
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
		public object Handle(string input, CQ context, Type outputType)
		{
			return Handle(input, context, outputType, null);
		}

		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context" /> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <param name="attribute">The optional attribute decorating the property that is currently being handled.</param>
		/// <returns>
		/// The output type object created, and filled with the parsed version of the <see cref="input" />.
		/// </returns>
		public object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
		{
			try
			{
				return Convert.ChangeType(input, outputType);
			}
			catch (Exception e)
			{
				throw new NapBindingException($"An error occurred binding {input} to type {outputType.FullName}.  See inner exception for details.", e);
			}
		}
	}
}
