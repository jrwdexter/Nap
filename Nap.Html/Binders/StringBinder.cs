using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using CsQuery;

using Nap.Html.Binders.Base;
using Nap.Html.Enum;
using Nap.Html.Attributes.Base;

namespace Nap.Html.Binders
{
	/// <summary>
	/// A simple binder for string types.
	/// </summary>
	public sealed class StringBinder : BaseBinder<string>
	{
		/// <summary>
		/// Binds the specified input string to an output object of type <see cref="string"/>
		/// The value <see cref="context" /> is unused.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
		/// <param name="context">Unused in this case.</param>
		/// <param name="outputType">Unused in this case.</param>
		/// <param name="attribute">Unused in this case.</param>
		/// <returns>
		/// The output type object created, and filled with the parsed version of the <see cref="input" />.
		/// </returns>
		[Pure]
		public override object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
		{
			return input;
		}
	}
}
