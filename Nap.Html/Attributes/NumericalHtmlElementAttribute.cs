using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nap.Html.Exceptions;
using Nap.Html.Formats.Base;

namespace Nap.Html.Attributes
{
	/// <summary>
	/// Describes a numeric-specific binding element, selecting a node for binding and using a specific set of format information.
	/// </summary>
	public class NumericalHtmlElementAttribute : HtmlElementAttribute
	{
		private static readonly ConcurrentDictionary<Type, IBindingFormat> _bindingFormats = new ConcurrentDictionary<Type, IBindingFormat>();
		private IBindingFormat _bindingFormat;
		private readonly object _padlock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="NumericalHtmlElementAttribute"/> class.
		/// </summary>
		/// <param name="selector">The selector to use to find the element.</param>
		public NumericalHtmlElementAttribute(string selector) : base(selector)
		{
		}

		/// <summary>
		/// Gets or sets the type of the binding behavior, which must inherit from <see cref="IBindingFormat"/> and have a parameterless constructor.
		/// </summary>
		public Type BindingFormatType { get; set; }

		/// <summary>
		/// The binding behavior for the numerical element.
		/// </summary>
		public IBindingFormat BindingFormat
		{
			get
			{
				if (BindingFormatType == null) return null;

				if (!_bindingFormats.TryGetValue(BindingFormatType, out _bindingFormat))
				{
					lock (_padlock)
					{
						try
						{

							if (_bindingFormat == null)
								_bindingFormat = (IBindingFormat)Activator.CreateInstance(BindingFormatType);

							_bindingFormats[BindingFormatType] = _bindingFormat;
						}
						catch (InvalidCastException e)
						{
							throw new NapBindingException($"The type specified ({BindingFormatType.FullName}) does not inherit from {typeof(IBindingFormat).FullName}.  Binding cannot continue.", e);
						}
						catch (Exception e)
						{
							throw new NapBindingException($"There was an error creating binding behavior of type {BindingFormatType.FullName}.  See inner exception for details.", e);
						}
					}
				}

				return _bindingFormat;
			}
		}
	}
}
