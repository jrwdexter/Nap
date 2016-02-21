using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Nap.Html.Formats.Base;

namespace Nap.Html.Formats
{
	/// <summary>
	/// Handle a numerical element that may be displayed as a currency (dollar) amount.
	/// </summary>
	public class DollarBindingFormat : IBindingFormat
	{
		private readonly NumberFormatInfo _formatInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="DollarBindingFormat"/> class.
		/// </summary>
		public DollarBindingFormat()
		{
			_formatInfo = new NumberFormatInfo();
			_formatInfo.NumberNegativePattern = 0;
			_formatInfo.CurrencySymbol = "$";
		}

		/// <summary>
		/// Gets the basic format information to bind percentage numbers.
		/// </summary>
		public NumberFormatInfo FormatInfo => _formatInfo;

		/// <summary>
		/// Gets the number style <see cref="NumberStyle.Number"/>.
		/// </summary>
		public NumberStyles NumberStyle
		{
			get
			{
				return NumberStyles.Currency;
			}
		}

		/// <summary>
		/// Parses the currency string to be castable to a number.
		/// </summary>
		/// <param name="input">The input string to parse.</param>
		/// <returns>The input string with percentage signs removed</returns>
		public string ParseToNumber(string input)
		{
			return input;
		}
	}
}
