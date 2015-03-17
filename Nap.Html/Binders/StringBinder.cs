using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Nap.Html.Binders.Base;
using Nap.Html.Enum;

namespace Nap.Html.Binders
{
	/// <summary>
	/// A simple binder for string types.
	/// </summary>
	public class StringBinder : BaseBinder<string>
	{
		/// <summary>
		/// Binds the specified input string to an output object of type <see cref="string"/>
		/// The value <see cref="context" /> is unused.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
		/// <param name="context">Unused in this case.</param>
		/// <param name="outputType">Unused in this case.</param>
		/// <returns>
		/// The output type object created, and filled with the parsed version of the <see cref="input" />.
		/// </returns>
		public override object Handle(string input, HtmlNode context, Type outputType)
		{
			return input;
		}
	}
}
