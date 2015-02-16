using System;
using System.Collections.Generic;
using System.Linq;

using Nap.Exceptions.Base;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception that gets thrown when there is an issue with Nap's configuration.
    /// </summary>
    public class NapConfigurationException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationException"/> class.
        /// </summary>
        public NapConfigurationException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with configuration was.</param>
        public NapConfigurationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with configuration was.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NapConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}