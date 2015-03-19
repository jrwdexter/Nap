using Nap.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception that is thrown when there is an issue with Nap's serialization or deserialization.
    /// </summary>
    public class NapSerializationException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapSerializationException"/> class.
        /// </summary>
        public NapSerializationException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with serialization was.</param>
        public NapSerializationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with serialization was.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NapSerializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
