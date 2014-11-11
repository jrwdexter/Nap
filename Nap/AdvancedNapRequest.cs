using System;

namespace Nap
{
    internal partial class NapRequest : IAdvancedNapRequestComponent
    {
        public IAdvancedNapRequestComponent UseProxy(Uri uri)
        {
            _config.Advanced.Proxy = uri;
            return this;
        }

        public IAuthenticatedNapRequestComponent Authentication
        {
            get { return this; }
        }
    }
}
