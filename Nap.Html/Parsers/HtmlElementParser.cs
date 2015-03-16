using HtmlAgilityPack;
using Nap.Html.Attributes;
using Nap.Html.Enum;
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
        public string Parse(HtmlNode node, HtmlElementAttribute attributeInstance)
        {
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
    }
}
