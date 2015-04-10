using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Exceptions;
using Nap.Formatters.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Describes an implementation of the <see cref="IFormatterConfig"/> interface for non-*.config implementations.
	/// </summary>
	public class EmptyFormatterConfig : IFormatterConfig
	{
		private readonly object _padlock = new object();
		private INapFormatter _formatterInstance;

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyFormatterConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap formatter configuration applies to.</param>
		/// <param name="formatterType">The formatter type used to initialize the class.</param>
		public EmptyFormatterConfig(string contentType, string formatterType)
		{
			ContentType = contentType;
			FormatterType = formatterType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyFormatterConfig"/> class.
		/// </summary>
		/// <param name="formatterInstance">The formatter instance used to initialize the class.</param>
		public EmptyFormatterConfig(INapFormatter formatterInstance)
		{
			if (formatterInstance == null)
				throw new ArgumentNullException(nameof(formatterInstance));

			ContentType = formatterInstance.ContentType;
			_formatterInstance = formatterInstance;
			FormatterType = formatterInstance.GetType().FullName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyFormatterConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap formatter configuration applies to.</param>
		/// <param name="formatterInstance">The formatter instance used to initialize the class.</param>
		public EmptyFormatterConfig(string contentType, INapFormatter formatterInstance)
		{
			if (contentType == null)
				throw new ArgumentNullException(nameof(contentType));
			if (formatterInstance == null)
				throw new ArgumentNullException(nameof(formatterInstance));

			ContentType = contentType;
			_formatterInstance = formatterInstance;
			FormatterType = formatterInstance.GetType().FullName;
		}

		/// <summary>
		/// Gets or sets the request format that this formatter configuration is for.  See <see cref="RequestFormat"/>.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of the formatter.
		/// </summary>
		/// <example><c>Nap.Formatters.NapJsonFormatter</c></example>
		public string FormatterType { get; set; }

		/// <summary>
		/// Gets the instance of the formatter corresponding to <see cref="IFormatterConfig.FormatterType"/>.
		/// </summary>
		/// <returns>An instance of the formatter corresponding to <see cref="IFormatterConfig.FormatterType"/>.</returns>
		public INapFormatter GetFormatter()
		{
			if (FormatterType == null && _formatterInstance == null)
				throw new NapConfigurationException("FormatterType is null, and FormatterConfiguration was not instantiated with an INapFormatter.");

			if (_formatterInstance == null)
			{
				lock (_padlock)
				{
					if (_formatterInstance == null)
						_formatterInstance = (INapFormatter)Activator.CreateInstance(Type.GetType(FormatterType));
				}
			}

			return _formatterInstance;
		}
	}
}
