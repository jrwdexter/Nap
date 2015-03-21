using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using CsQuery;
using Nap.Html.Attributes.Base;
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
		[Pure]
		public virtual T Handle(string input, CQ context)
		{
			return (T)Handle(input, context, typeof(T), null);
		}

		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context" /> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="attribute"></param>
		/// <returns>
		/// The output type object created, and filled with the parsed version of the <see cref="input" />.
		/// </returns>
		public virtual object Handle(string input, CQ context, BaseHtmlAttribute attribute)
		{
			return Handle(input, context, typeof(T), attribute);
		}


		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		[Pure]
		public virtual object Handle(string input, CQ context, Type outputType)
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
		public abstract object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute);
	}
}
