using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Nap.Html.Attributes.Base;
using Nap.Html.Factories;
using Nap.Html.Serialization.Binders.Base;

#if IMMUTABLE
using Microsoft.FSharp.Collections;
#endif

namespace Nap.Html.Serialization.Binders
{
	/// <summary>
	/// Describes an interface for handling enumerable binding behavior; that is, taking in an element and mapping it to a collection of POCOs.
	/// </summary>
    public class EnumerableBinder : IEnumerableBinder
    {
		/// <summary>
		/// Binds the specified input string to an output object of a certain type.
		/// The value <paramref name="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
		/// </summary>
		/// <param name="context">The collection of contexts to be bound by other binders.</param>
		/// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
		/// <returns>The output type object created, and filled with the parsed version of the <paramref name="context"/> nodes.</returns>
        [Pure]
        public object Handle(IEnumerable<CQ> context, Type outputType)
        {
            return Handle(context, outputType, null);
        }

        public object Handle(IEnumerable<CQ> context, Type outputType, BaseHtmlAttribute propertyAttribute)
        {
            // TODO: Cache reflection methods

            // Enumerable case
            var enumerableType = outputType.IsArray ? outputType.GetElementType() : outputType.GetGenericArguments().First();
            var castTypeMethod =
                typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public)
                    .MakeGenericMethod(enumerableType);
            object value = context.Select(node => ParseAndBind(propertyAttribute, enumerableType, node));
            value = castTypeMethod.Invoke(null, new[] { value });
            if (ImplementsGenericType(outputType, typeof(List<>))) // List
            {
                var toListMethod =
                    typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(enumerableType);
                value = toListMethod.Invoke(null, new[] { value });
            }
#if IMMUTABLE
            else if (ImplementsGenericType(outputType, typeof(FSharpList<>))) // F# list
            {
                var toFsharplistMethod =
                    typeof(ListModule).GetMethod("OfSeq", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(enumerableType);
                value = toFsharplistMethod.Invoke(null, new[] { value });
            }
#endif
            else // other enumerables
            {
                var toArrayMethod =
                    typeof(Enumerable).GetMethod("ToArray", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(enumerableType);
                value = toArrayMethod.Invoke(null, new[] { value });
            }
            return value;
        }

        private static bool ImplementsGenericType(Type type, Type interfaceType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
                return true;
            return type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == interfaceType
            );
        }

        private static object ParseAndBind(BaseHtmlAttribute attribute, Type objectType, CQ nodeForProperty)
        {
            // Parse
            var parsedValue = ParserFactory.Instance.GetParser(attribute.GetType()).Parse(nodeForProperty, attribute);

            // Bind
            var boundValue = BinderFactory.Instance.GetBinder(objectType).Handle(parsedValue, nodeForProperty, objectType, attribute);
            return boundValue;
        }
    }
}
