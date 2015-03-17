using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Nap.Html.Attributes;
using Nap.Html.Attributes.Base;
using Nap.Html.Binders.Base;

namespace Nap.Html
{
	public interface IBinderFactory
	{
		IBinder GetBinder<T>();

		IBinder GetBinder(Type type);

		IBinder GetBinderForModel(object model);
	}

	public class HtmlBinderFactory : IBinderFactory
	{
		private readonly IReadOnlyCollection<IBinder> _binders;
		private static IBinderFactory _instance;
		private static readonly object _padlock = new object();
		private readonly IDictionary<Type, IBinder> _cachedBinders = new Dictionary<Type, IBinder>();

		public HtmlBinderFactory()
		{
			var binderTypes = typeof(HtmlBinderFactory).Assembly.GetTypes().Where(t => typeof(IBinder).IsAssignableFrom(t));
			var binderInstances = binderTypes.Select(Activator.CreateInstance).OfType<IBinder>().ToList();
			_binders = new ReadOnlyCollection<IBinder>(binderInstances);
		}

		public IReadOnlyCollection<IBinder> Binders => _binders;

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

		public IBinder GetBinder<T>() => GetBinder(typeof(T));

		public IBinder GetBinder(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			IBinder binder;
			if (!_cachedBinders.TryGetValue(type, out binder))
			{
				binder = _binders.FirstOrDefault(b => BinderMatchesType(b, type));
				if (binder == null)
					throw new NapBindingException(type);

				_cachedBinders.Add(type, binder);
			}

			return binder;
		}

		public IBinder GetBinderForModel(object model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			return GetBinder(model.GetType());
		}

		private bool BinderMatchesType(IBinder binder, Type type)
		{
			var binderInterface = b.GetInterfaces().First(i => i.IsInstanceOfType(typeof(IBinder<T>)))
            return type.IsAssignableFrom(binderInterface.GetGenericArguments().First());
		}
	}
}