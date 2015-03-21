using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using CsQuery;

using System.Globalization;

using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Binders.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Formats;

namespace Nap.Html.Binders
{
	/// <summary>
	/// A binder for decimal types.
	/// </summary>
	public sealed class DecimalBinder : BaseBinder<decimal>, IBinder<decimal>
	{
		private static DefaultBindingFormat _defaultBindingBehavior = new DefaultBindingFormat();

		/// <summary>
		/// Binds the specified input string to an output object of a certain primitive type.
		/// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
		/// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
		[Pure]
		public override object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
		{
			try
			{
				var bindingFormat = (attribute as NumericalHtmlElementAttribute)?.BindingFormat ?? _defaultBindingBehavior;

				if (outputType.IsGenericType && outputType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					decimal d;
					var underlyingType = Nullable.GetUnderlyingType(outputType);
					return decimal.TryParse(bindingFormat.ParseToNumber(input) ?? input, bindingFormat.NumberStyle, bindingFormat.FormatInfo, out d) ? (decimal?)d : null;
				}

				return decimal.Parse(bindingFormat.ParseToNumber(input) ?? input, bindingFormat.NumberStyle, bindingFormat.FormatInfo);
			}
			catch (Exception e)
			{
				throw new NapBindingException($"An error occurred binding {input} to type {outputType.FullName}.  See inner exception for details.", e);
			}
		}
	}
}
