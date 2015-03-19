using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Nap.Html.Attributes.Base;
using Nap.Html.Binders.Base;
using Nap.Html.Enum;
using Nap.Html.Exceptions;
using Nap.Html.Factories;

namespace Nap.Html.Binders
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
		public object Handle(string input, HtmlNode context, Type outputType)
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
				var doc = new HtmlDocument();
				doc.LoadHtml(input);
				context = doc.DocumentNode;
			}

			var properties = outputType.GetProperties().Where(p => p.CanWrite);
			var toReturn = Activator.CreateInstance(outputType);

			foreach (var property in properties)
			{
				var attribute = property.GetCustomAttributes(typeof(BaseHtmlAttribute), true).FirstOrDefault() as BaseHtmlAttribute;
				if (attribute != null)
				{
					var enumerableInterface = property.PropertyType.GetInterfaces().FirstOrDefault(i => i == typeof(IEnumerable<>));
					var isEnumerable = enumerableInterface != null;

					// Find
					var typeOfFinder = isEnumerable ? typeof(IEnumerable<HtmlNode>) : typeof(HtmlNode);
					var nodeForProperty = FinderFactory.Instance.GetFinder(typeOfFinder).Find(context, attribute.Selector);

					// Verify
					if (nodeForProperty != null)
					{
						object value;
						var nodes = nodeForProperty as IEnumerable<HtmlNode>;
						var singleNode = nodeForProperty as HtmlNode;
						
						if (nodes != null && isEnumerable)
						{
							// Enumerable case
							var enumerableType = enumerableInterface.GetGenericArguments().First();
							value = nodes.Select(node => ParseAndBind(attribute, enumerableType, node));
						}
						else if (singleNode != null)
						{
							// Single case
							value = ParseAndBind(attribute, property.PropertyType, (HtmlNode)nodeForProperty);
						}
						else
						{
							throw new NapBindingException($"Unknown finder type found: {nodeForProperty.GetType().FullName}");
						}

						property.SetValue(toReturn, value);
					}
				}
			}

			return toReturn;
		}

		private static object ParseAndBind(BaseHtmlAttribute attribute, Type objectType, HtmlNode nodeForProperty)
		{
			// Parse
			var parsedValue = ParserFactory.Instance.GetParser(attribute.GetType()).Parse(nodeForProperty, attribute);

			// Bind
			var boundValue = BinderFactory.Instance.GetBinder(objectType).Handle(parsedValue, nodeForProperty, objectType);
			return boundValue;
		}
	}
}
