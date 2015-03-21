using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using CsQuery;

using Nap.Html.Finders.Base;

namespace Nap.Html.Finders
{
	/// <summary>
	/// A finder for locating single nodes.
	/// </summary>
	public sealed class EnumerableNodeFinder : IFinder<IEnumerable<CQ>>
	{
		/// <summary>
		/// Find an item of uknown type within another node (possibly the document root) to prepare for binding.
		/// Will return a type of item as defined by the <see cref="IFinder{T}.FindItem"/> method.
		/// </summary>
		/// <param name="currentNode">The node that we are currently operating on.</param>
		/// <param name="selector">The selector to find the next element.</param>
		/// <returns>A new element, found by performing the selector <paramref name="selector" /> on the current node.</returns>
		public object Find(CQ currentNode, string selector)
		{
			return FindItem(currentNode, selector);
		}

		/// <summary>
		/// Find a node within another node (possibly the document root) to prepare for binding.
		/// </summary>
		/// <param name="currentNode">The node that we are currently operating on.</param>
		/// <param name="selector">The selector to find the next element.</param>
		/// <returns>A new element, found by performing the selector <paramref name="selector" /> on the current node.</returns>
		[Pure]
		public IEnumerable<CQ> FindItem(CQ currentNode, string selector)
		{
			if (currentNode == null)
				throw new ArgumentNullException(nameof(currentNode));
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			return currentNode.Select(selector).Select(cq => new CQ(cq));
		}
	}
}
