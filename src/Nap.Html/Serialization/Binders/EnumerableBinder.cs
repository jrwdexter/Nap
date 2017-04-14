using System;
using System.Collections;
using System.Collections.Generic;
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
    public class EnumerableBinder : IEnumerableBinder
    {
        public object Handle(IEnumerable<CQ> input, Type outputType)
        {
            return Handle(input, outputType, null);
        }

        public object Handle(IEnumerable<CQ> input, Type outputType, BaseHtmlAttribute propertyAttribute)
        {
            // TODO: Cache reflection methods

            // Enumerable case
            var enumerableType = outputType.GetGenericArguments().First();
            var castTypeMethod =
                typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public)
                    .MakeGenericMethod(enumerableType);
            object value = input.Select(node => ParseAndBind(propertyAttribute, enumerableType, node));
            value = castTypeMethod.Invoke(null, new[] { value });
            if (ImplementsGenericInterface(outputType, typeof(List<>))) // List
            {
                var toListMethod =
                    typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(enumerableType);
                value = toListMethod.Invoke(null, new[] { value });
            }
#if IMMUTABLE
            else if (ImplementsGenericInterface(outputType, typeof(FSharpList<>))) // F# list
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
                    typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod(enumerableType);
                value = toArrayMethod.Invoke(null, new[] { value });
            }
            return value;
        }

        private static bool ImplementsGenericInterface(Type type, Type interfaceType)
        {
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
