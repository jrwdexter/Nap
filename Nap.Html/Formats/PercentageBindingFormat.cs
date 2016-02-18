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
	/// Handle a numerical element that may be displayed as a percentage.
	/// </summary>
	public class PercentageBindingFormat : IBindingFormat
	{
		/// <summary>
		/// Gets the basic format information to bind percentage numbers.
		/// </summary>
		public NumberFormatInfo FormatInfo
		{
			get
			{
				return new NumberFormatInfo();
			}
		}

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
		/// Parses the percentage string to be castable to a number.
		/// </summary>
		/// <param name="input">The input string to parse.</param>
		/// <returns>The input string with percentage signs removed</returns>
		public string ParseToNumber(string input)
		{
			return input?.Replace("%", string.Empty);
		}
	}
}
