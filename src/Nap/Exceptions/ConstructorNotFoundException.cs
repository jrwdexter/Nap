using System;

using Nap.Exceptions.Base;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception that gets thrown when no constructor is found.
    /// </summary>
    public class ConstructorNotFoundException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorNotFoundException"/> class.
        /// </summary>
        public ConstructorNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes why the constructor was not found.</param>
        public ConstructorNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes why the constructor was not found.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConstructorNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
