using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Nap.Exceptions;
using Nap.Formatters.Base;

namespace Nap.Configuration.Sections
{
    /// <summary>
    /// An implementation of <see cref="IFormatterConfig"/> that allows for use of *.config files.
    /// </summary>
	public class FormatterConfig : ConfigurationElement, IFormatterConfig
	{
		private readonly object _padlock = new object();
		private INapFormatter _formatterInstance;

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatterConfig"/> class.
		/// </summary>
		public FormatterConfig()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatterConfig"/> class.
		/// </summary>
		/// <param name="formatterInstance">The formatter instance used to initialize the class.</param>
		public FormatterConfig(INapFormatter formatterInstance)
		{
			if (formatterInstance == null)
				throw new ArgumentNullException(nameof(formatterInstance));

			ContentType = formatterInstance.ContentType;
			_formatterInstance = formatterInstance;
			FormatterType = formatterInstance.GetType().FullName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatterConfig"/> class.
		/// </summary>
		/// <param name="contentType">The content type this nap formatter configuration applies to.</param>
		/// <param name="formatterInstance">The formatter instance used to initialize the class.</param>
		public FormatterConfig(string contentType, INapFormatter formatterInstance)
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
		/// Gets or sets the content type (MIME)that this formatter configuration is for.
		/// </summary>
		[ConfigurationProperty("contentType", IsRequired = true)]
		public string ContentType
		{
			get { return (string)this["contentType"]; }
			set { this["contentType"] = value; }
		}

		/// <summary>
		/// Gets or sets the full name of the type of the formatter.
		/// </summary>
		/// <example><c>Nap.Formatters.NapJsonFormatter</c></example>
		[ConfigurationProperty("formatterType", IsRequired = true)]
		public string FormatterType
		{
			get { return (string)this["formatterType"]; }
			set { this["formatterType"] = value; }
		}

		/// <summary>
		/// Gets the instance of the formatter corresponding to <see cref="IFormatterConfig.FormatterType"/>.
		/// </summary>
		/// <returns>An instance of the formatter corresponding to <see cref="IFormatterConfig.FormatterType"/>.</returns>
		public INapFormatter GetFormatter()
		{
			if (_formatterInstance == null)
			{
				lock (_padlock)
				{
					if (_formatterInstance == null)
					{
						if (FormatterType == null)
							throw new NapConfigurationException("FormatterType is null, and FormatterConfiguration was not instantiated with an INapFormatter.");

						var formatterType = Type.GetType(FormatterType);
						if (formatterType == null)
							throw new NapConfigurationException($"No formatter found for type name {FormatterType}");

						_formatterInstance = (INapFormatter)Activator.CreateInstance(formatterType);
					}
				}
			}

			return _formatterInstance;
		}

		/// <summary>
		/// Creates a copy of the <see cref="FormatterConfig"/> configuration.
		/// </summary>
		/// <returns>A copy of the current instance.</returns>
		public FormatterConfig Clone()
		{
			return new FormatterConfig { ContentType = ContentType, FormatterType = FormatterType };
		}
	}
}
