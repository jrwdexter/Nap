using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Html.Enum
{
    /// <summary>
    /// Enumeration to describe the style of binding (InnerHtml, InnerText, etc).
    /// </summary>
    public enum BindingBehavior
    {
        /// <summary>
        /// Bind to the inner text (removing elements) of the selected/found node.
        /// </summary>
        InnerText,

        /// <summary>
        /// Bind to the inner html (not removing elements) of the selected/found node.
        /// </summary>
        InnerHtml,

        /// <summary>
        /// Bind to a specific attribute value of the selected/found node.
        /// Additional information is required for this type of binding.
        /// </summary>
        Attribute,

        /// <summary>
        /// Bind to the classes of the selected/found node.
        /// </summary>
        Class,

        /// <summary>
        /// Bind to the ID of the selected/found node.
        /// </summary>
        Id,

        /// <summary>
        /// Bind to the <c>value=""</c> (specific attribute) of the selected/found node.
        /// </summary>
        Value
    }
}
