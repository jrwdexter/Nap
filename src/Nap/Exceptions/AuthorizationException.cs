using System;

using Nap.Exceptions.Base;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception class that gets thrown when authorization configuration or execution fails for some reason.
    /// </summary>
    public class AuthorizationException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
        /// </summary>
        protected AuthorizationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected AuthorizationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        protected AuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
