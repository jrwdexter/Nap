using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using System;

namespace Nap.Html.Attributes
{
    /// <summary>
    /// Describes a simple binding element, selecting a node for binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class HtmlElementAttribute : BaseHtmlAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlElementAttribute"/> class.
		/// </summary>
        /// <param name="selector">The selector to use to find the element.</param>
		public HtmlElementAttribute(string selector) : base(selector)
	    {
	    }

	    /// <summary>
        /// Gets or sets the binding behavior of this element selector.
        /// </summary>
        public BindingBehavior BindingBehavior { get; set; }

	    /// <summary>
        /// Gets or sets the name of the attribute for use with <see cref="Attribute" />.
        /// </summary>
        public string AttributeName { get; set; }
	}
}
