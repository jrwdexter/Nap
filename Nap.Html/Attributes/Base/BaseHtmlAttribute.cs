using System;

namespace Nap.Html.Attributes.Base
{
    /// <summary>
    /// The base attribute for all Nap.Html attributes associated with selecting/binding elements to POCOs.
    /// </summary>
	public abstract class BaseHtmlAttribute : Attribute
	{
		private readonly string _selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHtmlAttribute"/> class.
        /// </summary>
        /// <param name="selector">The selector to use to find the element.</param>
		protected BaseHtmlAttribute(string selector)
		{
			_selector = selector;
		}

        /// <summary>
        /// Gets the selector that has been set to find this element.
        /// </summary>
	    public string Selector => _selector;
	}
}