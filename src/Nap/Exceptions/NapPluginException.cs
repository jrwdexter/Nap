using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nap.Exceptions.Base;

namespace Nap.Exceptions
{
    /// <summary>
    /// An exception that is thrown when there is an issue with a plugin registered in nap.
    /// </summary>
    public class NapPluginException : NapException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NapPluginException"/> class.
        /// </summary>
        public NapPluginException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapPluginException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with a plugin was.</param>
        public NapPluginException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NapPluginException"/> class.
        /// </summary>
        /// <param name="message">The message that describes what the issue with a plugin was.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NapPluginException(string message, Exception innerException) : base(message, innerException) { }
    }
}
