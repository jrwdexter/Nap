using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Html.Formats.Base
{
	/// <summary>
	/// Describes a behavior for binding a numeric result.
	/// </summary>
	public interface IBindingFormat
	{
		/// <summary>
		/// Parses the string to be castable to a number.
		/// In implemented methods, returning the input directly is acceptable.
		/// </summary>
		/// <param name="input">The input string to parse.</param>
		/// <returns>An optionally parsed version of the input string.</returns>
		string ParseToNumber(string input);

		/// <summary>
		/// Gets the format information for the number to be bound.
		/// </summary>
		NumberFormatInfo FormatInfo { get; }

		/// <summary>
		/// Gets the number style (eg. <see cref="NumberStyles.Currency"/>) for the number to be bound.
		/// </summary>
		NumberStyles NumberStyle { get; }
	}
}
