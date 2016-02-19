using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsQuery;
using CsQuery.ExtensionMethods;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Factories;
using Nap.Html.Serialization.Binders.Base;
using Nap.Html.Serialization.Contracts;
using Nap.Html.Serialization.Contracts.Base;

namespace Nap.Html.Serialization.Binders
{
    public class ObjectBinder : IBinder
    {
        /// <summary>
        /// Binds the specified input string to an output object of a certain type, generally a POCO.
        /// The value <see cref="context"/> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
        /// </summary>
        /// <param name="input">The input string.  See <see cref="BindingBehavior"/> for examples on what types of information may be passed in.</param>
        /// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
        /// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
        /// <returns>The output type object created, and filled with the parsed version of the <see cref="input"/>.</returns>
        public object Handle(string input, CQ context, Type outputType)
        {
            return Handle(input, context, outputType, null);
        }

        /// <summary>
        /// Binds the specified input string to an output object of a certain type.
        /// The value <see cref="context" /> must be provided for performance reasons, as to avoid parsing the HTML tree multiple times.
        /// </summary>
        /// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
        /// <param name="context">The context of the currently being-bound input value.  Generally the HTML element corresponding to the input value.</param>
        /// <param name="outputType">The type of output object to generate, whether a POCO, primitive, or other.</param>
        /// <param name="attribute">The optional attribute decorating the property that is currently being handled.</param>
        /// <returns>
        /// The output type object created, and filled with the parsed version of the <see cref="input" />.
        /// </returns>
        public object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
        {
            if (context == null && input == null)
            {
                throw new ArgumentNullException(nameof(input), new ArgumentNullException(nameof(context)));
            }
            if (outputType == null)
            {
                throw new ArgumentNullException(nameof(outputType));
            }

            if (context == null)
            {
                context = new CQ(input);
            }

            var contract = new ObjectContract(outputType);
            var allProperties = outputType.GetProperties();

            var properties = allProperties.Where(p => p.CanWrite)
                .Union(
                    allProperties.Where(p =>
                        contract.Parameters.Any(
                            param => param.Name.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase))));

            // Changing this from setting to creating a hash table of values
            var values = properties.ToDictionary(property => property.Name, property =>
            {
                var propertyAttribute =
                    property.GetCustomAttributes(typeof(BaseHtmlAttribute), true).FirstOrDefault() as BaseHtmlAttribute;
                if (propertyAttribute != null)
                {
                    var enumerableInterface =
                        property.PropertyType.GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                    var isEnumerable = enumerableInterface != null && property.PropertyType != typeof(string);

                    // Find
                    var typeOfFinder = isEnumerable ? typeof(IEnumerable<CQ>) : typeof(CQ);
                    var nodeForProperty = FinderFactory.Instance.GetFinder(typeOfFinder)
                        .Find(context, propertyAttribute.Selector);

                    // Verify
                    if (nodeForProperty != null)
                    {
                        object value;
                        var nodes = nodeForProperty as IEnumerable<CQ>;
                        var singleNode = nodeForProperty as CQ;

                        if (nodes != null && isEnumerable)
                        {
                            // Enumerable case
                            var enumerableType = enumerableInterface.GetGenericArguments().First();
                            var castTypeMethod =
                                typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public)
                                    .MakeGenericMethod(enumerableType);
                            var toListMethod =
                                typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public)
                                    .MakeGenericMethod(enumerableType);
                            value = nodes.Select(node => ParseAndBind(propertyAttribute, enumerableType, node));
                            value = toListMethod.Invoke(null, new[] { castTypeMethod.Invoke(null, new[] { value }) });
                        }
                        else if (singleNode != null)
                        {
                            // Single case
                            value = ParseAndBind(propertyAttribute, property.PropertyType, (CQ)nodeForProperty);
                        }
                        else
                        {
                            throw new NapBindingException($"Unknown finder type found: {nodeForProperty.GetType().FullName}");
                        }

                        return value;
                    }
                }

                // Return default type otherwise
                return property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
            });

            // Construct using a greedy constructor mechanis
            object toReturn;
            if (TryCreateNewObject(outputType, values, out toReturn))
            {
                foreach (var value in values)
                {
                    // TODO: Add contract.Properties to cache PropertyInfos
                    var pi =
                        outputType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .FirstOrDefault(p => p.Name.Equals(value.Key, StringComparison.InvariantCultureIgnoreCase));
                    if(pi?.CanWrite ?? false)
                        pi.SetValue(toReturn, value.Value);
                }
            }

            return toReturn;
        }

        private static bool TryCreateNewObject(Type outputType, IReadOnlyDictionary<string, object> values, out object newObject)
        {
            // Create a default instance if one such constructor exists
            var contract = new ObjectContract(outputType); // TODO: Change this to some sort of factory for caching
            if (contract.HasDefaultConstructor)
            {
                newObject = contract.DefaultConstructor.Invoke(null);
                return true;
            }
            if (contract.ParameterizedConstructor != null)
            {
                var parameters = contract.ParameterizedConstructor.GetParameters();
                var orderedProperties =
                    values.Where(v => parameters.Any(p => p.Name.Equals(v.Key, StringComparison.InvariantCultureIgnoreCase)))
                        .OrderBy(v =>
                            parameters.IndexOf(parameters.First(p => p.Name.Equals(v.Key, StringComparison.InvariantCultureIgnoreCase))))
                        .Select(kv => kv.Value);
                newObject = contract.ParameterizedConstructor.Invoke(orderedProperties.ToArray());
                return true;
            }

            newObject = null;
            return false;
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
