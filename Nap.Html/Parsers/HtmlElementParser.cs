using System;
using System.Diagnostics.Contracts;

using HtmlAgilityPack;
using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Finders.Base;
using Nap.Html.Parsers.Base;

namespace Nap.Html.Parsers
{
	/// <summary>
	/// The parser for elements decorated with <see cref="HtmlElementAttribute" />.
	/// </summary>
	internal class HtmlElementParser : IParser<HtmlElementAttribute>
	{
		/// <summary>
		/// Parses some string value out of an <see cref="HtmlNode" /> selected by an <see cref="IFinder" />
		/// </summary>
		/// <param name="node">The node that we are preparing to bind on.</param>
		/// <param name="attributeInstance">The instance of the attribute that is being used to perform binding.</param>
		/// <returns>The string value of the element to pass to the binder for binding to the POCO.</returns>
		/// <exception cref="ArgumentNullException">Thrown if either <paramref name="node"/> or <paramref name="attributeInstance"/> is null.</exception>
		[Pure]
		public string Parse(HtmlNode node, HtmlElementAttribute attributeInstance)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			if (attributeInstance == null)
				throw new ArgumentNullException(nameof(attributeInstance));

			switch (attributeInstance.BindingBehavior)
			{
				case BindingBehavior.InnerHtml:
					return node.InnerHtml;
				case BindingBehavior.Class:
					return node.Attributes["class"]?.Value;
				case BindingBehavior.Id:
					return node.Id;
				case BindingBehavior.Value:
					return node.Attributes["value"]?.Value;
				case BindingBehavior.InnerText:
				default:
					return node.InnerText;
			}
		}

		/// <summary>
		/// Parses some string value out of an <see cref="HtmlNode" /> selected by an <see cref="IFinder{T}" />
		/// </summary>
		/// <param name="node">The node that we are preparing to bind on.</param>
		/// <param name="attributeInstance">The instance of the attribute that is being used to perform binding.</param>
		/// <returns>The string value of the element to pass to the binder for binding to the POCO.</returns>
		/// <exception cref="ArgumentNullException">Thrown if either <paramref name="node"/> or <paramref name="attributeInstance"/> is null.</exception>
		public string Parse(HtmlNode node, BaseHtmlAttribute attributeInstance)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			if (attributeInstance == null)
				throw new ArgumentNullException(nameof(attributeInstance));

			try
			{
				return Parse(node, attributeInstance as HtmlElementAttribute);
			}
			catch (ArgumentNullException ex)
			{
				throw new NapParsingException($"Attribute {attributeInstance.GetType().FullName} is not of the correct type to be handled by ${GetType().FullName}.", ex);
			}
		}
	}
}
