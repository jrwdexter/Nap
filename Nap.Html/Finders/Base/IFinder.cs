using System.Diagnostics.Contracts;

using HtmlAgilityPack;

namespace Nap.Html.Finders.Base
{
    /// <summary>
    /// An interface describing how an element is found within another element, before parsing and binding occur.
    /// </summary>
    public interface IFinder
    {
        /// <summary>
        /// Find a node within another node (possibly the document root) to prepare for binding.
        /// </summary>
        /// <param name="currentNode">The node that we are currently operating on.</param>
        /// <param name="selector">The selector to find the next element.</param>
        /// <returns>A new element, found by performing the selector <paramref name="selector" /> on the current node.</returns>
		[Pure]
        HtmlNode FindNode(HtmlNode currentNode, string selector);
    }
}
