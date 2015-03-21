using System.Diagnostics.Contracts;

using CsQuery;

namespace Nap.Html.Finders.Base
{
	/// <summary>
	/// An interface describing how an item is found within another element, before parsing and binding occur.
	/// </summary>
	public interface IFinder
	{
		/// <summary>
		/// Find an item of uknown type within another node (possibly the document root) to prepare for binding.
		/// Will return a type of item as defined by the <see cref="IFinder{T}.FindItem"/> method.
		/// </summary>
		/// <param name="currentNode">The node that we are currently operating on.</param>
		/// <param name="selector">The selector to find the next element.</param>
		/// <returns>A new element, found by performing the selector <paramref name="selector" /> on the current node.</returns>
		object Find(CQ currentNode, string selector);
	}

	/// <summary>
	/// An interface describing how an item is found within another element, before parsing and binding occur.
	/// </summary>
	/// <typeparam name="T">The type of element for this finder to locate.</typeparam>
	public interface IFinder<out T> : IFinder
    {
        /// <summary>
        /// Find an item of <typeparamref name="T"/> within another node (possibly the document root) to prepare for binding.
        /// </summary>
        /// <param name="currentNode">The node that we are currently operating on.</param>
        /// <param name="selector">The selector to find the next element.</param>
        /// <returns>A new element, found by performing the selector <paramref name="selector" /> on the current node.</returns>
		[Pure]
        T FindItem(CQ currentNode, string selector);
    }
}
