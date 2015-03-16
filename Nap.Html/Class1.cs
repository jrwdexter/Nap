using System.Collections.Generic;

public interface IBinderFactory
{
	public IBinder GetBinder<T>() where T : IHtmlAttribute;

	public IBinder GetBinder(Type type);

	public IBinder GetBinderForModel(object model);

	public IHtmlPropertyBinder GetPropertyBinder<T>() where T : IHtmlAttribute;

	public IHtmlPropertyBinder GetPropertyBinder(Type type);

	public IHtmlPropertyBinder GetPropertyBinderForModel(PropertyInfo propertyInfo);
}

public class HtmlBinderFactory : IBinderFactory
{
	private readonly IReadOnlyCollection<IBinder> _binders;
	private static IBinderFactory _instance;
	private static readonly object _padlock = new object();
	private readonly IDictionary<Type, IBinder> _cachedBinders = new Dictionary<Type, IBinder>();

	public HtmlBinderFactory()
	{
		_binders = typeof(HtmlBinderFactory).Assembly.GetTypes().Where(t => typeof(IBinder<T>).IsAssignableFrom(t)).ToList();
	}

	public Binders { get; } => _binders;

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

public IBinder GetBinder<T>() where T : IHtmlAttribute => GetBinder(typeof(T));

public IBinder GetBinder(Type type)
{
	if (type == null)
		throw new ArgumentNullException(nameof(type));

	IBinder binder;
	if (!_cachedBinders.TryGetValue(type, out binder))
	{
		binder = (IBinder)Activator.CreateNew(_binders.FirstOrDefault(b => BinderMatchesType(b, type)));
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

public interface IFinder
{
	HtmlNode FindNode(HtmlNode currentNode, string selector);
}

public interface IParser<T> where T : BaseHtmlAttribute
{
	string Parse(HtmlNode node, T attributeInstance);
}

public class HtmlElementParser : IParser<HtmlElementAttribute>
{
	protected string Parse(HtmlElement node, HtmlElementAttribute attributeInstance)
	{
		switch (attributeInstance.BindingBehavior)
		{
			case BindingBehavior.InnerText:
				return inputElement.InnerText;
			case BindingBehavior.InnerHtml:
				return inputElement.InnerHtml;
		}
	}
}

public interface IBinder
{
	public object Handle(HtmlElement inputElement, Type outputType);
}

public interface IBinder<T> : IBinder
{
	public T Handle(HtmlElement inputElement);
}

public abstract class BaseBinder : IBinder
{
	protected TAttribute GetAttribute(Type type)
	{
		var htmlAttribute = outputType.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(TAttribute)) as TAttribute;
		if (htmlAttribute == null)
		{
			throw new NapHtmlConfigurationException($"Type {outputType.FullName} is not decorated with a")
        }

		return htmlAttribute;
	}
}

public abstract class BaseHtmlBinder<T> : BaseBinder, IBinder<T>
{
	public abstract object Handle(HtmlElement inputElement, BaseHtmlAttribute attributeInstance);

	public virtual T Handle<T>(HtmlElement inputElement, attributeInstance) where T
    {
		return (T)Handle(inputElement, typeof(outputType));
	}
}

public abstract class BaseHtmlElementBinder<TOutput> : BaseHtmlBinder<HtmlElementAttribute, TOutput>
{
}

public class StringBinder : BaseHtmlElementBinder<string>
{
	public override object Handle(HtmlElement inputElement, BaseHtmlAttribute attributeInstance)
	{
		return ParserFactory.Instance.GetParser(attributeInstance).Parse(inputElement);
	}
}

public class StringBinder : BaseHtmlElementBinder<int>
{
}

public class StringBinder : BaseHtmlElementBinder<>
{
}

public class StringBinder : BaseHtmlElementBinder<string>
{
}

public HtmlModelBinder : BaseHtmlBinder<HtmlModel>
{
    public object Handle(HtmlElement inputElement, Type outputType)
{
	var attribute = GetAttribute(outputType);
	var toReturn = Activator.CreateNew(outputType);

	var properties = outputType.GetProperties().Where(p => typeof(BaseHtmlAttribute).IsAssignableFrom(p.Type));

	foreach (var property in properties)
	{
		var binder = HtmlbinderFactory.Instance.GetPropertyBinder(property);
		binder.Handle()
        }
}
}