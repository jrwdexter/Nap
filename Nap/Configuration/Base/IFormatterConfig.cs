using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Formatters.Base;

namespace Nap.Configuration
{
	/// <summary>
	/// Represents a single formatter's configuration.
	/// </summary>
	public interface IFormatterConfig
	{
		/// <summary>
		/// Gets or sets the content type (MIME)that this formatter configuration is for.
		/// </summary>
		string ContentType { get; set; }

		/// <summary>
		/// Gets or sets the full name of the type of the formatter.
		/// </summary>
		/// <example><c>Nap.Formatters.NapJsonFormatter</c></example>
		string FormatterType { get; set; }

		/// <summary>
		/// Gets the instance of the formatter corresponding to <see cref="FormatterType"/>.
		/// </summary>
		/// <returns>An instance of the formatter corresponding to <see cref="FormatterType"/>.</returns>
		INapFormatter GetFormatter();
	}
}