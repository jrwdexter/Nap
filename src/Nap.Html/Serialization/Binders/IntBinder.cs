using System;
using System.Diagnostics.Contracts;
using CsQuery;
using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Formats;
using Nap.Html.Serialization.Binders.Base;

namespace Nap.Html.Serialization.Binders
{
    /// <summary>
    /// A binder for decimal types.
    /// </summary>
    public sealed class IntBinder : BaseBinder<int>
    {
        private static readonly DefaultBindingFormat DefaultBindingBehavior = new DefaultBindingFormat();

        /// <summary>
        /// Binds the specified input string to an output object of a certain primitive type.
        /// The value <paramref name="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
        /// </summary>
        /// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
        /// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
        /// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
        /// <param name="attribute">The optional attribute decorating the property that is currently being handled.</param>
        /// <returns>The output type object created, and filled with the parsed version of the <paramref name="input"/>.</returns>
        [Pure]
        public override object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
        {
            try
            {
                var bindingFormat = (attribute as NumericalHtmlElementAttribute)?.BindingFormat ?? DefaultBindingBehavior;

                if (outputType.IsGenericType && outputType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    int d;
                    return int.TryParse(bindingFormat.ParseToNumber(input) ?? input, bindingFormat.NumberStyle, bindingFormat.FormatInfo, out d) ? (int?)d : null;
                }

                return int.Parse(bindingFormat.ParseToNumber(input) ?? input, bindingFormat.NumberStyle, bindingFormat.FormatInfo);
            }
            catch (Exception e)
            {
                throw new NapBindingException($"An error occurred binding {input} to type {outputType.FullName}.  See inner exception for details.", e);
            }
        }
    }
}
