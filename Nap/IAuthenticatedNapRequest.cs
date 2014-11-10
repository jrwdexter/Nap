using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Napper
{
    internal partial class NapRequest : IAuthenticatedNapRequestComponent
    {
        public IAdvancedNapRequestComponent Basic(string username, string password)
        {
            return this;
        }
    }
}
