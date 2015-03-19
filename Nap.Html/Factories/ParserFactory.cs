using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Attributes.Base;
using Nap.Html.Exceptions;
using Nap.Html.Factories.Base;
using Nap.Html.Parsers.Base;

namespace Nap.Html.Factories
{
	/// <summary>
	/// Describes the factory that is used to get parsers used to extract strings for future binding of properties decorated with attributes inheriting from <see cref="BaseHtmlAttribute"/>.
	/// </summary>
	public class ParserFactory : BaseFactory<IParser>, IParserFactory
	{
		private static readonly object _padlock = new object();
		private static IParserFactory _instance;
		private readonly IDictionary<Type, IParser> _cachedParsers = new Dictionary<Type, IParser>();

		/// <summary>
		/// Gets the single, thread-safe instance of the <see cref="IFinderFactory" />.
		/// </summary>
		/// <value>
		/// The single, thread-safe instance of the <see cref="IFinderFactory" />.
		/// </value>
		public static IParserFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_padlock)
					{
						if (_instance == null)
							_instance = new ParserFactory();
					}
				}

				return _instance;
			}
		}

		/// <summary>
		/// Gets the parser for a specific attribute decorator (ones that inherit from <see cref="BaseHtmlAttribute"/>).
		/// </summary>
		/// <typeparam name="T">The type of attribute to retrieve the parser for.</typeparam>
		/// <returns>The parser implementaiton corresponding to the specified type.</returns>
		public IParser<T> GetParser<T>() where T : BaseHtmlAttribute => (IParser<T>)GetParser(typeof(T));

		/// <summary>
		/// Gets the parser for a specific type of return object (eg Enumerable or non-enumerable).
		/// </summary>
		/// <param name="type">The type of object to retrieve a parser for.</param>
		/// <returns>The parser implementaiton corresponding to the specified type.</returns>
		public IParser GetParser(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			IParser finder;
			if (!_cachedParsers.TryGetValue(type, out finder))
			{
				finder = Values.FirstOrDefault(b => ParserMatchesType(b, type));

				if (finder == null)
					throw new NapParsingException($"No parser found that can handle type {type.FullName}.");

				_cachedParsers.Add(type, finder);
			}

			return finder;
		}

		/// <summary>
		/// Determines if <paramref name="parser"/> can be used to parse items decorated with <paramref name="type"/>.
		/// </summary>
		/// <param name="parser">The parser to test.</param>
		/// <param name="type">The type of parser to return.</param>
		/// <returns>True if <see name="parser"/> can be used to parse items decorated with attributes of the type <paramref name="type"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if either parameter is null.</exception>
		[Pure]
		private bool ParserMatchesType(IParser parser, Type type)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			var parserInterface = parser.GetType().GetInterfaces().FirstOrDefault(i => i.IsInstanceOfType(typeof(IParser<>)));
			if (parserInterface == null)
				return false;

			return type.IsAssignableFrom(parserInterface.GetGenericArguments().First());
		}
	}
}
