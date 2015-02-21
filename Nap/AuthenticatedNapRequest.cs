using System;

namespace Nap
{
    internal partial class NapRequest : IAuthenticatedNapRequestComponent
    {
        private bool _authorizationSet = false;

        public IAdvancedNapRequestComponent Basic(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (_headers.ContainsKey("Authorization"))
                throw new ArgumentException("Authorization header already exists.");

            var hash = Convert.ToBase64String("\{username}:\{password}");
            _headers.Add("Authorization", "Basic \{hash}");
            return this;
        }

        public IAdvancedNapRequestComponent SAML(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IAdvancedNapRequestComponent OAuth1(string username, string password)
        {
            throw new NotImplementedException();
        }

        public IAdvancedNapRequestComponent OAuth2(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
