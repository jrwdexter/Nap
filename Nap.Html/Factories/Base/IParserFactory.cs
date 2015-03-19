using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Parsers.Base;

namespace Nap.Html.Factories.Base
{
	/// <summary>
	/// An interface for a factory pattern to supply parsers for specific attributes (ie <see cref="HtmlElementAttribute"/>).
	/// </summary>
	public interface IParserFactory
	{
		/// <summary>
		/// Gets the parser for a specific attribute decorator (ones that inherit from <see cref="BaseHtmlAttribute"/>).
		/// </summary>
		/// <typeparam name="T">The type of attribute to retrieve the parser for.</typeparam>
		/// <returns>The parser implementaiton corresponding to the specified type.</returns>
		[Pure]
		IParser<T> GetParser<T>() where T : BaseHtmlAttribute;

		/// <summary>
		/// Gets the parser for a specific attribute decorator (ones that inherit from <see cref="BaseHtmlAttribute"/>).
		/// </summary>
		/// <param name="type">The type of attribute to retrieve the parser for.</param>
		/// <returns>The parser implementaiton corresponding to the specified type.</returns>
		[Pure]
		IParser GetParser(Type type);
	}
}
