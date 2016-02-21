using System;
using System.Text;

namespace Nap
{
    public partial class NapRequest : IAuthenticatedNapRequestComponent
    {
        /// <summary>
        /// Configures the <see cref="NapRequest"/> for basic authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for basic authentication.</param>
        /// <param name="password">The plaintext password to use for basic authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        public IAdvancedNapRequestComponent Basic(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (_headers.ContainsKey("Authorization"))
                throw new ArgumentException("Authorization header already exists.");

            var hash = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            _headers.Add("Authorization", $"Basic {hash}");
            return this;
        }

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for SAML authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for SAML authentication.</param>
        /// <param name="password">The plaintext password to use for SAML authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        public IAdvancedNapRequestComponent SAML(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for OAuth1 authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for OAuth1 authentication.</param>
        /// <param name="password">The plaintext password to use for OAuth1 authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        public IAdvancedNapRequestComponent OAuth1(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configures the <see cref="NapRequest"/> for OAuth2 authentication, using plaintext <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">The plaintext username to use for OAuth2 authentication.</param>
        /// <param name="password">The plaintext password to use for OAuth2 authentication.</param>
        /// <returns>An <see cref="IAdvancedNapRequestComponent"/> for further advanced request configuration.</returns>
        public IAdvancedNapRequestComponent OAuth2(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
