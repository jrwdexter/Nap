using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using Nap.Html.Formats.Base;

namespace Nap.Html.Formats
{
	/// <summary>
	/// Handle a numerical element that should be displayed as a normal number.
	/// </summary>
	public class DefaultBindingFormat : IBindingFormat
	{
		private readonly NumberFormatInfo _formatInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultBindingFormat"/> class.
		/// </summary>
		public DefaultBindingFormat()
		{
			_formatInfo = new NumberFormatInfo();
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
				return NumberStyles.Number;
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
