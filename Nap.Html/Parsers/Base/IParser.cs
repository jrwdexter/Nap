using HtmlAgilityPack;
using Nap.Html.Attributes.Base;

namespace Nap.Html.Parsers.Base
{
    /// <summary>
    /// Describes an interface for parsing a specific type of 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IParser<T> where T : BaseHtmlAttribute
    {
        string Parse(HtmlNode node, T attributeInstance);
    }

}
