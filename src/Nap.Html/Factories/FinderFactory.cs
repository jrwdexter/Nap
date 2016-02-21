using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Exceptions;
using Nap.Html.Factories.Base;
using Nap.Html.Finders.Base;

namespace Nap.Html.Factories
{
    /// <summary>
    /// Describes the factory that is used to find finders to locate various types of nodes.
    /// </summary>
    public class FinderFactory : BaseFactory<IFinder>, IFinderFactory
    {
        private static readonly object _padlock = new object();
        private static IFinderFactory _instance;
        private readonly IDictionary<Type, IFinder> _cachedFinders = new Dictionary<Type, IFinder>();

        /// <summary>
        /// Gets the single, thread-safe instance of the <see cref="IFinderFactory" />.
        /// </summary>
        /// <value>
        /// The single, thread-safe instance of the <see cref="IFinderFactory" />.
        /// </value>
        public static IFinderFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padlock)
                    {
                        if (_instance == null)
                            _instance = new FinderFactory();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets the finder for a specific type of return object (eg Enumerable or non-enumerable).
        /// </summary>
        /// <typeparam name="T">The type of object to retrieve a finder for.</typeparam>
        /// <returns>The finder implementaiton corresponding to the specified type.</returns>
        public IFinder<T> GetFinder<T>() => (IFinder<T>)GetFinder(typeof(T));

        /// <summary>
        /// Gets the finder for a specific type of return object (eg Enumerable or non-enumerable).
        /// </summary>
        /// <param name="type">The type of object to retrieve a finder for.</param>
        /// <returns>The finder implementaiton corresponding to the specified type.</returns>
        public IFinder GetFinder(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            IFinder finder;
            if (!_cachedFinders.TryGetValue(type, out finder))
            {
                lock (_padlock)
                {
                    if (!_cachedFinders.TryGetValue(type, out finder))
                    {
                        finder = Values.FirstOrDefault(b => FinderMatchesType(b, type));

                        if (finder == null)
                            throw new NapFindingException($"No finder found that returns type {type.FullName}.");

                        _cachedFinders.Add(type, finder);
                    }
                }
            }

            return finder;
        }

        /// <summary>
        /// Determines if <paramref name="finder"/> can be used to find <paramref name="type"/>.
        /// </summary>
        /// <param name="finder">The finder to test.</param>
        /// <param name="type">The type of finder to return.</param>
        /// <returns>True if <see name="finder"/> can be used to handle the specified <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either parameter is null.</exception>
        [Pure]
        private bool FinderMatchesType(IFinder finder, Type type)
        {
            if (finder == null)
                throw new ArgumentNullException(nameof(finder));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var finderInterface = finder.GetType().GetInterfaces().FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IFinder<>));
            if (finderInterface == null)
                return false;

            return type.IsAssignableFrom(finderInterface.GetGenericArguments().First());
        }
    }
}
