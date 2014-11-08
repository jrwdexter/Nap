using System;
using System.Collections.Generic;
using System.Linq;

namespace Napper
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
