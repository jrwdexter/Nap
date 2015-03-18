using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Exceptions.Base;

namespace Nap.Html.Exceptions
{
	/// <summary>
	/// An exception that is thrown when an error occurs in binding.
	/// </summary>
	public class NapBindingException : NapException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NapBindingException"/> class.
		/// </summary>
		/// <param name="message">The message describing why the error occurred.</param>
		public NapBindingException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="NapBindingException"/> class.
		/// </summary>
		/// <param name="message">The message describing why the error occurred.</param>
		/// <param name="innerException">The exception that caused the error.</param>
		public NapBindingException(string message, Exception innerException) : base(message, innerException) { }
	}
}
