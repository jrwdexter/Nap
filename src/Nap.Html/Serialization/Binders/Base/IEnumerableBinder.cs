using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CsQuery;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;

namespace Nap.Html.Serialization.Binders.Base
{
	/// <summary>
	/// Describes an interface for handling binding behavior; that is, taking in an element and mapping it to a POCO.
	/// </summary>
	public interface IEnumerableBinder
	{
		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <paramref name="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="context">The collection of contexts to be bound by other binders.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <paramref name="context"/> nodes.</returns>
		[Pure]
		object Handle(IEnumerable<CQ> context, Type outputType);

		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <paramref name="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="context">The collection of contexts to be bound by other binders.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <param name="attribute">The optional attribute decorating the property that is currently being handled.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <paramref name="context"/> nodes.</returns>
		[Pure]
		object Handle(IEnumerable<CQ> context, Type outputType, BaseHtmlAttribute attribute);
	}
}
