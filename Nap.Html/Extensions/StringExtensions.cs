using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Html.Extensions
{
	/// <summary>
	/// A class to contain some common string extensions for operating on HTML.
	/// </summary>
	internal static class StringExtensions
	{

		/// <summary>
		/// Method to remove all common InnerText cruft.
		/// </summary>
		/// <param name="toTrim">The string to trim.</param>
		/// <returns>The string with many pieces of cruft removed (spaces, returns, non-breaking spaces, etc).</returns>
		internal static string TrimAll(this string toTrim)
		{
			return toTrim?.Replace("&nbsp;", " ")?.Trim();
		}
	}
}
