using System;
using System.Diagnostics.Contracts;

using HtmlAgilityPack;
using Nap.Html.Attributes.Base;
using Nap.Html.Finders.Base;

namespace Nap.Html.Parsers.Base
{
    /// <summary>
    /// Describes an interface for parsing a specific type of <see cref="BaseHtmlAttribute"/> property.
    /// </summary>
    /// <typeparam name="T">The type of attribute to parse for.</typeparam>
    public interface IParser<T> where T : BaseHtmlAttribute
    {
        /// <summary>
        /// Parses some string value out of an <see cref="HtmlNode" /> selected by an <see cref="IFinder" />
        /// </summary>
        /// <param name="node">The node that we are preparing to bind on.</param>
        /// <param name="attributeInstance">The instance of the attribute that is being used to perform binding.</param>
        /// <returns>The string value of the element to pass to the binder for binding to the POCO.</returns>
		[Pure]
        string Parse(HtmlNode node, T attributeInstance);
    }

}
