using Nap.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception that gets thrown when something occurs with the response.
    /// </summary>
    public class NapResponseException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapResponseException"/> class.
        /// </summary>
        public NapResponseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapResponseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with configuration was.</param>
        public NapResponseException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapResponseException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with configuration was.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NapResponseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
