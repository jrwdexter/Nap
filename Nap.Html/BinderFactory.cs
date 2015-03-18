using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Binders.Base;
using Nap.Html.Exceptions;

namespace Nap.Html
{
	/// <summary>
	/// Describes the factory that is used to find binders to handle various types.
	/// </summary>
	public class HtmlBinderFactory : IBinderFactory
	{
		private readonly IReadOnlyCollection<IBinder> _binders;
		private static readonly object _padlock = new object();
		private static IBinderFactory _instance;
		private readonly IDictionary<Type, IBinder> _cachedBinders = new Dictionary<Type, IBinder>();

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlBinderFactory"/> class.
		/// </summary>
		public HtmlBinderFactory()
		{
			var binderTypes = typeof(HtmlBinderFactory).Assembly.GetTypes().Where(t => typeof(IBinder).IsAssignableFrom(t));
			var binderInstances = binderTypes.Select(Activator.CreateInstance).OfType<IBinder>().ToList();
			_binders = new ReadOnlyCollection<IBinder>(binderInstances);
		}

		/// <summary>
		/// Gets the collection of binders that are available for use.
		/// </summary>
		/// <value>
		/// The collection of binders that are available for use.
		/// </value>
		public IReadOnlyCollection<IBinder> Binders => _binders;

		/// <summary>
		/// Gets the single, thread-safe instance of the <see cref="HtmlBinderFactory" />.
		/// </summary>
		/// <value>
		/// The single, thread-safe instance of the <see cref="HtmlBinderFactory" />.
		/// </value>
		public static IBinderFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_padlock)
					{
						if (_instance == null)
							_instance = new HtmlBinderFactory();
					}
				}

				return _instance;
			}
		}

		/// <summary>
		/// Gets the binder for a specific type of object (eg string, int, etc).
		/// </summary>
		/// <typeparam name="T">The type of object to retrieve a binder for.</typeparam>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		public IBinder GetBinder<T>() => GetBinder(typeof(T));

		/// <summary>
		/// Gets the binder for a specific type of object (eg string, int, etc).
		/// </summary>
		/// <param name="type">The type of object to retrieve a binder for.</param>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
		/// <exception cref="NapBindingException">Thrown if no binder is found that matches the specified <paramref name="type"/>.</exception>
		[Pure]
		public IBinder GetBinder(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			IBinder binder;
			if (!_cachedBinders.TryGetValue(type, out binder))
			{
				binder = _binders.FirstOrDefault(b => BinderMatchesType(b, type));
				if (binder == null)
					throw new NapBindingException($"No binder found that matches type {type.FullName}.");

				_cachedBinders.Add(type, binder);
			}

			return binder;
		}

		/// <summary>
		/// Gets the binder for a specific object (eg string, int, etc).
		/// </summary>
		/// <param name="model">The object to retrieve a binder for.</param>
		/// <returns>The binder implementaiton corresponding to the specified type.</returns>
		/// <exception cref="ArgumentNullException">Thrown if model is null.</exception>
		[Pure]
		public IBinder GetBinderForModel(object model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			return GetBinder(model.GetType());
		}

		/// <summary>
		/// Determines if <paramref name="binder"/> can be used to bind <paramref name="type"/>.
		/// </summary>
		/// <param name="binder">The binder to test.</param>
		/// <param name="type">The type to test against.</param>
		/// <returns>True if <see name="binder"/> can be used to handle the specified <paramref name="type"/>.</returns>
		/// <exception cref="ArgumentNullException">Thrown if either parameter is null.</exception>
		/// <exception cref="NapBindingException">Thrown if <paramref name="binder"/> does not implement <see cref="IBinder{T}"/>.</exception>
		[Pure]
		private bool BinderMatchesType(IBinder binder, Type type)
		{
			if (binder == null)
				throw new ArgumentNullException(nameof(binder));
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			var binderInterface = binder.GetType().GetInterfaces().FirstOrDefault(i => i.IsInstanceOfType(typeof(IBinder<>)));
			if (binderInterface == null)
				throw new NapBindingException($"Type {type.FullName} does not implement {typeof(IBinder<>).FullName}");

			return type.IsAssignableFrom(binderInterface.GetGenericArguments().First());
		}
	}
}