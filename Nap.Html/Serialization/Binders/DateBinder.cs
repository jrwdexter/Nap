using System;
using System.Diagnostics.Contracts;
using CsQuery;
using Nap.Html.Attributes.Base;
using Nap.Html.Enum;
using Nap.Html.Serialization.Binders.Base;

namespace Nap.Html.Serialization.Binders
{
	/// <summary>
	/// A simple binder for string types.
	/// </summary>
	public sealed class DateBinder : BaseBinder<DateTime>
	{
		/// <summary>
		/// Binds the specified input string to an output object of type <see cref="string"/>
		/// The value <see cref="context" /> is unused.
		/// </summary>
		/// <param name="input">The input string.  See <see cref="BindingBehavior" /> for examples on what types of information may be passed in.</param>
		/// <param name="context">Unused in this case.</param>
		/// <param name="outputType">Unused in this case.</param>
		/// <param name="attribute">Unused in this case.</param>
		/// <returns>
		/// The output type object created, and filled with the parsed version of the <see cref="input" />.
		/// </returns>
		[Pure]
		public override object Handle(string input, CQ context, Type outputType, BaseHtmlAttribute attribute)
		{
			DateTime dateTime;
			var success = DateTime.TryParse(input, out dateTime);
			if (outputType.IsGenericType && outputType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return success ? (DateTime?)dateTime : null;

			return dateTime;
		}
	}
}
