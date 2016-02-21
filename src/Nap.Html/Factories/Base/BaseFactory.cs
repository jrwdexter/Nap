using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nap.Html.Factories.Base
{
	/// <summary>
	/// The base factory, providing functionality for inheriting factories to populate themselves with discovered classes.
	/// </summary>
	/// <typeparam name="T">The type of item this factory provides.</typeparam>
	public abstract class BaseFactory<T>
	{
		private readonly IReadOnlyCollection<T> _values;

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlBinderFactory"/> class.
		/// </summary>
		protected BaseFactory()
		{
			var binderTypes = typeof(BaseFactory<>).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
			var binderInstances = binderTypes.Select(Activator.CreateInstance).OfType<T>().ToList();
			_values = new ReadOnlyCollection<T>(binderInstances);
		}

		/// <summary>
		/// Gets the collection of binders that are available for use.
		/// </summary>
		/// <value>
		/// The collection of binders that are available for use.
		/// </value>
		protected IReadOnlyCollection<T> Values => _values;
	}
}
