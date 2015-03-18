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
		private readonly string _selector;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElementAttribute"/> class.
        /// </summary>
        /// <param name="selector">The selector to use to find the element.</param>
		public HtmlElementAttribute(string selector)
		{
			_selector = selector;
		}

        /// <summary>
        /// Gets or sets the binding behavior of this element selector.
        /// </summary>
        public BindingBehavior BindingBehavior { get; set; }

        /// <summary>
        /// Gets or sets the name of the attribute for use with <see cref="BindingBehavior.Attribute" />.
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets the selector that has been set to find this element.
        /// </summary>
        public string Selector  => _selector;
	}
}
