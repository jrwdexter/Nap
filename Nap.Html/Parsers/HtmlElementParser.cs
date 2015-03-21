using System;
using System.Diagnostics.Contracts;
using System.Linq;

using CsQuery;
using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Finders.Base;
using Nap.Html.Parsers.Base;
using Nap.Html.Extensions;

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
		public string Parse(CQ node, HtmlElementAttribute attributeInstance)
		{
			if (node == null)
				throw new ArgumentNullException(nameof(node));
			if (attributeInstance == null)
				throw new ArgumentNullException(nameof(attributeInstance));

			switch (attributeInstance.BindingBehavior)
			{
				case BindingBehavior.InnerHtml:
					return node.Html();
				case BindingBehavior.Class:
					return node.Attr("class");
				case BindingBehavior.Id:
					return node.Attr("id");
				case BindingBehavior.Value:
					return node.Attr("value");
				case BindingBehavior.Attribute:
					return node.Attr(attributeInstance.AttributeName);
				case BindingBehavior.InnerText:
					return node.Text()?.Trim();
				case BindingBehavior.Smart:
				default:
					return ParseSmart(node, attributeInstance);
			}
		}

		/// <summary>
		/// Parses some string value out of an <see cref="HtmlNode" /> selected by an <see cref="IFinder{T}" />
		/// </summary>
		/// <param name="node">The node that we are preparing to bind on.</param>
		/// <param name="attributeInstance">The instance of the attribute that is being used to perform binding.</param>
		/// <returns>The string value of the element to pass to the binder for binding to the POCO.</returns>
		/// <exception cref="ArgumentNullException">Thrown if either <paramref name="node"/> or <paramref name="attributeInstance"/> is null.</exception>
		public string Parse(CQ node, BaseHtmlAttribute attributeInstance)
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

		/// <summary>
		/// Parses the node in an intelligent fashion, trying to find selected optoins, values, and finally inner text..
		/// </summary>
		/// <param name="node">The node to parse.</param>
		/// <param name="attribute">The attribute corresponding to the property being bound.</param>
		/// <returns>A string containing either the selected option's value, the value attribute, or the inner text of the node.</returns>
		private string ParseSmart(CQ node, HtmlElementAttribute attribute)
		{
			var option = node.Select("option[selected]", node).FirstOrDefault();
			var input = node.Select("input", node).FirstOrDefault();
			return option?.InnerText?.TrimAll() ?? input?.Attributes["value"] ?? node.Attr("value") ?? node.Text()?.TrimAll();
		}
	}
}
