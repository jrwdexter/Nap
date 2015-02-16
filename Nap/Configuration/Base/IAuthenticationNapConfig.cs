using System;
using System.Collections.Generic;
using System.Linq;

namespace Nap.Configuration
{
    /// <summary>
    /// Credential configuration propreties for Nap REST requests.
    /// </summary>
    public interface IAuthenticationNapConfig
    {
        /// <summary>
        /// Gets or sets the username to use for authenticaiton.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets the password to use for authentication.
        /// </summary>
        string Password { get; set; }
    }
}