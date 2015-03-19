using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Nap.Html.Binders;
using Nap.Html.Binders.Base;
using Nap.Html.Exceptions;
using Nap.Html.Factories.Base;

namespace Nap.Html.Factories
{
	/// <summary>
	/// Describes the factory that is used to find binders to handle various types.
	/// </summary>
	public class BinderFactory : BaseFactory<IBinder>, IBinderFactory
	{
		private static readonly object _padlock = new object();
		private static IBinderFactory _instance;
		private readonly IDictionary<Type, IBinder> _cachedBinders = new Dictionary<Type, IBinder>();

		/// <summary>
		/// Gets the single, thread-safe instance of the <see cref="IBinderFactory" />.
		/// </summary>
		/// <value>
		/// The single, thread-safe instance of the <see cref="IBinderFactory" />.
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
							_instance = new BinderFactory();
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
				binder = Values.FirstOrDefault(b => BinderMatchesType(b, type));
				if (binder == null && type.IsPrimitive)
					binder = Values.FirstOrDefault(b => b is PrimitiveBinder);
				if (binder == null && type.IsClass)
					binder = Values.FirstOrDefault(b => b is ObjectBinder);

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
		[Pure]
		private bool BinderMatchesType(IBinder binder, Type type)
		{
			if (binder == null)
				throw new ArgumentNullException(nameof(binder));
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			var binderInterface = binder.GetType().GetInterfaces().FirstOrDefault(i => i.IsInstanceOfType(typeof(IBinder<>)));
			if (binderInterface == null)
				return false;

			return type.IsAssignableFrom(binderInterface.GetGenericArguments().First());
		}
	}
}